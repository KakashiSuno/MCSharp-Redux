using System;

namespace Minecraft_Server {
	public class CmdWraptest : Command {
		public override string name { get { return "wraptest"; } }
		public CmdWraptest() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			p.SendMessage("So you want to see the awesome wrapping feature i built into "+
			              "the awesome server? Did i mention, that I'm awesome, too? :P "+
			              "Watever, you saw it, didn't you? The wordwrap, not me awesoming "+
			              "around, of course! So can I do anything else for you? ... "+
			              "Oh you want OP? Well FUCK YOU!");
		} public override void Help(Player p)  {
			p.SendMessage("/wraptest - Sends a long message back to you.");
		}
	}
}