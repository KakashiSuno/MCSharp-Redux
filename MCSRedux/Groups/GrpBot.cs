using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class GrpBot : Group
    {
        public override LevelPermission Permission { get { return LevelPermission.Admin; } }
        public override string name { get { return "bots"; } }
        public override string color { get { return "[bot]&6"; } }
        public override bool canChat { get { return true; } }
        //public override bool canBuild { get { return false; } }
        CommandList _commands = new CommandList();
        public override CommandList commands { get { return _commands; } }
        public GrpBot()
        {
            
            _commands.Add(Command.all.Find("goto"));
            
        }
    }
}
