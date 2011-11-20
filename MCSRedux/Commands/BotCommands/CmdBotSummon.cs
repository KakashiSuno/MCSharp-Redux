using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdBotSummon : Command
    {
        public override string name { get { return "botsummon"; } }
        public CmdBotSummon() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            PlayerBot who = PlayerBot.Find(message);
            if (who == null) { p.SendMessage("There is no bot " + message + "!"); return; }
            if (p.level != who.level) { p.SendMessage(who.name + " is in a different level."); return; }
            who.SetPos( p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0);
            //who.SendMessage("You were summoned by " + p.color + p.name + "&e.");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/botsummon <name> - Summons a bot to your position.");
        }
    }
}