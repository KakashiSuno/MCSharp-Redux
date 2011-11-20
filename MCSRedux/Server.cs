using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;

using System.Collections.Specialized;

namespace Minecraft_Server
{
    public class Server
    {

        static bool shouldQuit = false;

        public static string Version { get { return "1.0.0"; } } // Version matches that of Minecraft
		
        static Socket listen;
        static System.Diagnostics.Process process;
        static System.Timers.Timer updateTimer = new System.Timers.Timer(100);
        //static System.Timers.Timer heartbeatTimer = new System.Timers.Timer(60000);     //Every 45 seconds
        static System.Timers.Timer messageTimer = new System.Timers.Timer(60000 * 5);   //Every 5 mins
        static Thread inputThread;
        static Thread physThread;
        // static Thread botsThread;

        public static PlayerList operators;
        public static PlayerList superOps;
        public static PlayerList banned;
        public static PlayerList bannedIP;
        public static PlayerList builders;
        public static PlayerList advbuilders;
        public static PlayerList bot;
        public static MapGenerator MapGen;

        public static PlayerList ircControllers;

        public static Level mainLevel;
        public static List<Level> levels;
        public static List<string> afkset = new List<string>();
        public static List<string> messages = new List<string>();
		
        public Server(Window w)
        {
            levels = new List<Level>(Properties.maps);
            Random random = new  Random();
            win = (w != null) ? w : null;
            Console.Title = "MCsharp";

            MapGen = new MapGenerator();

            if (File.Exists("levels/" + Properties.level + ".lvl"))
            {
                mainLevel = Level.Load(Properties.level);
                if (mainLevel == null)
                {
                    if (File.Exists("levels/" + Properties.level + ".lvl.backup"))
                    {
                        Server.Log("Atempting to load backup.");
                        File.Copy("levels/" + Properties.level + ".lvl.backup", "levels/" + Properties.level + ".lvl", true);
                        mainLevel = Level.Load(Properties.level);
                        if (mainLevel == null)
                        {
                            Server.Log("BACKUP FAILED!");
                            Console.ReadKey(); return;
                        }
                    }
                    else
                    {
                        Server.Log("BACKUP NOT FOUND!");
                        Console.ReadKey(); return;
                    }

                }
            }
            else
            {
                Log("mainlevel not found");
                mainLevel = new Level(Properties.level, 128, 64, 128, "flat");

                mainLevel.permissionvisit = LevelPermission.Guest;
                mainLevel.permissionbuild = LevelPermission.Guest;
                mainLevel.Save();
            }
            levels.Add(mainLevel);

            // TODO: Administrator group.
            if (File.Exists("ranks/admins.txt"))
            {
                File.Copy("ranks/admins.txt", "ranks/operators.txt", true);
                File.Delete("ranks/admins.txt");
            }
            
            banned = PlayerList.Load("banned.txt");
            bannedIP = PlayerList.Load("banned-ip.txt");
            builders = PlayerList.Load("builders.txt");
            advbuilders = PlayerList.Load("advbuilders.txt");
			operators = PlayerList.Load("operators.txt");
			superOps = PlayerList.Load("uberOps.txt");
            bot = PlayerList.Load("bots.txt");
            ircControllers = PlayerList.Load("../IRC_Controllers.txt");

            if (!bot.Contains("flist"))
            {
                bot.Add("flist");
                bot.Save("bots.txt", false);
            }
            Command.InitAll(); 
            Group.InitAll();

            if (File.Exists("autoload.txt"))
            {
                try
                {
                    string[] lines = File.ReadAllLines("autoload.txt");
                    foreach (string line in lines)
                    {
                        //int temp = 0;
                        string _line = line.Trim();
                        try
                        {

                            if (_line == "") { continue; }
                            if (_line[0] == '#') { continue; }
							int index = _line.IndexOf("=");

							string key = line.Split('=')[0].Trim();
							string value;
							try
							{
								value = line.Split('=')[1].Trim();
							}
							catch
							{
								value = "0";
							}

                            if (!key.Equals("main"))
                            {
                                Command.all.Find("load").Use(null, key + " " + value);
                            }
                            else
                            {
                                try
                                {
                                    int temp = int.Parse(value);
                                    if (temp >= 0 && temp <= 2)
                                    {
                                        mainLevel.physics = temp;
                                    }
                                }
                                catch
                                {
                                    Server.Log("Physics variable invalid");
                                }
                            }
                            

                        }
                        catch
                        {
                            Server.Log(_line + " failed.");
                        }



                    }
                }
                catch
                {
                    Server.Log("autoload.txt error");
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            else
            {
                Server.Log("autoload.txt does not exist");
            }

            Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "Creating listening socket... ");
            if (Setup())
            {
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("Error!");
                Console.ReadKey();
                return;
            }

			updateTimer.Elapsed += delegate { 
                Player.GlobalUpdate(); 
                PlayerBot.GlobalUpdatePosition(); 
            }; 
            
            updateTimer.Start();
			
            // Heartbeat code here:

			Heartbeat.Init();
			
			// END Heartbeat code
            

            physThread = new Thread(new ThreadStart(Physics));
            physThread.Start();

            //botsThread = new Thread(new ThreadStart(PlayerBot.GlobalUpdate));
            //botsThread.Start();

            messageTimer.Elapsed += delegate { RandomMessage(); }; messageTimer.Start();
            process = System.Diagnostics.Process.GetCurrentProcess();
            
            if (File.Exists("messages.txt"))
            {
                StreamReader r = File.OpenText("messages.txt");
                while (!r.EndOfStream)
                    messages.Add(r.ReadLine());
            }
            else
                File.Create("messages.txt").Close();

            if (Properties.irc)
                new IRCBot();

            new AutoSaver(150);     //2 and a half mins
            //Thread physThread = new Thread(new ThreadStart(Physics));
            //physThread.Start();
            if (Properties.console)
            {
                inputThread = new Thread(new ThreadStart(ParseInput));
                inputThread.Start();
                Console.Title = Properties.name;
            }

            while (!shouldQuit)
            {
                Thread.Sleep(100);
            }
        }

        static bool Setup()
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, Properties.port);
                listen = new Socket(endpoint.Address.AddressFamily,
                                    SocketType.Stream, ProtocolType.Tcp);
                listen.Bind(endpoint);
                listen.Listen((int)SocketOptionName.MaxConnections);
                
