using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server.Commands
{
    class ScriptedCommand : Command
    {
        public override string name
        {
            get { throw new NotImplementedException(); }
        }

        public override void Use(Player p, string message)
        {
        }

        public override void Help(Player p)
        {
            throw new NotImplementedException();
        }
    }
}
