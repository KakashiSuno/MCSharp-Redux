using System;

namespace Minecraft_Server {
	public class CmdMe : Command {
		public override string name { get { return "me"; } }
		public CmdMe() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; } else {
                if (Properties.worldChat)
                {
                    Player.GlobalChat(p, p.color + "*" + p.name + " " + message, false);
                }
                else
                {
                    Player.GlobalChatLevel(p, p.color + "*" + p.name + " " + message, false);
                }
			}
		} public override void Help(Player p)  {
			p.SendMessage("/me <action> - Roleplay-like action message.");
		}
	}
}