using System;

namespace Minecraft_Server
{
    public class CmdAdvBuilder : Command
    {
        public override string name { get { return "advbuilder"; } }
        public CmdAdvBuilder() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { if (p != null)Help(p); return; }
            if (!Player.ValidName(message)) { if (p != null)p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (Server.banned.Contains(message)) { if (p != null)p.SendMessage("You can't allow a banned player to build!"); return; }
            if (Server.advbuilders.Contains(message)) { if (p != null)p.SendMessage(message + " is already an adv builder."); return; }
            if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
            if (Server.operators.Contains(message)) { Server.operators.Remove(message); Server.operators.Save("admins.txt"); }
            if (Server.superOps.Contains(message)) { if (p != null)p.SendMessage(message + " is a Super Op, mutiny averted."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now a &aadv builder&e!"); }
            else
            {
                who.BlockAction = 0;
                Player.GlobalChat(who, who.color + who.name + "&e is now a &aadv builder!", false);
                who.group = Group.Find("advbuilder"); 
				who.color = who.group.color;
				Player.GlobalDie(who, false);
                who.SendMessage("You are now ranked Advanced Builder, type /help for your new set of commands.");
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.advbuilders.Add(message); Server.advbuilders.Save("advbuilders.txt", false);
            Server.Log("ADV BUILDER: " + message.ToLower());
            IRCBot.Say("ADV BUILDER: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/advbuilder <player> - Promotes a player to Advanced Builder");

        }
    }
}