using System;

namespace Minecraft_Server {
	public class CmdInfo : Command {
		public override string name { get { return "info"; } }
		public CmdInfo() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); } else {
				p.SendMessage("MCsharp Revision " + Server.Version + " written in C# by KakashiSuno, Neko_Baron and the MCSharp team based off of copyboy's C# net code.");
				p.SendMessage("Official channel: &2#mcsharp @ irc.esper.net&e.");
			}
		} public override void Help(Player p)  {
			p.SendMessage("/info - Displays the server information.");
		}
	}
}
