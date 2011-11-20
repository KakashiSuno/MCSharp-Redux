using System;

namespace Minecraft_Server
{
    public class CmdOp : Command
    {
        public override string name { get { return "op"; } }
        public CmdOp() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (Server.banned.Contains(message)) { p.SendMessage("You can't op a banned player!"); return; }
            if (Server.operators.Contains(message)) { p.SendMessage(message + " is already operator."); return; }
            if (Server.superOps.Contains(message)) { p.SendMessage(message + " is a Super Op, mutiny averted."); return; }
            if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
            if (Server.advbuilders.Contains(message)) { Server.advbuilders.Remove(message); Server.advbuilders.Save("advbuilders.txt"); }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now &3operator&e!"); }
            else
            {
                Player.GlobalChat(who, who.color + who.name + "&e is now &3operator&e!", false);
                who.group = Group.Find("operator"); 
				who.color = who.group.color;
				Player.GlobalDie(who, false);
                who.SendMessage("You are now ranked Operator, type /help for your new set of commands.");
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.operators.Add(message); Server.operators.Save("admins.txt", false);
            Server.Log("OPPED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/op <player> - Promotes a player to operator.");
        }
    }
}