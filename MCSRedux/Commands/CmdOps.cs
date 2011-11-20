using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdOps : Command
    {
        public override string name { get { return "ops"; } }
        public CmdOps() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            if (Server.operators.All().Count > 0)
            {
                Server.operators.All().ForEach(delegate(string name) { message += ", " + name; });
                p.SendMessage(Server.operators.All().Count + " &3operator" + ((Server.operators.All().Count != 1) ? "s" : "") + "&e: " + message.Remove(0, 2) + ".");
            }
            else { p.SendMessage("Nobody is operator."); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/ops - Lists all operators.");
        }
    }
}