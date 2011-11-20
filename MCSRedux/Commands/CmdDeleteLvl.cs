using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Minecraft_Server
{
    class CmdDeleteLvl : Command
    {
        public override string name { get { return "deletelvl"; } }
        public CmdDeleteLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            foreach (Level l in Server.levels)
            {
                if (l.name.ToLower() == message)
                {
                    Command.all.Find("unload").Use(p, message);
                    File.Delete("levels/" + l.name + ".lvl");
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/deletelvl - perminatly deletes a level.");
        }
    }
}
