using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Minecraft_Server
{
    public class CmdLoad : Command
    {
        public override string name { get { return "load"; } }
        public CmdLoad() { }
        public override void Use(Player p, string message)
        {
            try
            {
                new Thread(new ThreadStart(delegate
                {
                    if (p != null)
                    {
                        if (message == "") { Help(p); return; }
                        if (message.Split(' ').Length > 2) { Help(p); return; }
                        int pos = message.IndexOf(' ');
                        string phys = "0";
                        if (pos != -1)
                        {
                            phys = message.Substring(pos+1);
                            message = message.Substring(0, pos).ToLower();
                        }
                        else
                        {
                            message = message.ToLower();
                        }
                        
                        foreach (Level l in Server.levels)
                        {
                            if (l.name == message) { p.SendMessage(message + " is already loaded!"); return; }
                        }
                        if (Server.levels.Count == Server.levels.Capacity)
                        {
                            if (Server.levels.Capacity == 1)
                            { p.SendMessage("You can't load any levels!"); }
                            else
                            { p.SendMessage("You can't load more than " + Server.levels.Capacity + " levels!"); } return;
                        }
                        if (!File.Exists("levels/" + message + ".lvl"))
                        { p.SendMessage("Level \"" + message + "\" doesn't exist!"); return; }
                        Level level = Level.Load(message);

                        if (level == null)
                        {
                            if (File.Exists("levels/" + message + ".lvl.backup"))
                            {
                                Server.Log("Atempting to load backup.");
                                File.Copy("levels/" + message + ".lvl.backup", "levels/" + message + ".lvl", true);
                                level = Level.Load(message);
                                if (level == null)
                                {
                                    if(!p.disconnected) p.SendMessage("Backup of " + message + " failed.");
                                    return;
                                }
                            }
                            else
                            {
                                if(!p.disconnected) p.SendMessage("Backup of " + message + " does not exist.");
                                return;
                            }

                        }
						lock (Server.levels)
						{
							Server.levels.Add(level);
						}
                        Player.GlobalMessage("Level \"" + level.name + "\" loaded.");
                        try
                        {
                            int temp = int.Parse(phys);
                            if (temp >= 0 && temp <= 2)
                            {
                                level.physics = temp;
                            }
                        }
                        catch
                        {
                            if (!p.disconnected) p.SendMessage("Physics variable invalid");
                        }

                    }
                    else    //None player command
                    {
                        //message = message.ToLower();
                        if (message.Split(' ').Length > 2) { return; }
                        int pos = message.IndexOf(' ');
                        string phys = "0";
                        if (pos != -1)
                        {
                            phys = message.Substring(pos + 1);
                            message = message.Substring(0, pos).ToLower();
                        }
                        else
                        {
                            message = message.ToLower();
                        }
                        foreach (Level l in Server.levels)
                        {
                            if (l.name == message) { Server.Log(message + " is already loaded!"); return; }
                        }
                        if (Server.levels.Count == Server.levels.Capacity)
                        {
                            if (Server.levels.Capacity == 1)
                            { Server.Log("You can't load any levels!"); }
                            else
                            { Server.Log("You can't load more than " + Server.levels.Capacity + " levels!"); } return;
                        }
                        if (!File.Exists("levels/" + message + ".lvl"))
                        { Server.Log("Level \"" + message + "\" doesn't exist!"); return; }
                        Level level = Level.Load(message);

                        if (level == null)
                        {
                            if (File.Exists("levels/" + message + ".lvl.backup"))
                            {
                                Server.Log("Atempting to load backup.");
                                File.Copy("levels/" + message + ".lvl.backup", "levels/" + message + ".lvl", true);
                                level = Level.Load(message);
                                if (level == null)
                                {
                                    Server.Log("Backup of " + message + " failed.");
                                    return;
                                }
                            }
                            else
                            {
                                Server.Log("Backup of " + message + " does not exist.");
                                return;
                            }

                        }
                        Server.levels.Add(level);
                        Server.Log("Level \"" + level.name + "\" loaded.");
                        try
                        {
                            int temp = int.Parse(phys);
                            if (temp >= 0 && temp <= 2)
                            {
                                level.physics = temp;
                            }
                        }
                        catch
                        {
                            Server.Log("Physics variable invalid");
                        }

                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                })).Start();
            }
            catch (Exception e) { Player.GlobalMessage("An error occured with /load"); Server.ErrorLog(e); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/load <level> - Loads a level.");
        }
    }
}