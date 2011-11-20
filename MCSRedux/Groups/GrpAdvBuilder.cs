using System;

namespace Minecraft_Server
{
    public class GrpAdvBuilder : Group
    {
        public override LevelPermission Permission { get { return LevelPermission.AdvBuilder; } }
        public override string name { get { return "advbuilder"; } }
        public override string color { get { return "&2"; } }
        public override bool canChat { get { return true; } }
        //public override bool canBuild { get { return true; } }
        public override CommandList commands { get { return _commands; } }
        CommandList _commands = new CommandList();
        public GrpAdvBuilder()
        {
            _commands.Add(Command.all.Find("about"));
            _commands.Add(Command.all.Find("abort"));     //They dont have cuboid/replace and dont need it then
            _commands.Add(Command.all.Find("activelava"));
            _commands.Add(Command.all.Find("activewater"));
            _commands.Add(Command.all.Find("admins"));
            _commands.Add(Command.all.Find("afk"));
            //_commands.Add(Command.all.Find("bind"));
            _commands.Add(Command.all.Find("cuboid"));
            _commands.Add(Command.all.Find("goto"));
            _commands.Add(Command.all.Find("help"));
            _commands.Add(Command.all.Find("info"));
            _commands.Add(Command.all.Find("kick"));
            _commands.Add(Command.all.Find("lava"));
            _commands.Add(Command.all.Find("levels"));
            _commands.Add(Command.all.Find("mapinfo"));
            _commands.Add(Command.all.Find("me"));
            _commands.Add(Command.all.Find("ops"));
            _commands.Add(Command.all.Find("paint"));
            _commands.Add(Command.all.Find("players"));
            _commands.Add(Command.all.Find("rules"));
            _commands.Add(Command.all.Find("spawn"));
            _commands.Add(Command.all.Find("time"));
            _commands.Add(Command.all.Find("tp"));
            //_commands.Add(Command.all.Find("whodid"));
            _commands.Add(Command.all.Find("water"));
            _commands.Add(Command.all.Find("whois"));
            _commands.Add(Command.all.Find("whowas"));
        }
    }
}