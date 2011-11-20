using System;

namespace Minecraft_Server 
{
	public class CmdAbout : Command 
    {
		public override string name { get { return "about"; } }

		public CmdAbout() {  }

		public override void Use(Player p,string message)  
        {
			if (message != "") { Help(p); return; }
			p.SendMessage("Break/build a block to display information.");
			p.ClearBlockchange();
			p.Blockchange += new Player.BlockchangeEventHandler(Blockchange);
		} 
        public override void Help(Player p)  
        {
			p.SendMessage("/about - Displays information about a block.");
		} public void Blockchange(Player p,ushort x,ushort y,ushort z,byte type) {
			p.ClearBlockchange();
			byte b = p.level.GetTile(x,y,z);
            if (b == Block.Zero) { p.SendMessage("Invalid Block(" + x + "," + y + "," + z + ")!"); return; }
			p.SendBlockchange(x,y,z,b);
			string message = "Block ("+x+","+y+","+z+"): ";
            message += "&f" + b + " = " + Block.Name(b);
			p.SendMessage(message+"&e.");
		}
	}
}