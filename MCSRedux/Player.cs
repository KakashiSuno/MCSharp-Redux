using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Minecraft_Server
{
    public sealed class Player
    {
        public static List<Player> players = new List<Player>(64);
        public static Dictionary<string, LeftPlayer> left = new Dictionary<string, LeftPlayer>(64);
        public static List<Player> connections = new List<Player>(Properties.players);
        public static byte number { get { return (byte)players.Count; } }
        static System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        Socket socket;
        System.Timers.Timer loginTimer = new System.Timers.Timer(10000);
        System.Timers.Timer pingTimer = new System.Timers.Timer(500);
        byte[] buffer = new byte[0];
        byte[] tempbuffer = new byte[0xFF];
        public bool disconnected = false;

        public string name;
        public string ip;
        public byte id;
        public string color;
        public Group group;
        public bool hidden = false;
        public bool painting = false;
        public byte BlockAction = 0;  //0-Nothing 1-solid 2-lava 3-water 4-active_lava 5 Active_water 6 OpGlass
        //public List<Edit> actions = new List<Edit>(128);
        public byte[] bindings = new byte[128];
        public Level level = Server.mainLevel;
        public bool Loading = true;     //True if player is loading a map.

        public delegate void BlockchangeEventHandler(Player p, ushort x, ushort y, ushort z, byte type);
        public event BlockchangeEventHandler Blockchange = null;
        public void ClearBlockchange() { Blockchange = null; }
        public bool HasBlockchange() { return (Blockchange == null); }
        public object blockchangeObject = null;

        public ushort[] pos = new ushort[3] { 0, 0, 0 };
        ushort[] oldpos = new ushort[3] { 0, 0, 0 };
        ushort[] basepos = new ushort[3] { 0, 0, 0 };
        public byte[] rot = new byte[2] { 0, 0 };
        byte[] oldrot = new byte[2] { 0, 0 };

        bool loggedIn = false;
        public Player(Socket s)
        {
			try
			{
				socket = s;
				ip = socket.RemoteEndPoint.ToString().Split(':')[0];
				Server.Log(ip + " connected.");

				if (Server.bannedIP.Contains(ip)) { Kick("You're banned!"); return; }
				if (connections.Count >= 5) { Kick("Too many connections!"); return; }

				for (byte i = 0; i < 128; ++i)
					bindings[i] = i;

				socket.BeginReceive(tempbuffer, 0, tempbuffer.Length, SocketFlags.None,
									new AsyncCallback(Receive), this);

				loginTimer.Elapsed += delegate
				{
					loginTimer.Stop();
					if (!loggedIn) { Kick("You must login! Try again."); }
					else if (group == Group.Find("superOp")) { SendMessage("Welcome " + name + "! You rule!"); }
					else { SendMessage("Welcome " + name + "! Please use /rules"); }
                    if (File.Exists("wellcome.txt"))
                    {
                        try
                        {
                            List<string> Wellcome = new List<string>();
                            StreamReader wm = File.OpenText("wellcome.txt");
                            while (!wm.EndOfStream)
                                Wellcome.Add(wm.ReadLine());

                            wm.Close();

                            foreach (string w in Wellcome)
                                SendMessage(w);
                        }
                        catch
                        {

                        }
                    }
				}; loginTimer.Start();

				pingTimer.Elapsed += delegate { SendPing(); };
				pingTimer.Start();
				connections.Add(this);
			}
			catch(Exception e)
			{
				Server.ErrorLog(e);
			}
        }
        #region == INCOMING ==
        static void Receive(IAsyncResult result)
        {
            Player p = (Player)result.AsyncState;
            if (p.disconnected)
                return;
			try
			{
				int length = p.socket.EndReceive(result);
				if (length == 0) { p.Disconnect(); return; }

				byte[] b = new byte[p.buffer.Length + length];
				Buffer.BlockCopy(p.buffer, 0, b, 0, p.buffer.Length);
				Buffer.BlockCopy(p.tempbuffer, 0, b, p.buffer.Length, length);

				p.buffer = p.HandleMessage(b);
				p.socket.BeginReceive(p.tempbuffer, 0, p.tempbuffer.Length, SocketFlags.None,
									  new AsyncCallback(Receive), p);
			}
			catch (SocketException)
			{
				p.Disconnect();
			}
			catch (Exception e)
			{
				Server.ErrorLog(e); 
				p.Kick("Error!");
			}
        }
        byte[] HandleMessage(byte[] buffer)
        {
			try
			{
				int length = 0; byte msg = buffer[0];
				// Get the length of the message by checking the first byte
				switch (msg)
				{
					case 0: length = 130; break; // login
					case 5: length = 8; break; // blockchange
					case 8: length = 9; break; // input
					case 13: length = 65; break; // chat
					default: Kick("Unhandled message id \"" + msg + "\"!"); return new byte[0];
				}
				if (buffer.Length > length)
				{
					byte[] message = new byte[length];
					Buffer.BlockCopy(buffer, 1, message, 0, length);

					byte[] tempbuffer = new byte[buffer.Length - length - 1];
					Buffer.BlockCopy(buffer, length + 1, tempbuffer, 0, buffer.Length - length - 1);

					buffer = tempbuffer;

					// Thread thread = null; 
					switch (msg)
					{
						case 0: HandleLogin(message); break;
						case 5: HandleBlockchange(message); break;
						case 8: HandleInput(message); break;
						case 13: HandleChat(message); break;
					}
					//thread.Start((object)message);
					if (buffer.Length > 0)
						buffer = HandleMessage(buffer);
					else
						return new byte[0];
				}
			}
			catch (Exception e)
			{
				Server.ErrorLog(e);
			}
			return buffer;
        }
        void HandleLogin(byte[] message)
        {
			try
			{
				//byte[] message = (byte[])m;
				if (loggedIn)
					return;

				byte version = message[0];
				name = enc.GetString(message, 1, 64).Trim();
				string verify = enc.GetString(message, 65, 32).Trim();
				byte type = message[129];

				if (Server.banned.Contains(name)) { Kick("You're banned!"); return; }
				if (number >= Properties.players) { Kick("Server full!"); return; }
				if (version != Properties.version) { Kick("Wrong version!"); return; }
				if (name.Length > 16 || !ValidName(name)) { Kick("Illegal name!"); return; }

				if (Properties.verify)
				{
					if (verify == "--" || verify != BitConverter.ToString(
						md5.ComputeHash(enc.GetBytes(Properties.salt + name))).
						Replace("-", "").ToLower().TrimStart('0'))
					{
						if (ip != "127.0.0.1")
						{
							Kick("Login failed! Try again."); return;
						}
					}
				}
				Player old = Player.Find(name);
				Server.Log(ip + " logging in as " + name + ".");

				if (old != null)
				{
					if (Properties.verify)
					{
						old.Kick("Someone logged in as you!");
					}
					else { Kick("Already logged in!"); return; }
				}
				left.Remove(name.ToLower());
				SendMotd(); SendMap();

				if (disconnected)
					return;

				loggedIn = true;
				id = FreeId();

				if (Server.operators.Contains(name))
					group = Group.Find("operator");
				else if (Server.builders.Contains(name))
					group = Group.Find("builder");
				else if (Server.superOps.Contains(name))
					group = Group.Find("superop");
				else if (Server.bot.Contains(name))
					group = Group.Find("bots");
				else if (Server.advbuilders.Contains(name))
					group = Group.Find("advbuilder");
				else
					group = Group.standard;

				connections.Remove(this);
				players.Add(this);

				color = group.color;

				if (!checkDev(this))
                {
                    if (!checkSupporter(this))
                    {
                        GlobalChat(this, "&a+ " + color + name + "&e joined the game.", false);
                    }
                    else
                    {
                        GlobalChat(this, "&a+ " + color + name + "&e MCSharp supporter appeared!", false);
                    }
                }
                else
                {
                    GlobalChat(this, "&a+ " + color + name + "&e MCSharp developer appeared!", false);
                }

				
				if (!Properties.console && Server.win != null)
					Server.win.UpdateClientList(players);
				IRCBot.Say(name + " joined the game.");

				//Test code to show wehn people come back with different accounts on the same IP
				string temp = "Lately known as:";
				bool found = false;
				if (ip != "127.0.0.1")
				{
					foreach (var prev in left)
					{
						if (prev.Value.ip == ip)
						{
							found = true;
							temp += " " + prev.Value.name;
						}
					}
					if (found)
					{
						GlobalMessageOps(temp);
						Server.Log(temp);
						IRCBot.Say(temp);       //Tells people in IRC only hopefully
					}
				}

				ushort x = (ushort)((0.5 + level.spawnx) * 32);
				ushort y = (ushort)((1 + level.spawny) * 32);
				ushort z = (ushort)((0.5 + level.spawnz) * 32);
				pos = new ushort[3] { x, y, z }; rot = new byte[2] { level.rotx, level.roty };

				GlobalSpawn(this, x, y, z, rot[0], rot[1], true);
				foreach (Player p in players)
				{
					if (p.level == level && p != this && !p.hidden)
						SendSpawn(p.id,
							p.color + p.name,
							p.pos[0],
							p.pos[1],
							p.pos[2],
							p.rot[0],
							p.rot[1]);
				}
                Loading = false;
			}
			catch (Exception e)
			{
				Server.ErrorLog(e); 
				Player.GlobalMessage("An error occurred: " + e.Message);
			}
        }

        void HandleBlockchange(byte[] message)
        {
            try
            {
                if (group.name == "bots") { return; } //connected bots cant do block changes
                //byte[] message = (byte[])m;
                if (!loggedIn)
                    return;

                ushort x = NTHO(message, 0);
                ushort y = NTHO(message, 2);
                ushort z = NTHO(message, 4);
                byte action = message[6];
                byte type = message[7];

                if (type > 49)
                {
                    Kick("Unknown block type!");
                    return;
                }

                byte b = level.GetTile(x, y, z);
                if (b == Block.Zero) { return; }

                if (group.Permission < level.permissionbuild) 
                { 
                    SendMessage("Your not allowed to edit this map."); 
                    SendBlockchange(x, y, z, b);
                    return; 
                }

                if (Blockchange != null)    //Blockchange actions now have priority, allowing people to /about blocks they cant change
                {
                    Blockchange(this, x, y, z, type);
                    return;
                }

                if (group.Permission == LevelPermission.Guest)
                {
                    if (group.name == "banned") //Just let them think theyre are griefing instead.
                    {
                        return;
                    }

                    int Diff = 0;

                    Diff = Math.Abs((int)(pos[0] / 32) - x);
                    Diff += Math.Abs((int)(pos[1] / 32) - y);
                    Diff += Math.Abs((int)(pos[2] / 32) - z);

                    if (Diff > 8)   //Danger level compensation
                    {
                        if (Diff > 10)  //Too much distance
                        {
                            Server.Log(name + " attempted to build with a " + Diff.ToString() + " distance offset");
                            GlobalMessageOps("To Ops &f-" + color + name + "&f- attempted to build with a "+ Diff.ToString() + " distance offset");
                            Kick("Hacked client.");
                            return;
                        }
                        SendMessage("You cant build that far away.");
                        SendBlockchange(x, y, z, b); return;
                    }

                    if (Properties.antiTunnel)
                    {
                        if (y < level.depth / 2 - Properties.maxDepth)     //Anti tunneling countermeasure
                        {
                            SendMessage("You're not allowed to build this far down!");
                            SendBlockchange(x, y, z, b); return;
                        }
                    }
                }

                if (b == 7)    //Check for client hacker trying to delete adminium
                {
                    if (checkOp())
                    {
                        Server.Log(name + " attempted to delete an adminium block.");
                        GlobalMessageOps("To Ops &f-" + color + name + "&f- attempted to delete an adminium block.");
                        Kick("Hacked client.");
                        return;
                    }
                }

                if (b >= 100 && b != Block.door && b != Block.door2 && b != Block.door3)    //Special blocks only deletable by ops
                {
                    if (checkOp())
                    {
                        SendMessage("You're not allowed to destroy this block!");
                        SendBlockchange(x, y, z, b); 
                        return;
                    }
                    if (b >= 200)    //Special blocks that should never be replaced until they are finished
                    {
                        SendMessage("Block is active, you cant disturb it!");
                        SendBlockchange(x, y, z, b);
                        return;
                    }
                }


                if (!Block.Placable(type))
                { // :3
                    SendMessage("You can't place this block type!");
                    SendBlockchange(x, y, z, b); return;
                }
                if (action > 1) { Kick("Unknown block action!"); }
                type = bindings[type];

                //Ignores updating blocks that are the same and send block only to the player
                if (b == (byte)((painting || action == 1) ? type : 0))
                {
                    if (painting || message[7] != type) { SendBlockchange(x, y, z, b); } return;
                }

                //else

                if (!painting && action == 0)   //player is deleting a block
                {
                    deleteBlock(b, type, x, y, z);
                }
                else    //player is placing a block
                {
                    placeBlock(b, type, x, y, z);
                }
               
            }
            catch (Exception e) 
            { 
                // Don't ya just love it when the server tattles?
                Server.ErrorLog(name + " has triggered a block change error");
                GlobalMessageOps(name + " has triggered a block change error");
                IRCBot.Say(name + " has triggered a block change error");
                Server.ErrorLog(e); Player.GlobalMessage("An error occurred: " + e.Message); 
            }
        }

        private bool checkOp()
        {
            return group.name != "operator" && group.name != "superop";
        }

        private void deleteBlock(byte b, byte type, ushort x, ushort y, ushort z)
        {
            switch (b)
            {
                case Block.door:   //Door
                    if (level.physics != 0)
                    { level.Blockchange(this, x, y, z, (byte)(Block.door_air)); }
                    else
                    { SendBlockchange(x, y, z, b); }
                    break;
                case Block.door2:   //Door2
                    if (level.physics != 0)
                    { level.Blockchange(this, x, y, z, (byte)(Block.door2_air)); }
                    else
                    { SendBlockchange(x, y, z, b); }
                    break;
                case Block.door3:   //Door3
                    if (level.physics != 0)
                    { level.Blockchange(this, x, y, z, (byte)(Block.door3_air)); }
                    else
                    { SendBlockchange(x, y, z, b); }
                    break;
                case Block.door_air:   //Door_air
                case Block.door2_air:
                case Block.door3_air:
                    break;
                default:
                    level.Blockchange(this, x, y, z, (byte)(Block.air));
                    break;
            }
        }

        private void placeBlock(byte b, byte type, ushort x, ushort y, ushort z)
        {
            switch (BlockAction)
            {
                case 0:     //normal
                    if (level.physics == 0)
                    {
                        switch (type)
                        {
                            case Block.dirt: //instant dirt to grass
                                level.Blockchange(this, x, y, z, (byte)(Block.grass));
                                break;
                            case Block.staircasestep:    //stair handler
                                if (level.GetTile(x, (ushort)(y - 1), z) == Block.staircasestep)
                                {
                                    SendBlockchange(x, y, z, Block.air);    //send the air block back only to the user.
                                    //level.Blockchange(this, x, y, z, (byte)(Block.air));
                                    level.Blockchange(this, x, (ushort)(y - 1), z, (byte)(Block.staircasefull));
                                    break;
                                }
                                //else
                                level.Blockchange(this, x, y, z, type);
                                break;
                            default:
                                level.Blockchange(this, x, y, z, type);
                                break;
                        }

                    }
                    else
                    {
                        level.Blockchange(this, x, y, z, type);
                    }

                    if (!Block.LightPass(type))
                    {
                        if (level.GetTile(x, (ushort)(y - 1), z) == Block.grass)
                        {
                            level.Blockchange(x, (ushort)(y - 1), z, Block.dirt);
                        }
                    }

                    break;
                case 1:     //solid
                    if (b == Block.blackrock) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.blackrock));
                    break;
                case 2:     //lava
                    if (b == Block.lavastill) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.lavastill));
                    break;
                case 3:     //water
                    if (b == Block.waterstill) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.waterstill));
                    break;
                case 4:     //ACTIVE lava
                    if (b == Block.lava) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.lava));
                    BlockAction = 0;
                    break;
                case 5:     //ACTIVE water
                    if (b == Block.water) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.water));
                    BlockAction = 0;
                    break;
                case 6:     //OpGlass
                    if (b == Block.op_glass) { SendBlockchange(x, y, z, b); return; }
                    level.Blockchange(this, x, y, z, (byte)(Block.op_glass));
                    break;
                default:
                    Server.Log(name + " is breaking something");
                    BlockAction = 0;
                    break;
            }
        }
        void HandleInput(object m)
        {
            byte[] message = (byte[])m;
            if (!loggedIn) 
            return;

            byte thisid = message[0];
            ushort x = NTHO(message, 1);
            ushort y = NTHO(message, 3);
            ushort z = NTHO(message, 5);
            byte rotx = message[7];
            byte roty = message[8];
            pos = new ushort[3] { x, y, z };
            rot = new byte[2] { rotx, roty };
        }
        void HandleChat(byte[] message)
        {
            try
            {
                if (!loggedIn)
                    return;
                if (!group.canChat)
                    return;

                //byte[] message = (byte[])m;
                string text = enc.GetString(message, 1, 64).Trim();

                text = Regex.Replace(text, @"\s\s+", " ");
                foreach (char ch in text)
                {
                    if (ch < 32 || ch >= 127 || ch == '&')
                    {
                        Kick("Illegal character in chat message!");
                        return;
                    }
                } 
                if (text.Length == 0) 
                    return;
                if (text[0] == '/')
                {
                    text = text.Remove(0, 1);
                    
                    int pos = text.IndexOf(' ');
                    if (pos == -1)
                    {
                        HandleCommand(text.ToLower(), "");
                        return;
                    }
                    string cmd = text.Substring(0, pos).ToLower();
                    string msg = text.Substring(pos + 1);
                    HandleCommand(cmd, msg); 
                    return;
                } 
                if (text[0] == '@')
                {
                    string newtext = text.Substring(1).Trim();
                    int pos = newtext.IndexOf(' ');
                    if (pos != -1)
                    {
                        string to = newtext.Substring(0, pos);
                        string msg = newtext.Substring(pos + 1);
                        HandleQuery(to, msg); return;
                    }
                }
                if (text[0] == '#')
                {
                    string newtext = text.Remove(0, 1).Trim();
                    GlobalMessageOps("To Ops &f-" + color + name + "&f- " + newtext);
                    if (group.name != "operator" && group.name != "superop")
                        SendMessage("To Ops &f-" + color + name + "&f- " + newtext);
                    return;
                }
                if (text[0] == '%')
                {
                    string newtext = text.Remove(0, 1).Trim();
                    if (!Properties.worldChat)
                    {
                        GlobalChatWorld(this, newtext, true);
                    }
                    else
                    {
                        GlobalChat(this, newtext);
                    }
                    Server.Log("<" + name + "> " + newtext);
                    IRCBot.Say("<" + name + "> " + newtext);
                    return;
                }
                Server.Log("<" + name + "> " + text);

                if (Properties.worldChat)
                {
                    GlobalChat(this, text);
                }
                else
                {
                    GlobalChatLevel(this, text, true);
                }

                IRCBot.Say(name + ": " + text);
            }
            catch (Exception e) { Server.ErrorLog(e); Player.GlobalMessage("An error occurred: " + e.Message); }
        }
        void HandleCommand(string cmd, string message)
        {
            if (cmd.Equals("z")) { cmd = "cuboid"; }
            if (cmd.Equals("p")) { cmd = "paint"; }
            if (cmd.Equals("r")) { cmd = "replace"; }
            if (cmd.Equals("a")) { cmd = "abort"; }
            Command command = Command.all.Find(cmd);
            if (command != null)
            {
                if (group.CanExecute(command))
                {
                    Server.Log(name + " uses /" + cmd + " " + message);
                    command.Use(this, message);
                }
                else { SendMessage("You are not allowed to use \"" + cmd + "\"!"); }
            }
            else { SendMessage("Unknown command \"" + cmd + "\"!"); }
        }
        void HandleQuery(string to, string message)
        {
            Player p = Find(to);
            if (p == this) { SendMessage("Trying to talk to yourself, huh?"); return; }
            if (p != null)
            {
                Server.Log(name + " @" + p.name + ": " + message);
                p.SendChat(this, "&e[<] " + color + name + ": &f" + message);
                SendChat(this, "&9[>] " + p.color + p.name + ": &f" + message);
            }
            else { SendMessage("Player \"" + to + "\" doesn't exist!"); }
        }
        #endregion
        #region == OUTGOING ==
        public void SendRaw(int id) { SendRaw(id, new byte[0]); }
        public void SendRaw(int id, byte[] send)
        {
            byte[] buffer = new byte[send.Length + 1];
            buffer[0] = (byte)id;
            Buffer.BlockCopy(send, 0, buffer, 1, send.Length);
            try { socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, delegate(IAsyncResult result) { }, null); }
            catch (SocketException) { Disconnect(); }
        }
        public void SendMessage(string message) { unchecked { SendMessage((byte)-1, message); } }
        public void SendChat(Player p, string message) { SendMessage(p.id, message); }
        public void SendMessage(byte id, string message)
        {
            byte[] buffer = new byte[65];
            unchecked { buffer[0] = id; }
            foreach (string line in Wordwrap(message))
            {
                StringFormat(line, 64).CopyTo(buffer, 1);
                SendRaw(13, buffer);
            }
        }
        public void SendMotd()
        {
            byte[] buffer = new byte[130];
            buffer[0] = Properties.version;
            StringFormat(Properties.name, 64).CopyTo(buffer, 1);
            StringFormat(Properties.motd, 64).CopyTo(buffer, 65);
            if (Server.operators.Contains(this.name.ToLower()) || Server.superOps.Contains(this.name.ToLower()))
                buffer[129] = 100;
            else
                buffer[129] = 0;
            SendRaw(0, buffer);
        }
        public void SendMap()
        {
            SendRaw(2); byte[] buffer = new byte[level.blocks.Length + 4];
            BitConverter.GetBytes(IPAddress.HostToNetworkOrder(level.blocks.Length)).CopyTo(buffer, 0);
            for (int i = 0; i < level.blocks.Length; ++i)
            {
                buffer[4 + i] = Block.Convert(level.blocks[i]);
            } buffer = GZip(buffer);
            int number = (int)Math.Ceiling(((double)buffer.Length) / 1024);
            for (int i = 1; buffer.Length > 0; ++i)
            {
                short length = (short)Math.Min(buffer.Length, 1024);
                byte[] send = new byte[1027];
                HTNO(length).CopyTo(send, 0);
                Buffer.BlockCopy(buffer, 0, send, 2, length);
                byte[] tempbuffer = new byte[buffer.Length - length];
                Buffer.BlockCopy(buffer, length, tempbuffer, 0, buffer.Length - length);
                buffer = tempbuffer;
                send[1026] = (byte)(i * 100 / number);
                SendRaw(3, send);
                Thread.Sleep(10);
            } buffer = new byte[6];
            HTNO((short)level.width).CopyTo(buffer, 0);
            HTNO((short)level.depth).CopyTo(buffer, 2);
            HTNO((short)level.height).CopyTo(buffer, 4);
            SendRaw(4, buffer);
        }
        public void SendSpawn(byte id, string name, ushort x, ushort y, ushort z, byte rotx, byte roty)
        {
            pos = new ushort[3] { x, y, z }; // This could be remove and not effect the server :/
            rot = new byte[2] { rotx, roty };
            byte[] buffer = new byte[73]; buffer[0] = id;
            StringFormat(name, 64).CopyTo(buffer, 1);
            HTNO(x).CopyTo(buffer, 65);
            HTNO(y).CopyTo(buffer, 67);
            HTNO(z).CopyTo(buffer, 69);
            buffer[71] = rotx; buffer[72] = roty;
            SendRaw(7, buffer);
        }
        public void SendPos(byte id, ushort x, ushort y, ushort z, byte rotx, byte roty)
        {
            pos = new ushort[3] { x, y, z };
            rot = new byte[2] { rotx, roty };
            byte[] buffer = new byte[9]; buffer[0] = id;
            HTNO(x).CopyTo(buffer, 1);
            HTNO(y).CopyTo(buffer, 3);
            HTNO(z).CopyTo(buffer, 5);
            buffer[7] = rotx; buffer[8] = roty;
            SendRaw(8, buffer);
        }
        public void SendDie(byte id) { SendRaw(0x0C, new byte[1] { id }); }
        public void SendBlockchange(ushort x, ushort y, ushort z, byte type)
        {
            byte[] buffer = new byte[7];
            HTNO(x).CopyTo(buffer, 0);
            HTNO(y).CopyTo(buffer, 2);
            HTNO(z).CopyTo(buffer, 4);
            buffer[6] = Block.Convert(type);
            SendRaw(6, buffer);
        }
        void SendKick(string message) { SendRaw(14, StringFormat(message, 64)); }
        void SendPing() { /*pingDelay = 0; pingDelayTimer.Start();*/ SendRaw(1); }
        void UpdatePosition()
        {

            //pingDelayTimer.Stop();

            // Shameless copy from JTE's Server
            byte changed = 0;   //Denotes what has changed (x,y,z, rotation-x, rotation-y)
            // 0 = no change - never happens with this code.
            // 1 = position has changed
            // 2 = rotation has changed
            // 3 = position and rotation have changed
            // 4 = Teleport Required (maybe something to do with spawning)
            // 5 = Teleport Required + position has changed
            // 6 = Teleport Required + rotation has changed
            // 7 = Teleport Required + position and rotation has changed
            //NOTE: Players should NOT be teleporting this often. This is probably causing some problems.
            if (oldpos[0] != pos[0] || oldpos[1] != pos[1] || oldpos[2] != pos[2]) 
            {
                changed |= 1; 
            }
            if (oldrot[0] != rot[0] || oldrot[1] != rot[1]) 
            { 
                changed |= 2; 
            }
            if (Math.Abs(pos[0] - basepos[0]) > 32 || Math.Abs(pos[1] - basepos[1]) > 32 || Math.Abs(pos[2] - basepos[2]) > 32) 
            { 
                changed |= 4; 
            }
            if ((oldpos[0] == pos[0] && oldpos[1] == pos[1] && oldpos[2] == pos[2]) && (basepos[0] != pos[0] || basepos[1] != pos[1] || basepos[2] != pos[2])) 
            { 
                changed |= 4; 
            }

            byte[] buffer = new byte[0]; byte msg = 0;
            if ((changed & 4) != 0)
            {
                msg = 8; //Player teleport - used for spawning or moving too fast
                buffer = new byte[9]; buffer[0] = id;
                HTNO(pos[0]).CopyTo(buffer, 1);
                HTNO(pos[1]).CopyTo(buffer, 3);
                HTNO(pos[2]).CopyTo(buffer, 5);
                buffer[7] = rot[0]; buffer[8] = rot[1];
            }
            else if (changed == 1)
            {
                try
                {
                    msg = 10; //Position update
                    buffer = new byte[4]; buffer[0] = id;
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[0] - oldpos[0])), 0, buffer, 1, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[1] - oldpos[1])), 0, buffer, 2, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[2] - oldpos[2])), 0, buffer, 3, 1);
                }
                catch
                {

                }
            }
            else if (changed == 2)
            {
                msg = 11; //Orientation update
                buffer = new byte[3]; buffer[0] = id;
                buffer[1] = rot[0]; buffer[2] = rot[1];
            }
            else if (changed == 3)
            {
                try
                {
                    msg = 9; //Position and orientation update
                    buffer = new byte[6]; buffer[0] = id;
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[0] - oldpos[0])), 0, buffer, 1, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[1] - oldpos[1])), 0, buffer, 2, 1);
                    Buffer.BlockCopy(System.BitConverter.GetBytes((sbyte)(pos[2] - oldpos[2])), 0, buffer, 3, 1);
                    buffer[4] = rot[0]; buffer[5] = rot[1];
                }
                catch
                {

                }
            }

            if (changed != 0)
                foreach (Player p in players)
                {
                    if (p != this && p.level == level)
                    {
                        p.SendRaw(msg, buffer);
                    }
                }
            oldpos = pos; oldrot = rot;
        }
        #endregion
        #region == GLOBAL MESSAGES ==
        public static void GlobalBlockchange(Level level, ushort x, ushort y, ushort z, byte type)
        {
            players.ForEach(delegate(Player p) { if (p.level == level) { p.SendBlockchange(x, y, z, type); } });
        } 
        public static void GlobalChat(Player from, string message) { GlobalChat(from, message, true); }
        public static void GlobalChat(Player from, string message, bool showname)
        {
            if (showname) { message = from.color + from.name + ": &f" + message; }
            players.ForEach(delegate(Player p) { p.SendChat(from, message); });
        }
        public static void GlobalChatLevel(Player from, string message, bool showname)
        {
			if (showname) { message = "<Level>" + from.color + from.name + ": &f" + message; }
            players.ForEach(delegate(Player p) { if (p.level == from.level)p.SendChat(from, message); });
        }
        public static void GlobalChatWorld(Player from, string message, bool showname)
        {
            if (showname) { message = "<World>" + from.color + from.name + ": &f" + message; }
            players.ForEach(delegate(Player p) { p.SendChat(from, message); });
        }
        public static void GlobalMessage(string message)
        {
            players.ForEach(delegate(Player p) { p.SendMessage(message); });
        }
        public static void GlobalMessageLevel(Level l, string message)
        {
            players.ForEach(delegate(Player p) { if (p.level == l) p.SendMessage(message); });
        }
        public static void GlobalMessageOps(string message)     //Send a global messege to ops only
        {
            players.ForEach(delegate(Player p) 
            {
                if (p.group == Group.Find("operator") || p.group == Group.Find("superOp"))
                {
                    p.SendMessage(message);
                }
            });
        }
        public static void GlobalSpawn(Player from, ushort x, ushort y, ushort z, byte rotx, byte roty, bool self)
        {
            players.ForEach(delegate(Player p)
            {
                if (p.Loading && p != from) { return; }
                if (p.level != from.level || (from.hidden && !self)) { return; }
                if (p != from) { p.SendSpawn(from.id, from.color + from.name, x, y, z, rotx, roty); }
                else if (self)
                {
                    p.pos = new ushort[3] { x, y, z }; p.rot = new byte[2] { rotx, roty };
                    p.oldpos = p.pos; p.basepos = p.pos; p.oldrot = p.rot;
                    unchecked { p.SendSpawn((byte)-1, from.color + from.name, x, y, z, rotx, roty); }
                }
            });
        }
        public static void GlobalDie(Player from, bool self)
        {
            players.ForEach(delegate(Player p)
            {
                if (p.level != from.level || (from.hidden && !self)) { return; }
                if (p != from) { p.SendDie(from.id); }
                else if (self) { unchecked { p.SendDie((byte)-1); } }
            });
        }
        public static void GlobalUpdate() { players.ForEach(delegate(Player p) { if (!p.hidden) { p.UpdatePosition(); } }); }
        #endregion
        #region == DISCONNECTING ==
        public void Disconnect()
        {
            if (disconnected) { return; } disconnected = true;
            pingTimer.Stop(); SendKick("Disconnected."); if (loggedIn)
            {
                GlobalDie(this, false);
                if (!hidden) { GlobalChat(this, "&c- " + color + name + "&e disconnected.", false); }
                IRCBot.Say(name + " left the game.");
                Server.Log(name + " disconnected."); players.Remove(this);
                if (!Properties.console && Server.win != null)
                    Server.win.UpdateClientList(players);
                new LeftPlayer(this);
            }
            else { connections.Remove(this); Server.Log(ip + " disconnected."); }
            if (Server.afkset.Contains(name)) { Server.afkset.Remove(name); }    //Removes from afk list on disconnect
        }
        public void Kick(string message)
        {
            if (disconnected) { return; } disconnected = true;
            pingTimer.Stop(); SendKick(message); if (loggedIn)
            {
                GlobalDie(this, false);
                GlobalChat(this, "&c- " + color + name + "&e kicked (" + message + ").", false);
                Server.Log(name + " kicked (" + message + ")."); players.Remove(this);
                new LeftPlayer(this);
            }
            else { connections.Remove(this); Server.Log(ip + " kicked (" + message + ")."); }
            if (Server.afkset.Contains(name)) { Server.afkset.Remove(name); }    //Removes from afk list on disconnect
        }
        #endregion
        #region == CHECKING ==
        public static List<Player> GetPlayers() { return new List<Player>(players); }
        public static bool Exists(string name)
        {
            foreach (Player p in players)
            { if (p.name.ToLower() == name.ToLower()) { return true; } } return false;
        }
        public static bool Exists(byte id)
        {
            foreach (Player p in players)
            { if (p.id == id) { return true; } } return false;
        }
        public static Player Find(string name)
        {
            foreach (Player p in players)
            { if (p.name.ToLower() == name.ToLower()) { return p; } } return null;
        }
        public static Group GetGroup(string name)
        {
            Player who = Player.Find(name); if (who != null) { return who.group; }
            if (Server.banned.All().Contains(name.ToLower())) { return Group.Find("banned"); }
            if (Server.operators.All().Contains(name.ToLower())) { return Group.Find("operator"); }
            return Group.standard;
        } public static string GetColor(string name) { return GetGroup(name).color; }
        #endregion
        #region == OTHER ==
        static byte FreeId()
        {
            for (byte i = 0; i < Properties.players; ++i)
            {
                foreach (Player p in players)
                {
                    if (p.id == i) { goto Next; }
                } return i;
            Next: continue;
            } unchecked { return (byte)-1; }
        }
        static byte[] StringFormat(string str, int size)
        {
            byte[] bytes = new byte[size];
            bytes = enc.GetBytes(str.PadRight(size).Substring(0, size));
            return bytes;
        }
        static List<string> Wordwrap(string message)
        {
            List<string> lines = new List<string>();
            message = Regex.Replace(message, @"(&[0-9a-f])+(&[0-9a-f])", "$2");
            message = Regex.Replace(message, @"(&[0-9a-f])+$", "");
            int limit = 64; string color = "";
            while (message.Length > 0)
            {
                if (lines.Count > 0) { message = "> " + color + message.Trim(); }
                if (message.Length <= limit) { lines.Add(message); break; }
                for (int i = limit - 1; i > limit - 9; --i)
                {
                    if (message[i] == ' ') 
					{
						lines.Add(message.Substring(0, i)); goto Next; 
					}
                } 
				lines.Add(message.Substring(0, limit));
			Next: message = message.Substring(lines[lines.Count - 1].Length);
				if (lines.Count == 1)
				{
					limit = 60;
				}
                int index = lines[lines.Count - 1].LastIndexOf('&');
				if (index != -1)
				{
					if (index < lines[lines.Count - 1].Length - 1)
					{
						char next = lines[lines.Count - 1][index + 1];
						if ("0123456789abcdef".IndexOf(next) != -1) { color = "&" + next; }
						if (index == lines[lines.Count - 1].Length - 1)
						{
							lines[lines.Count - 1] = lines[lines.Count - 1].
								Substring(0, lines[lines.Count - 1].Length - 2);
						}
					}
					else if (message.Length != 0)
					{
						char next = message[0];
						if ("0123456789abcdef".IndexOf(next) != -1)
						{
							color = "&" + next;
						}
						lines[lines.Count - 1] = lines[lines.Count - 1].
							Substring(0, lines[lines.Count - 1].Length - 1);
						message = message.Substring(1);
					}
				}
            } return lines;
        }
        public static bool ValidName(string name)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890._";
            foreach (char ch in name) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }
        public static byte[] GZip(byte[] bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            GZipStream gs = new GZipStream(ms, CompressionMode.Compress, true);
            gs.Write(bytes, 0, bytes.Length); gs.Close();
            ms.Position = 0; bytes = new byte[ms.Length];
            ms.Read(bytes, 0, (int)ms.Length); ms.Close();
            return bytes;
        }
        #endregion
        #region == Host <> Network ==
        byte[] HTNO(ushort x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        ushort NTHO(byte[] x, int offset)
        {
            byte[] y = new byte[2];
            Buffer.BlockCopy(x, offset, y, 0, 2); Array.Reverse(y);
            return BitConverter.ToUInt16(y, 0);
        }
        byte[] HTNO(short x)
        {
            byte[] y = BitConverter.GetBytes(x); Array.Reverse(y); return y;
        }
        #endregion

		public static bool checkDev(Player p)
		{
            if (p.name == "Descention" || p.name == "Neko_Baron" || p.name == "KakashiSuno")
                return true;
            return false;
		}

        public static bool checkSupporter(Player p)
        {
            if (p.name == "Arib")
                return true;
            return false;
        }

        public static bool checkDevS(string p)
        {
            if (p == "Descention" || p == "Neko_Baron" || p == "KakashiSuno")
                return true;
            return false;
        }
    }
    public class LeftPlayer
    {
        public string name;
        public string ip;
        public DateTime logout;
        //public List<Edit> actions;
        public LeftPlayer(Player p)
        {
            name = p.name; ip = p.ip;
            //actions = p.actions;
            if (Player.left.Count == 64) {}    //what was this for
            Player.left.Add(name.ToLower(), this);
        }
    }
}