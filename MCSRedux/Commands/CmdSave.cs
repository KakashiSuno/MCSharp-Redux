using System;

namespace Minecraft_Server
{
    public class CmdSave : Command
    {
        public override string name { get { return "save"; } }
        public CmdSave() { }
        public override void Use(Player p, string message)
        {
            if (p != null)
            {
                if (message != "") { Help(p); return; }
                p.level.Save();
                p.SendMessage("Level \"" + p.level.name + "\" saved.");
            }
            else
            {
                foreach (Level l in Server.levels)
                {
                    l.Save();
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/save - Saves the level, not an actual backup.");
        }
    }
}