                listen.BeginAccept(new AsyncCallback(Accept), null);
				return true;
            }
            catch (SocketException e) { ErrorLog(e); return false; }
            catch (Exception e) { ErrorLog(e); return false; }
        }

        static void Accept(IAsyncResult result)
        {
			// found information: http://www.codeguru.com/csharp/csharp/cs_network/sockets/article.php/c7695
			// -Descention
			try
			{
				new Player(listen.EndAccept(result));
				Log("New Connection");
				listen.BeginAccept(new AsyncCallback(Accept), null);
			}
			catch (SocketException e)
			{
				//s.Close();
				ErrorLog(e);
			}
			catch (Exception e)
			{
				//s.Close(); 
				ErrorLog(e);
			}
        }

        public static void Exit()
        {
            Player.players.ForEach(delegate(Player p) { p.Kick("Server shutdown."); });
            Player.connections.ForEach(delegate(Player p) { p.Kick("Server shutdown."); });
            //heartbeatThread.Abort();
			if(physThread!=null)
				physThread.Abort();
			if(process!=null)
				process.Dispose();
            shouldQuit = true;
        }

        public static void Log(string message)
        {
            if (!Properties.console && win != null)
                win.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + message);
            else
                Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + message);
            
            Logger.Write(DateTime.Now.ToString("(HH:mm:ss) ") + message + Environment.NewLine);
            
            //StreamWriter sw = File.AppendText("server.log");
            //sw.WriteLine(DateTime.Now.ToString("(HH:mm:ss)") + message);
            //sw.Close();
        }

        public static void LogConsole(string message)   //to console only, and no logging it.
        {
            Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + message);
        }
        public static void ErrorLog(string message)
        {
            if (Properties.errlog == "") { Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "ERROR!"); }
            else
            {
                Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "ERROR! See \"" + Properties.errlog + "\" for more information.");
                StreamWriter sw = File.AppendText(Properties.errlog);
                sw.WriteLine(DateTime.Now.ToString("(HH:mm:ss)"));
                sw.WriteLine(message); sw.Close();
            }
        }

        public static void ErrorLog(Exception ex)
        {
            if (Properties.errlog.Length == 0)  
                Console.WriteLine(DateTime.Now.ToString("(HH:mm:ss) ") + "ERROR!"); 
            else
            {
                Logger.WriteError(ex);
            }
        }

        static void ParseInput()        //Handle console commands
        {
            string cmd;
            string msg;
            while (true)
            {
                string input = Console.ReadLine();
				if(input == null)
					continue;
                cmd = input.Split(' ')[0];
                if (input.Split(' ').Length > 1)
                    msg = input.Substring(input.IndexOf(' ')).Trim();
                else
                    msg = "";

                try
                {
                    switch (cmd)
                    {
                        case "kick":
                            Command.all.Find("kick").Use(null, msg); break;
                        case "ban":
                            Command.all.Find("ban").Use(null, msg); break;
                        case "banip":
                            Command.all.Find("banip").Use(null, msg); break;
                        case "resetbot":
                            Command.all.Find("resetbot").Use(null, msg); break;
                        case "save":
                            Command.all.Find("save").Use(null, msg); break;
                        case "say":
                            if (!msg.Equals(""))
                            {
                                if (Properties.ValidString(msg, "![]&:.,{}~-+()?_/\\@%$ "))
                                {
                                    Player.GlobalMessage(msg);
                                }
                                else
                                {
                                    Console.WriteLine("bad char in say");
                                }
                            }
                            break;
                        case "guest":
                            Command.all.Find("guest").Use(null, msg); break;
                        case "builder":
                            Command.all.Find("builder").Use(null, msg); break;
						case "help":
                            Console.WriteLine("ban, banip, builder, guest, kick, resetbot, save, say");
							break;
                        default:
                            Console.WriteLine("No such command!"); break;
                    }
                }
                catch (Exception e) { ErrorLog(e); }
				//Thread.Sleep(10);
            }
        }
        public static void Physics()
        {
            int wait = 250;
            while (true)
            {
                try
                {
                    if (wait > 0)
                    {
                        Thread.Sleep(wait);
                    }
                    DateTime Start = DateTime.Now;
                    levels.ForEach(delegate(Level L)    //update every level
                    {
                        L.CalcPhysics();
                    });
                    TimeSpan Took = DateTime.Now - Start;
                    wait = (int)250 - (int)Took.TotalMilliseconds;
                    if (wait < -Properties.Overload)
                    {
                        levels.ForEach(delegate(Level L)    //update every level
                        {
                            try
                            {
                                L.physics = 0;
                                L.ClearPhysics();
                            }
                            catch
                            {

                            }
                        });
                        Log("!PHYSICS SHUTDOWN!");
                        Player.GlobalMessage("!PHYSICS SHUTDOWN!");
                        wait = 250;
                    }
                    else if (wait < (int)(-Properties.Overload*0.75f))
                    {
                        Log("!PHYSICS WARNING!");
                    }
                }
                catch
                {
                    Log("GAH! PHYSICS EXPLODING!");
                    wait = 250;
                }
                
            }
		}

		#region Hearbeats

        void Day7Heartbeat()
        {
            string name = "";
            for (int i = 0; i < Properties.name.Length; ++i)
            {
                name += "%" + BitConverter.ToString(new byte[1] { (byte)Properties.name[i] });
            }
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://day7tech.com/dataghost/post_server.php");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2";
            request.Method = "POST";
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(
                "name=" + name +
                "&url=" + _serverURL +
                "&server=" + Properties.ircServer +
                "&channel=" + Properties.ircChannel);
            sw.Close();
            Server.LogConsole("Sent info to day7tech.com");
            

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            sr.Close(); response.Close();
        }

		#endregion

		public static char[] reservedChars = { ' ', '!', '*', '\'', '(', ')', ';', ':', '@', '&',
                                                 '=', '+', '$', ',', '/', '?', '%', '#', '[', ']' };

		public static string UrlEncode(string input)
		{
			StringBuilder output = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if ((input[i] >= '0' && input[i] <= '9') ||
					(input[i] >= 'a' && input[i] <= 'z') ||
					(input[i] >= 'A' && input[i] <= 'Z') ||
					input[i] == '-' || input[i] == '_' || input[i] == '.' || input[i] == '~')
				{
					output.Append(input[i]);
				}
				else if (Array.IndexOf<char>(reservedChars, input[i]) != -1)
				{
					output.Append('%').Append(((int)input[i]).ToString("X"));
				}
			}
			return output.ToString();
		}

		public static void RandomMessage()
        {
            if (Player.number != 0 && messages.Count > 0)
                Player.GlobalMessage(messages[new Random().Next(0, messages.Count)]);
        }
    }
}