using System;

namespace Minecraft_Server
{
    public class CmdDeop : Command
    {
        public override string name { get { return "deop"; } }
        public CmdDeop() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (!Server.operators.Contains(message)) { p.SendMessage(message + " isn't operator."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now " + Group.standard.color + Group.standard.name + "&e!"); }
            else
            {
                Player.GlobalChat(who, who.color + who.name + "&e is now " + Group.standard.color + Group.standard.name + "&e!", false);
                who.group = Group.standard; 
				who.color = who.group.color;
				Player.checkDev(who);
				Player.GlobalDie(who, false);
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.operators.Remove(message); Server.operators.Save("admins.txt", false);
            Server.Log("DEOPPED: " + message.ToLower()); IRCBot.Say("DEOPPED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/deop <player> - Degrades an operator to guest.");
        }
    }
}