using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdOpGlass : Command
    {
        public override string name { get { return "opglass"; } }
        public CmdOpGlass() { }
        public override void Use(Player p, string message)
        {
            if (p.BlockAction == 6)
            {
                p.BlockAction = 0;
                p.SendMessage("Op Glass: &cOFF&e.");
            }
            else
            {
                p.BlockAction = 6;
                p.SendMessage("Op Glass: &aON&e.");
            }
            p.painting = false;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/opglass -  Turns Op Glass mode on/off.");
        }
    }
}