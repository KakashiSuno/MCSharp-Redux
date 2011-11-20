using System;
using System.IO;

namespace Minecraft_Server {
	public class CmdTp : Command {
		public override string name { get { return "tp"; } }
		public CmdTp() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			Player who = Player.Find(message);
			if (who == null) { p.SendMessage("There is no player \""+message+"\"!"); return; }
            if (p.level != who.level) { p.SendMessage("-" + who.group.color + who.name + "&e- is on &b" + who.level.name + "&e."); return; }
			unchecked { p.SendPos((byte)-1,who.pos[0],who.pos[1],who.pos[2],who.rot[0],0); }
		} public override void Help(Player p)  {
			p.SendMessage("/tp <player> - Teleports yourself to a player.");
		}
	}
}