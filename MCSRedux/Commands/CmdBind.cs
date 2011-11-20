using System;

namespace Minecraft_Server {
	public class CmdBind : Command {
		public override string name { get { return "bind"; } }
		public CmdBind() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			if (message.Split(' ').Length > 2) { Help(p); return; }
			message = message.ToLower();
			int pos = message.IndexOf(' ');
			if (pos != -1) {
				byte b1 = Block.Byte(message.Substring(0,pos));
				if (b1==255) { p.SendMessage("There is no block \""+message.Substring(0,pos)+"\"."); return; }
				if (!Block.Placable(b1)) { p.SendMessage("You can't bind "+Block.Name(b1)+"."); return; }
                byte b2 = Block.Byte(message.Substring(pos + 1));
                if (b2 == 255) { p.SendMessage("There is no block \"" + message.Substring(pos + 1) + "\"."); return; }
                if (b2 == 0 || b2 == 8 || b2 == 10) { p.SendMessage("You can't bind " + Block.Name(b2) + "."); return; }
                if (Block.Placable(b2)) { p.SendMessage(Block.Name(b2) + " isn't a special block."); return; }
                if (p.bindings[b1] == b2) { p.SendMessage(Block.Name(b1) + " is already bound to " + Block.Name(b2) + "."); return; }
				p.bindings[b1] = b2;
                message = Block.Name(b1) + " bound to " + Block.Name(b2) + ".";
				for (byte i=0;i<128;++i) 
                {
                    
					byte b = i;
					if (p.bindings[i]==b2 && i!=b1 && Block.Placable(b)) {
						message += " Unbound "+Block.Name(b)+".";
						p.bindings[i] = i; break;
					}
				} p.SendMessage(message);
			} else {
				byte b = Block.Byte(message);
				if (b==255) { p.SendMessage("There is no block \""+message+"\"."); return; }
				if (!Block.Placable(b)) { p.SendMessage("You can't place "+Block.Name(b)+"."); return; }
                if (p.bindings[b] == b) { p.SendMessage(Block.Name(b) + " isn't bound."); return; }
                p.bindings[b] = b; p.SendMessage("Unbound " + Block.Name(b) + ".");
			}
		} public override void Help(Player p)  {
			p.SendMessage("/bind <block> [type] - Replaces block with type.");
		}
	}
}