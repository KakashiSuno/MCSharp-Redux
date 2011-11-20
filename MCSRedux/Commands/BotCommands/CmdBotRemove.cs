using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdBotRemove : Command
    {
        public override string name { get { return "botremove"; } }
        public CmdBotRemove() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            PlayerBot who = PlayerBot.Find(message);
            if (who == null) { p.SendMessage("There is no bot " + who + "!"); return; }
            if (p.level != who.level) { p.SendMessage(who.name + " is in a different level."); return; }
            who.GlobalDie();
            //who.SendMessage("You were summoned by " + p.color + p.name + "&e.");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/botremove <name> - Remove a bot on the same level as you");
        }
    }
}