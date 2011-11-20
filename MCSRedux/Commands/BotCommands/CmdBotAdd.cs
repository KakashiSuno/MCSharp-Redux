using System;
using System.IO;

namespace Minecraft_Server
{
    public class CmdBotAdd : Command
    {
        public override string name { get { return "botadd"; } }
        public CmdBotAdd() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            PlayerBot who = PlayerBot.Find(message);
            if (who != null) { p.SendMessage("bot " + who.name + " already exists!"); return; }
            if (!PlayerBot.ValidName(message)) { p.SendMessage("bot name " + message + " not valid!"); return; }
            PlayerBot.playerbots.Add(new PlayerBot(message, p.level, p.pos[0], p.pos[1], p.pos[2], p.rot[0], 0));
            //who.SendMessage("You were summoned by " + p.color + p.name + "&e.");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/botadd <name> - Add a  new bot at your position.");
        }
    }
}