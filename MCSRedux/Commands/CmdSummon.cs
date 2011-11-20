using System;
using System.IO;

namespace Minecraft_Server {
	public class CmdSummon : Command {
		public override string name { get { return "summon"; } }
		public CmdSummon() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			Player who = Player.Find(message);
			if (who == null) { p.SendMessage("There is no player \""+who+"\"!"); return; }
			if (p.level != who.level) { p.SendMessage(who.name+" is in a different level."); return; }
				unchecked { who.SendPos((byte)-1,p.pos[0],p.pos[1],p.pos[2],p.rot[0],0); }
				who.SendMessage("You were summoned by "+p.color+p.name+"&e.");
			} public override void Help(Player p)  {
				p.SendMessage("/summon <player> - Summons a player to your position.");
			}
		}
	}