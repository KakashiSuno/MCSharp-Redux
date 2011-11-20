using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdActiveWater : Command
    {
        public override string name { get { return "activewater"; } }
        public CmdActiveWater() { }
        public override void Use(Player p, string message)
        {
            if (p.BlockAction == 5)
            {
                p.BlockAction = 0;
                p.SendMessage("Active water aborted.");
            }
            else
            {
                p.BlockAction = 5;
                p.SendMessage("Now placing &cactive_water&e!");
            }
            p.painting = false;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/activewater - Place a single block of active water.");
        }
    }
}