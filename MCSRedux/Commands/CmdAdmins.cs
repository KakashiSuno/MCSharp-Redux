using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    public class CmdAdmins : Command
    {
        public override string name { get { return "admins"; } }
        public CmdAdmins() { }

        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            if (Server.operators.All().Count > 0)
            {
                Server.superOps.All().ForEach(delegate(string name) { message += ", " + name; });
                p.SendMessage(Server.superOps.All().Count + " &4Admin" + ((Server.superOps.All().Count != 1) ? "s" : "") + "&e: " + message.Remove(0, 2) + ".");
            }
            else { p.SendMessage("Nobody is admin. What's wrong with this server?"); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/admins - List the admins of the server");
        }
    }
}
