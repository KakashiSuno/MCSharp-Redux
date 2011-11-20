using System;

namespace Minecraft_Server
{
    public class CmdKickban : Command
    {
        public override string name { get { return "kickban"; } }
        public CmdKickban() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            string who = message;
            int index = message.IndexOf(' ');
            string kickmessage = "";
            if (index != -1)
            {
                who = message.Substring(0, index);
                kickmessage = message.Substring(index + 1);
            } Player kick = Player.Find(who);
            if (kick != null)
            {
                if (kick == p) { p.SendMessage("You can't kickban yourself!"); return; }
                if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
                if (Server.advbuilders.Contains(message)) { Server.advbuilders.Remove(message); Server.advbuilders.Save("advbuilders.txt"); }
                if (kick.group == Group.Find("operator")) { p.SendMessage("You can't kickban an operator!"); return; }
                if (kick.group == Group.Find("superop")) { p.SendMessage("You can't kickban a Super Op!"); return; }
                if (Server.banned.Contains(kick.name)) { Command.all.Find("kick").Use(p, message); return; }
                if (index == -1) { kick.Kick("You were kickbanned by " + p.name + "!"); }
                else { kick.Kick(kickmessage); } Server.banned.Add(kick.name);
                Server.banned.Save("banned.txt", false);
                Server.Log("BANNED: " + message.ToLower());
                IRCBot.Say(kick.name + " was banned by " + p.name);
            }
            else { Command.all.Find("ban").Use(p, who); }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/kickban <player> [message] - Kicks and bans a player with an optional message.");
        }
    }
}