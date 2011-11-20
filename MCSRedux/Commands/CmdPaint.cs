using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdPaint : Command
    {
        public override string name { get { return "paint"; } }
        public CmdPaint() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            p.painting = !p.painting; if (p.painting) { p.SendMessage("Painting mode: &aON&e."); }
            else { p.SendMessage("Painting mode: &cOFF&e."); }
            p.BlockAction = 0;
        }
        public override void Help(Player p)
        {
            p.SendMessage("/paint - Turns painting mode on/off.");
        }
    }
}