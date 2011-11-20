using System;
using System.Collections.Generic;
using System.Text;
using Meebey.SmartIrc4net;
using System.Threading;

namespace Minecraft_Server
{
    class IRCBot
    {
        static IrcClient irc = new IrcClient();
        static string server = Properties.ircServer;
        static string channel = Properties.ircChannel;
        static string nick = Properties.ircNick;
        static Thread ircThread;

        static string[] names;

        public IRCBot()
        {
            // the irc must run in a seperate thread, or else the server will freeze.
            ircThread = new Thread(new ThreadStart(delegate
            {
                // attach event handlers
                irc.OnConnecting += new EventHandler(OnConnecting);
                irc.OnConnected += new EventHandler(OnConnected);
                irc.OnChannelMessage += new IrcEventHandler(OnChanMessage);
                irc.OnJoin += new JoinEventHandler(OnJoin);
                irc.OnPart += new PartEventHandler(OnPart);
                irc.OnQuit += new QuitEventHandler(OnQuit);
                irc.OnNickChange += new NickChangeEventHandler(OnNickChange);
                //irc.OnDisconnected += new EventHandler(OnDisconnected);
                irc.OnQueryMessage += new IrcEventHandler(OnPrivMsg);
                irc.OnNames += new NamesEventHandler(OnNames);
                irc.OnChannelAction += new ActionEventHandler(OnAction);

                // Attempt to connect to the IRC server
                try { irc.Connect(server, Properties.ircPort); }
                catch (Exception ex) { Console.WriteLine("Unnable to connect to IRC server: {0}", ex.Message); }
            }));
            ircThread.Start();
        }

        // While connecting
        void OnConnecting(object sender, EventArgs e)
        {
            Server.Log("Connecting to IRC");
        }
        // When connected
        void OnConnected(object sender, EventArgs e)
        {
            Server.Log("Connected to IRC");
            irc.Login(nick, nick, 0, nick);
            Server.Log("Joining channel");
            irc.RfcJoin(channel);
            irc.Listen();
        }
        void OnNames(object sender, NamesEventArgs e)
        {
            names = e.UserList;
        }
        //void OnDisconnected(object sender, EventArgs e)
        //{
        //    try { irc.Connect(server, 6667); }
        //    catch (Exception ex) { Console.WriteLine("Failed to reconnect to IRC"); }
        //}
        // On public channel message
        void OnChanMessage(object sender, IrcEventArgs e)
        {

            string temp = e.Data.Message;
            
            string allowedchars = "1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./!@#$%^*()_+QWERTYUIOPASDFGHJKL:\"ZXCVBNM<>? ";

            foreach (char ch in temp)
            {
                if (allowedchars.IndexOf(ch) == -1)
                    temp = temp.Replace(ch.ToString(), "");
            }

            Server.Log("IRC: " + e.Data.Nick + ": " + temp);
            Player.GlobalMessage("IRC: &1" + e.Data.Nick + ": &f" + temp);

            //Server.Log("IRC: " + e.Data.Nick + ": " + e.Data.Message);
            //Player.GlobalMessage("IRC: &1" + e.Data.Nick + ": &f" + e.Data.Message);
        }
        // When someone joins the IRC
        void OnJoin(object sender, JoinEventArgs e)
        {
            Server.Log(e.Data.Nick + " has joined the IRC");
            Player.GlobalMessage(e.Data.Nick + " has joined the IRC");
            irc.RfcNames(channel);
        }
        // When someone leaves the IRC
        void OnPart(object sender, PartEventArgs e)
        {
            Server.Log(e.Data.Nick + " has left the IRC");
            Player.GlobalMessage(e.Data.Nick + " has left the IRC");
            irc.RfcNames(channel);
        }
        void OnQuit(object sender, QuitEventArgs e)
        {
            Server.Log(e.Data.Nick + " has Left the IRC");
            Player.GlobalMessage(e.Data.Nick + " has left the IRC");
            irc.RfcNames(channel);
        }
        void OnPrivMsg(object sender, IrcEventArgs e)
        {
            Server.Log("IRC RECIEVING MESSESGE");
            if (Server.ircControllers.Contains(e.Data.Nick))
            {
                string cmd;
                string msg;
                int len = e.Data.Message.Split(' ').Length;
                cmd = e.Data.Message.Split(' ')[0];
                if (len > 1)
                    msg = e.Data.Message.Substring(e.Data.Message.IndexOf(' ')).Trim();
                else
                    msg = "";

                //Console.WriteLine(cmd + " : " + msg);
                Server.Log(cmd + " : " + msg);
                switch (cmd)
                {
                    case "kick":
                        Command.all.Find("kick").Use(null, msg); break;
                    case "ban":
                        Command.all.Find("ban").Use(null, msg); break;
                    case "banip":
                        Command.all.Find("banip").Use(null, msg); break;
                    case "guest":
                        Command.all.Find("guest").Use(null, msg); break;
                    case "builder":
                        Command.all.Find("builder").Use(null, msg); break;
                    case "say":
                        irc.SendMessage(SendType.Message, channel, msg); break;
                    default:
                        irc.SendMessage(SendType.CtcpReply, e.Data.Nick, "Fail No Such Command"); break;
                }
            }
        }
        void OnNickChange(object sender, NickChangeEventArgs e)
        {
            string key;
            if (e.NewNickname.Split('|').Length == 2)
            {
                key = e.NewNickname.Split('|')[1];
                if (key != null && key != "")
                {
                    switch (key)
                    {
                        case "AFK":
                            Player.GlobalMessage("IRC: " + e.OldNickname + " is AFK"); Server.afkset.Add(e.OldNickname); break;
                        case "Away":
                            Player.GlobalMessage("IRC: " + e.OldNickname + " is Away"); Server.afkset.Add(e.OldNickname); break;
                    }
                }
            }
            else if (Server.afkset.Contains(e.NewNickname))
            {
                Player.GlobalMessage("IRC: " + e.NewNickname + " is no longer away");
                Server.afkset.Remove(e.NewNickname);
            }
            else
                Player.GlobalMessage("IRC: " + e.OldNickname + " is now known as " + e.NewNickname);

            irc.RfcNames(channel);
        }
        void OnAction(object sender, ActionEventArgs e)
        {
            Player.GlobalMessage("* " + e.Data.Nick + " " + e.ActionMessage);
        }
        
        
        /// <summary>
        /// A simple say method for use outside the bot class
        /// </summary>
        /// <param name="msg">what to send</param>
        public static void Say(string msg)
        {
            if (irc != null && irc.IsConnected && Properties.irc)
                irc.SendMessage(SendType.Message, channel, msg);
        }
        public static bool IsConnected()
        {
            if (irc.IsConnected)
                return true;
            else
                return false;
        }
        public static void Reset()
        {
            if (irc.IsConnected)
                irc.Disconnect();
            ircThread = new Thread(new ThreadStart(delegate
            {
                try { irc.Connect(server, Properties.ircPort); }
                catch (Exception e)
                {
                    Server.Log("Error Connecting to IRC");
                    Server.Log(e.ToString());
                }
            }));
            ircThread.Start();
        }
        public static string[] GetConnectedUsers()
        {
            return names;
        }
    }
}
