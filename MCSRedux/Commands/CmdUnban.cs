using System;

namespace Minecraft_Server
{
    public class CmdUnban : Command
    {
        public override string name { get { return "unban"; } }
        public CmdUnban() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
            if (!Server.banned.Contains(message)) { p.SendMessage(message + " isn't banned."); return; }
            Player who = Player.Find(message);
            if (who == null) { Player.GlobalMessage(message + " &8(banned)&e is now " + Group.standard.color + Group.standard.name + "&e!"); }
            else
            {
                Player.GlobalChat(who, who.color + who.name + "&e is now " + Group.standard.color + Group.standard.name + "&e!", false);
                who.group = Group.standard; who.color = who.group.color; Player.GlobalDie(who, false);
                Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
            } Server.banned.Remove(message); Server.banned.Save("banned.txt", false);
            Server.Log("UNBANNED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/unban <player> - Unbans a player.");
        }
    }
}