using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdSolid : Command
    {
        public override string name { get { return "solid"; } }
        public CmdSolid() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            if (p.BlockAction == 1)
            {
                p.BlockAction = 0;
                p.SendMessage("Solid mode: &cOFF&e.");
            }
            else
            {
                p.BlockAction = 1;
                p.SendMessage("Solid Mode: &aON&e.");
            }
            p.painting = false;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/solid - Turns solid mode on/off.");
        }
    }
}