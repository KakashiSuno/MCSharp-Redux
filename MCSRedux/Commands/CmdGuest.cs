using System;

namespace Minecraft_Server
{
    public class CmdGuest : Command
    {
        public override string name { get { return "guest"; } }
        public CmdGuest() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { if (p != null)Help(p); return; }
            if (!Player.ValidName(message)) { if (p != null) p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (Server.banned.Contains(message)) { if (p != null)p.SendMessage("Use /unban instead!"); return; }
            if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
            if (Server.advbuilders.Contains(message)) { Server.advbuilders.Remove(message); Server.advbuilders.Save("advbuilders.txt"); }
            if (Server.operators.Contains(message)) { Server.operators.Remove(message); Server.operators.Save("admins.txt"); }
            if (Server.superOps.Contains(message)) { if (p != null)p.SendMessage(message + " is a Super Op, mutiny averted."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now a &7guest&e!"); }
            else
            {
                who.BlockAction = 0;
                Player.GlobalChat(who, who.color + who.name + "&e is now a &7guest!", false);
                who.SendMessage("You are now ranked Guest, type /help for your new set of commands.");
                who.group = Group.Find("guest"); 
				who.color = who.group.color;
				Player.GlobalDie(who, false);
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            }
            Server.Log("GUEST: " + message.ToLower());
            IRCBot.Say("GUEST: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/guest <player> - Demotes a player to guest.");

        }
    }
}