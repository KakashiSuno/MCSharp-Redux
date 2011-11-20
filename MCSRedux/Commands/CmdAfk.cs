using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdAfk : Command
    {
        public override string name { get { return "afk"; } }
        public CmdAfk() { }

        public override void Use(Player p, string message)
        {
            if (message != "list")
            {
                if (Server.afkset.Contains(p.name))
                {
                    Server.afkset.Remove(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&e- is no longer AFK");
                    IRCBot.Say(p.name + " is no longer AFK");
                }
                else
                {
                    Server.afkset.Add(p.name);
                    Player.GlobalMessage("-" + p.group.color + p.name + "&e- is AFK " + message);
                    IRCBot.Say(p.name + " is AFK " + message);
                }
            }
            else
            {
                foreach (string s in Server.afkset)
                {
                    p.SendMessage(s);
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/afk <reason> - mark yourself as AFK. Use again to mark yourself as back");
        }
    }
}
