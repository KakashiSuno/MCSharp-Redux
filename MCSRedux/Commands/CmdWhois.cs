using System;

namespace Minecraft_Server
{
    public class CmdWhois : Command
    {
        public override string name { get { return "whois"; } }
        public CmdWhois() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; } else { who = Player.Find(message); }
            if (who != null)
            {
                if (who == p)
                {
                    message = "&eYou are " + who.group.color + who.group.name + "&e.";
                }
                else
                {
                    message = who.color + who.name + "&e is " +
                        who.group.color + who.group.name + "&e on &b" + who.level.name + "&e.";
                }

                if (Server.afkset.Contains(who.name)) { message += "-AFK-"; }

                if (p.group == Group.Find("operator") || p.group == Group.Find("superOp"))
                {
                    message += " IP: " + who.ip + ".";
                }

                p.SendChat(p, message);
                if (Player.checkDev(who))
                {
                    p.SendMessage("Is a developer of MCSharp.");
                }
                else if(Player.checkSupporter(who))
                {
                    p.SendMessage("Is a supporter of MCSharp.");
                }
            }
            else { p.SendMessage("There is no player \"" + who + "\"!"); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/whois [player] - Displays information about someone.");
        }
    }
}