using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdWater : Command
    {
        public override string name { get { return "water"; } }
        public CmdWater() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            if (p.BlockAction == 3)
            {
                p.BlockAction = 0;
                p.SendMessage("Water mode: &cOFF&e.");
            }
            else
            {
                p.BlockAction = 3;
                p.SendMessage("Water Mode: &aON&e.");
            }
            p.painting = false;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/water - Turns inactive water mode on/off.");
        }
    }
}