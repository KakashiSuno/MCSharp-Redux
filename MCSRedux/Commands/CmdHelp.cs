using System;

namespace Minecraft_Server {
	public class CmdHelp : Command {
		public override string name { get { return "help"; } }
		public CmdHelp() {  }
		public override void Use(Player p,string message)  
        {
			message.ToLower(); 
            if (message == "") 
            {
				p.group.ToString();     //What is this line even for???
                p.group.commands.All().ForEach(delegate(Command cmd) { message += ", " + cmd.name; });
				p.SendMessage("Available commands: " + message.Remove(0,2)+". For more info about a specific command write \"/help <command>\".");
			} 
            else 
            {
				Command cmd = Command.all.Find(message);
				if (cmd == null) { p.SendMessage("There is no command \""+message+"\""); }
				else { cmd.Help(p); }
			}
		} 
        public override void Help(Player p)  
        {
			p.SendMessage("/help [command] - Shows a list of commands or more detail for a specific command.");
		}
	}
}