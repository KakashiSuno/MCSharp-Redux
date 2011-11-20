using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdLava : Command
    {
        public override string name { get { return "lava"; } }
        public CmdLava() { }
        public override void Use(Player p, string message)
        {
            if (p.BlockAction == 2)
            {
                p.BlockAction = 0;
                p.SendMessage("Lava mode: &cOFF&e.");
            }
            else
            {
                p.BlockAction = 2;
                p.SendMessage("Lava Mode: &aON&e.");
            }
            p.painting = false;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/lava - Turns inactive lava mode on/off.");
        }
    }
}