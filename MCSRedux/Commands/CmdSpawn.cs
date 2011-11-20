using System;

namespace Minecraft_Server {
	public class CmdSpawn : Command {
		public override string name { get { return "spawn"; } }
		public CmdSpawn() {  }
		public override void Use(Player p,string message)  {
			if (message != "") { Help(p); return; }
			ushort x = (ushort)((0.5+p.level.spawnx)*32);
            ushort y = (ushort)((1 + p.level.spawny) * 32);
            ushort z = (ushort)((0.5 + p.level.spawnz) * 32);
			unchecked { p.SendPos((byte)-1,x,y,z,
                                    p.level.rotx,
                                    p.level.roty);
            }
		} public override void Help(Player p)  {
			p.SendMessage("/spawn - Teleports yourself to the spawn location.");
		}
	}
}