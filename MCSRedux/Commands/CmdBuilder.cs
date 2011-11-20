using System;

namespace Minecraft_Server
{
    public class CmdBuilder : Command
    {
        public override string name { get { return "builder"; } }
        public CmdBuilder() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { if (p != null)Help(p); return; }
            if (!Player.ValidName(message)) { if (p != null)p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (Server.banned.Contains(message)) { if (p != null)p.SendMessage("You can't allow a banned player to build!"); return; }
            if (Server.builders.Contains(message)) { if (p != null)p.SendMessage(message + " is already builder."); return; }
            if (Server.advbuilders.Contains(message)) { Server.advbuilders.Remove(message); Server.advbuilders.Save("advbuilders.txt"); }
            if (Server.operators.Contains(message)) { Server.operators.Remove(message); Server.operators.Save("admins.txt"); }
            if (Server.superOps.Contains(message)) { if (p != null)p.SendMessage(message + " is a Super Op, mutiny averted."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now a &abuilder&e!"); }
            else
            {
                who.BlockAction = 0;
                Player.GlobalChat(who, who.color + who.name + "&e is now a &abuilder!", false);
                who.group = Group.Find("builder"); 
				who.color = who.group.color;
				Player.GlobalDie(who, false);
                who.SendMessage("You are now ranked Builder, type /help for your new set of commands.");
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.builders.Add(message); Server.builders.Save("builders.txt", false);
            Server.Log("BUILDER: " + message.ToLower());
            IRCBot.Say("BUILDER: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/builder <player> - Promotes a player to builder.");

        }
    }
}