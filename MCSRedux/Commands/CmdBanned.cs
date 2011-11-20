using System;
using System.IO;

namespace Minecraft_Server {
	public class CmdBanned : Command {
		public override string name { get { return "banned"; } }
		public CmdBanned() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			if (Server.banned.All().Count > 0) {
				Server.banned.All().ForEach(delegate(string name) { message += ", "+name; } );
				p.SendMessage(Server.banned.All().Count+" player"+((Server.banned.All().Count!=1) ? "s" : "")+" &8banned&e: "+message.Remove(0,2)+".");
			} else { p.SendMessage("Nobody is banned."); }
		} public override void Help(Player p)  {
			p.SendMessage("/banned - Lists all banned names.");
		}
	}
}