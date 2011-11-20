using System;
using System.IO;

namespace Minecraft_Server {
	public class CmdSetspawn : Command {
		public override string name { get { return "setspawn"; } }
		public CmdSetspawn() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			p.SendMessage("Spawn location changed.");
			p.level.spawnx = (ushort)(p.pos[0]/32);
            p.level.spawny = (ushort)(p.pos[1] / 32);
            p.level.spawnz = (ushort)(p.pos[2] / 32);
            p.level.rotx = p.rot[0];
            p.level.roty = 0;
		} public override void Help(Player p)  {
			p.SendMessage("/setspawn - Set the default spawn location.");
		}
	}
}