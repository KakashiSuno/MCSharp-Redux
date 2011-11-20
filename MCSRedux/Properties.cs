using System;
using System.IO;

namespace Minecraft_Server
{
    public static class Properties
    {
        public const byte version = 7;
        public static string salt = "";

        public static string name = "Minecraft Server";
        public static string motd = "Welcome to my server!";
        public static byte players = 12;
        public static byte maps = 5;
        public static int port = 25566;
        public static bool pub = true;
        public static bool verify = false;
        public static bool worldChat = true;
        public static bool guestGoto = false;

        public static ushort backup = 10;
        public static bool silent = false;      //not used??
        public static string level = "main";
        public static string errlog = "error.log";

        public static bool console = false;
        public static bool reportBack = true;

        public static bool irc = false;
        public static int ircPort = 6667;
        public static string ircNick = "MCsharp";
        public static string ircServer = "irc.esper.net";
        public static string ircChannel = "#changethis";

        public static bool antiTunnel = true;
        public static byte maxDepth = 4;
        public static int Overload = 1500;

        //public static bool debug = false;

        public static void Load()
        {
            string rndchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            for (int i = 0; i < 16; ++i) { salt += rndchars[rnd.Next(rndchars.Length)]; }

            if (File.Exists("server.properties"))
            {
                string[] lines = File.ReadAllLines("server.properties");

                foreach (string line in lines)
                {
                    if (line != "" && line[0] != '#')
                    {
                        //int index = line.IndexOf('=') + 1; // not needed if we use Split('=')
                        string key = line.Split('=')[0].Trim();
                        string value = line.Split('=')[1].Trim();

                        switch (key.ToLower())
                        {
                            case "server-name":
                                if (ValidString(value, "![]:.,{}~-+()?_/\\ "))
                                {
                                    name = value;
                                }
                                else
                                {
                                    Server.Log("server-name invalid! setting to default.");
                                }
                                break;
                            case "motd":
                                if (ValidString(value, "![]&:.,{}~-+()?_/\\ "))
                                {
                                    motd = value;
                                }
                                else
                                {
                                    Server.Log("motd invalid! setting to default.");
                                }
                                break;
                            case "port":
                                try
                                {
                                    port = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.Log("port invalid! setting to default.");
                                }
                                break;
                            case "verify-names":
                                verify = (value.ToLower() == "true") ? true : false;
                                break;
                            case "public":
                                pub = (value.ToLower() == "true") ? true : false;
                                break;
                            case "world-chat":
                                worldChat = (value.ToLower() == "true") ? true : false;
                                break;
                            case "guest-goto":
                                guestGoto = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-players":
                                try
                                {
                                    if (Convert.ToByte(value) > 64)
                                    {
                                        value = "64";
                                        Server.Log("Max players has been lowered to 64.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                        Server.Log("Max players has been increased to 1.");
                                    }
                                    players = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.Log("max-players invalid! setting to default.");
                                }
                                break;
                            case "max-maps":
                                try
                                {
                                    if (Convert.ToByte(value) > 20)
                                    {
                                        value = "20";
                                        Server.Log("Max maps has been lowered to 20.");
                                    }
                                    else if (Convert.ToByte(value) < 1)
                                    {
                                        value = "1";
                                        Server.Log("Max maps has been increased to 1.");
                                    }
                                    maps = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.Log("max-maps invalid! setting to default.");
                                }
                                break;
                            case "irc":
                                irc = (value.ToLower() == "true") ? true : false;
                                break;
                            case "irc-server":
                                ircServer = value;
                                break;
                            case "irc-nick":
                                ircNick = value;
                                break;
                            case "irc-channel":
                                ircChannel = value;
                                break;
                            case "irc-port":
                                try
                                {
                                    ircPort = Convert.ToInt32(value);
                                }
                                catch
                                {
                                    Server.Log("irc-port invalid! setting to default.");
                                }
                                break;

                            case "anti-tunnels":
                                antiTunnel = (value.ToLower() == "true") ? true : false;
                                break;
                            case "max-depth":
                                try
                                {
                                    maxDepth = Convert.ToByte(value);
                                }
                                catch
                                {
                                    Server.Log("maxDepth invalid! setting to default.");
                                }
                                break;

                            case "overload":
                                try
                                {
                                    if (Convert.ToInt16(value) > 5000)
                                    {
                                        value = "4000";
                                        Server.Log("Max overload is 5000.");
                                    }
                                    else if (Convert.ToInt16(value) < 500)
                                    {
                                        value = "500";
                                        Server.Log("Min overload is 500");
                                    }
                                    Overload = Convert.ToInt16(value);
                                }
                                catch
                                {
                                    Server.Log("Overload invalid! setting to default.");
                                }
                                break;
                            case "report-back":
                                reportBack = (value.ToLower() == "true") ? true : false;
                                break;
                            case "console-only":
                                console = (value.ToLower() == "true") ? true : false;
                                break;

                        }
                    }
                } 
                Server.Log("LOADED: server.properties");
                Save();
            }
            else
                Generate();
        }
        public static bool ValidString(string str, string allowed)
        {
            string allowedchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890" + allowed;
            foreach (char ch in str)
            {
                if (allowedchars.IndexOf(ch) == -1)
                {
                    return false;
                }
            } return true;
        }

        static void Generate()
        {
            StreamWriter w = new StreamWriter(File.Create("server.properties"));
            w.WriteLine("# Edit the settings below to modify how your server operates. This is an explanation of what each setting does.");
            w.WriteLine("#   server-name\t=\tThe name which displays on minecraft.net");
            w.WriteLine("#   motd\t=\tThe message which displays when a player connects");
            w.WriteLine("#   port\t=\tThe port to operate from");
            w.WriteLine("#   console-only\t=\tRun without a GUI (useful for Linux servers with mono)");
            w.WriteLine("#   verify-names\t=\tVerify the validity of names");
            w.WriteLine("#   public\t=\tSet to true to appear in the public server list");
            w.WriteLine("#   max-players\t=\tThe maximum number of connections");
            w.WriteLine("#   max-maps\t=\tThe maximum number of maps loaded at once");
            w.WriteLine("#   world-chat\t=\tSet to true to enable world chat");
            w.WriteLine("#   guest-goto\t=\tSet to true to give guests goto and levels commands");
            w.WriteLine("#   irc\t=\tSet to true to enable the IRC bot");
            w.WriteLine("#   irc-nick\t=\tThe name of the IRC bot");
            w.WriteLine("#   irc-server\t=\tThe server to connect to");
            w.WriteLine("#   irc-channel\t=\tThe channel to join");
            w.WriteLine("#   irc-port\t=\tThe port to use to connect");
            w.WriteLine("#   anti-tunnels\t=\tStops people digging below max-depth");
            w.WriteLine("#   max-depth\t=\tThe maximum allowed depth to dig down");
            w.WriteLine("#   overload\t=\tThe higher this is, the longer the physics is allowed to lag. Default 1500");
            w.WriteLine("#   report-back\t=\tAutomatically report crash information back to MCSharp developers (not yet in use)");
            w.WriteLine();
            w.WriteLine();
            w.WriteLine("# Server options");
            w.WriteLine("server-name = Minecraft Server");
            w.WriteLine("motd = Welcome to my server!");
            w.WriteLine("port = 25565");
            w.WriteLine("console-only = false");
            w.WriteLine("verify-names = true");
            w.WriteLine("public = true");
            w.WriteLine("max-players = 12");
            w.WriteLine("max-maps = 5");
            w.WriteLine("world-chat = true");
            w.WriteLine("guest-goto = false");
            w.WriteLine();
            w.WriteLine("# irc bot options");
            w.WriteLine("irc = false");
            w.WriteLine("irc-nick = MCsharp");
            w.WriteLine("irc-server = irc.esper.net");
            w.WriteLine("irc-channel = #changethis");
            w.WriteLine("irc-port = 6667");
            w.WriteLine();
            w.WriteLine("# other options");
            w.WriteLine("anti-tunnels = true");
            w.WriteLine("max-depth = 4");
            w.WriteLine("overload = 1500");
            w.WriteLine();
            w.WriteLine("#Error logging");
            w.WriteLine("report-back = true");

            w.Flush();
            w.Close();
        }

        static void Save()
        {
            try
            {
                StreamWriter w = new StreamWriter(File.Create("server.properties"));
                w.WriteLine("# Edit the settings below to modify how your server operates. This is an explanation of what each setting does.");
                w.WriteLine("#   server-name\t=\tThe name which displays on minecraft.net");
                w.WriteLine("#   motd\t=\tThe message which displays when a player connects");
                w.WriteLine("#   port\t=\tThe port to operate from");
                w.WriteLine("#   console-only\t=\tRun without a GUI (useful for Linux servers with mono)");
                w.WriteLine("#   verify-names\t=\tVerify the validity of names");
                w.WriteLine("#   public\t=\tSet to true to appear in the public server list");
                w.WriteLine("#   max-players\t=\tThe maximum number of connections");
                w.WriteLine("#   max-maps\t=\tThe maximum number of maps loaded at once");
                w.WriteLine("#   world-chat\t=\tSet to true to enable world chat");
                w.WriteLine("#   guest-goto\t=\tSet to true to give guests goto and levels commands");
                w.WriteLine("#   irc\t=\tSet to true to enable the IRC bot");
                w.WriteLine("#   irc-nick\t=\tThe name of the IRC bot");
                w.WriteLine("#   irc-server\t=\tThe server to connect to");
                w.WriteLine("#   irc-channel\t=\tThe channel to join");
                w.WriteLine("#   irc-port\t=\tThe port to use to connect");
                w.WriteLine("#   anti-tunnels\t=\tStops people digging below max-depth");
                w.WriteLine("#   max-depth\t=\tThe maximum allowed depth to dig down");
                w.WriteLine("#   overload\t=\tThe higher this is, the longer the physics is allowed to lag. Default 1500");
                w.WriteLine("#   report-back\t=\tAutomatically report crash information back to MCSharp developers (not yet in use)");
                w.WriteLine();
                w.WriteLine();
                w.WriteLine("# Server options");
                w.WriteLine("server-name = " + name);
                w.WriteLine("motd = " + motd);
                w.WriteLine("port = " + port.ToString());
                w.WriteLine("console-only = " + console.ToString().ToLower());
                w.WriteLine("verify-names = " + verify.ToString().ToLower());
                w.WriteLine("public = " + pub.ToString().ToLower());
                w.WriteLine("max-players = " + players.ToString());
                w.WriteLine("max-maps = " + maps.ToString());
                w.WriteLine("world-chat = " + worldChat.ToString().ToLower());
                w.WriteLine("guest-goto = " + guestGoto.ToString().ToLower());
                w.WriteLine();
                w.WriteLine("# irc bot options");
                w.WriteLine("irc = " + irc.ToString().ToLower());
                w.WriteLine("irc-nick = " + ircNick);
                w.WriteLine("irc-server = " + ircServer);
                w.WriteLine("irc-channel = " + ircChannel);
                w.WriteLine("irc-port = " + ircPort.ToString());
                w.WriteLine();
                w.WriteLine("# other options");
                w.WriteLine("anti-tunnels = " + antiTunnel.ToString().ToLower());
                w.WriteLine("max-depth = " + maxDepth.ToString());
                w.WriteLine("overload = " + Overload.ToString());
                w.WriteLine();
                w.WriteLine("#Error logging");
                w.WriteLine("report-back = " + reportBack.ToString().ToLower());
                w.Flush();
                w.Close();

                Server.Log("SAVED: server.properties");
            }
            catch
            {
                Server.Log("SAVE FAILED! server.properties");
            }
        }
    }
}
