using System;

namespace Minecraft_Server
{
    public class GrpOperator : Group
    {
        public override LevelPermission Permission { get { return LevelPermission.Operator; } }
        public override string name { get { return "operator"; } }
        public override string color { get { return "&3"; } }
        public override bool canChat { get { return true; } }
        //public override bool canBuild { get { return true; } }
        public override CommandList commands { get { return _commands; } }
        CommandList _commands = new CommandList();
        public GrpOperator()
        {
            _commands.Add(Command.all.Find("about"));
            _commands.Add(Command.all.Find("abort"));
            _commands.Add(Command.all.Find("activelava"));
            _commands.Add(Command.all.Find("activewater"));
            _commands.Add(Command.all.Find("admins"));
            _commands.Add(Command.all.Find("advbuilder"));
            _commands.Add(Command.all.Find("afk"));
            _commands.Add(Command.all.Find("ban"));
            _commands.Add(Command.all.Find("banip"));
            _commands.Add(Command.all.Find("banned"));
            _commands.Add(Command.all.Find("bannedip"));
            _commands.Add(Command.all.Find("bind"));
            _commands.Add(Command.all.Find("builder"));
            _commands.Add(Command.all.Find("cuboid"));
            //_commands.Add(Command.all.Find("color"));         //Stop changing your color to black shark |:<
            //_commands.Add(Command.all.Find("deop"));
            _commands.Add(Command.all.Find("guest"));
            _commands.Add(Command.all.Find("goto"));
            _commands.Add(Command.all.Find("help"));
            _commands.Add(Command.all.Find("info"));
            _commands.Add(Command.all.Find("kick"));
            _commands.Add(Command.all.Find("kickban"));
            _commands.Add(Command.all.Find("lava"));
            _commands.Add(Command.all.Find("levels"));
            _commands.Add(Command.all.Find("load"));
            _commands.Add(Command.all.Find("mapinfo"));
            _commands.Add(Command.all.Find("me"));
            _commands.Add(Command.all.Find("op"));
            _commands.Add(Command.all.Find("opglass"));
            _commands.Add(Command.all.Find("ops"));
            _commands.Add(Command.all.Find("paint"));
            _commands.Add(Command.all.Find("players"));
            _commands.Add(Command.all.Find("replace"));
            _commands.Add(Command.all.Find("restore"));
            _commands.Add(Command.all.Find("resetbot"));
            _commands.Add(Command.all.Find("rules"));
            _commands.Add(Command.all.Find("save"));
            _commands.Add(Command.all.Find("say"));
            _commands.Add(Command.all.Find("setspawn"));
            _commands.Add(Command.all.Find("solid"));
            _commands.Add(Command.all.Find("spawn"));
            _commands.Add(Command.all.Find("summon"));
            _commands.Add(Command.all.Find("time"));
            _commands.Add(Command.all.Find("tp"));
            _commands.Add(Command.all.Find("unban"));
            _commands.Add(Command.all.Find("unbanip"));
            //_commands.Add(Command.all.Find("undo"));
            _commands.Add(Command.all.Find("unload"));
            _commands.Add(Command.all.Find("water"));
            //_commands.Add(Command.all.Find("whodid"));
            _commands.Add(Command.all.Find("whois"));
            _commands.Add(Command.all.Find("whowas"));
            
            //_commands.Add(Command.all.Find("newlvl"));
            //_commands.Add(Command.all.Find("deletelvl"));
        }
    }
}