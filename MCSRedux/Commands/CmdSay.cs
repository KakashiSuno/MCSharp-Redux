using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdSay : Command
    {
        public override string name { get { return "say"; } }
        public CmdSay() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            message = "&e" + message; // defaults to yellow
            message = message.Replace("%", "&"); // Alow colors in global messages
            Player.GlobalChat(p,message,false);
            message = message.Replace("&", ""); // converts the MC color codes to IRC. Doesn't seem to work with multiple colors
            IRCBot.Say("Global: " + message);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/say - brodcasts a global message to everyone in the server.");
        }
    }
}
