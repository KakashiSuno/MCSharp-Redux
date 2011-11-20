using System;

namespace Minecraft_Server
{
    public class GrpGuest : Group
    {
        public override LevelPermission Permission { get { return LevelPermission.Guest; } }
        public override string name { get { return "guest"; } }
        public override string color { get { return "&7"; } }
        public override bool canChat { get { return true; } }
        //public override bool canBuild { get { return false; } }
        public override CommandList commands { get { return _commands; } }
        CommandList _commands = new CommandList();
        public GrpGuest()
        {
            _commands.Add(Command.all.Find("about"));
            _commands.Add(Command.all.Find("admins"));
            //_commands.Add(Command.all.Find("abort"));
            _commands.Add(Command.all.Find("afk"));
            //_commands.Add(Command.all.Find("bind"));
            if (Properties.guestGoto)
            {
                _commands.Add(Command.all.Find("goto"));
            }
            _commands.Add(Command.all.Find("help"));
            _commands.Add(Command.all.Find("info"));
            if (Properties.guestGoto)
            {
                _commands.Add(Command.all.Find("levels"));
            }
            _commands.Add(Command.all.Find("mapinfo"));
            _commands.Add(Command.all.Find("me"));
            _commands.Add(Command.all.Find("ops"));
            _commands.Add(Command.all.Find("rules"));
            _commands.Add(Command.all.Find("time"));
            //_commands.Add(Command.all.Find("paint"));
            _commands.Add(Command.all.Find("players"));
            _commands.Add(Command.all.Find("spawn"));
            //_commands.Add(Command.all.Find("whodid"));
            _commands.Add(Command.all.Find("whois"));
            _commands.Add(Command.all.Find("whowas"));
            
            
            
        }
    }
}