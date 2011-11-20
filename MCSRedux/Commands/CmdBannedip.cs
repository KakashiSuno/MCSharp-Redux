using System;
using System.IO;

namespace Minecraft_Server {
	public class CmdBannedip : Command {
		public override string name { get { return "bannedip"; } }
		public CmdBannedip() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			if (Server.bannedIP.All().Count > 0) {
				Server.bannedIP.All().ForEach(delegate(string name) { message += ", "+name; } );
				p.SendMessage(Server.bannedIP.All().Count.ToString()+" IP"+((Server.bannedIP.All().Count!=1) ? "s" : "")+"&8banned&e: "+message.Remove(0,2)+".");
			} else { p.SendMessage("No IP is banned."); }
		} public override void Help(Player p)  {
			p.SendMessage("/bannedip - Lists all banned IPs.");
		}
	}
}