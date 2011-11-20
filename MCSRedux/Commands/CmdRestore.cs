using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Minecraft_Server
{
    class CmdRestore : Command
    {
        public override string name { get { return "restore"; } }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Server.Log("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl");
                if (File.Exists("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl"))
                {
                    try
                    {
                        File.Copy("levels/backups/" + p.level.name + "/" + message + "/" + p.level.name + ".lvl",
                              "levels/" + p.level.name + ".lvl", true);
                        Level temp = Level.Load(p.level.name);
                        if (temp != null)
                        {
                            p.level.spawnx = temp.spawnx;
                            p.level.spawny = temp.spawny;
                            p.level.spawnz = temp.spawnz;

                            p.level.height = temp.height;
                            p.level.width = temp.width;
                            p.level.depth = temp.depth;

                            p.level.blocks = temp.blocks;
                            p.level.physics = 0;
                            p.level.ClearPhysics();
                            /*Player.players.ForEach(delegate(Player pl) { if (pl.level == p.level) { pl.SendMotd(); } });
                            ushort x = (ushort)((0.5 + p.level.spawnx) * 32);
                            ushort y = (ushort)((1 + p.level.spawny) * 32);
                            ushort z = (ushort)((0.5 + p.level.spawnz) * 32);*/
                            Player.players.ForEach(delegate(Player pl)
                            {
                                if (pl != p)
                                {
                                    if (pl.level == p.level)
                                    {
                                        pl.Kick("Restore in progress, please rejoin.");
                                    }
                                }
                            });

                            p.SendMotd(); p.SendMap();
                            ushort x = (ushort)((0.5+p.level.spawnx)*32);
                            ushort y = (ushort)((1 + p.level.spawny) * 32);
                            ushort z = (ushort)((0.5 + p.level.spawnz) * 32);
                            unchecked{
                                p.SendPos((byte)-1, x, y, z,
                                    p.level.rotx,
                                    p.level.roty);}
                        }
                        else
                        {
                            Server.Log("Restore nulled");
                            File.Copy("levels/" + p.level.name + ".lvl.backup","levels/" + p.level.name + ".lvl", true);
                        }

                    }
                    catch
                    {
                        Server.Log("Restore fail");
                    }
                }
                else
                {
                    p.SendMessage("Backup " + message + " does not exist.");
                }
            }
            else
            {
                if (Directory.Exists("levels/backups/" + p.level.name))
                {
                    int backupNumber = Directory.GetDirectories("levels/backups/" + p.level.name).Length;
                    p.SendMessage(p.level.name + " has " + backupNumber.ToString() + " backups .");
                }
                else
                {
                    p.SendMessage(p.level.name + " has no backups yet.");
                }
            }

        }

        public override void Help(Player p)
        {
            p.SendMessage("/restore <number> - restores a previous backup of the current map");
        }
    }
}
