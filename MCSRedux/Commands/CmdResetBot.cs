using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdResetBot : Command
    {
        public override string name { get { return "resetbot"; } }
        public CmdResetBot() { }

        public override void Use(Player p, string message)
        {
            if (p == null)
                IRCBot.Reset();
            else
                IRCBot.Reset();
        }
        public override void Help(Player p)
        {
            p.SendMessage("/resetbot - reloads the IRCBot. FOR EMERGENCIES ONLY!");
        }
    }
}
