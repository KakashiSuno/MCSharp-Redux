using System;

namespace Minecraft_Server {
	public class GrpBanned : Group {
        public override LevelPermission Permission { get { return LevelPermission.Guest; } }
		public override string name { get { return "banned"; } }
		public override string color { get { return "&7"; } }
		public override bool canChat { get { return true; } }
		//public override bool canBuild { get { return false; } }
		public override CommandList commands { get { return _commands; } }
		CommandList _commands = new CommandList();
		public GrpBanned() {
            _commands.Add(Command.all.Find("admins"));
			_commands.Add(Command.all.Find("help"));
			_commands.Add(Command.all.Find("info"));
            _commands.Add(Command.all.Find("mapinfo"));
			//_commands.Add(Command.all.Find("me"));
			_commands.Add(Command.all.Find("ops"));
			_commands.Add(Command.all.Find("spawn"));
            _commands.Add(Command.all.Find("time"));
			_commands.Add(Command.all.Find("whois"));
			_commands.Add(Command.all.Find("whowas"));
		}
	}
}