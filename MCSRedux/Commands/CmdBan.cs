using System;

namespace Minecraft_Server
{
    public class CmdBan : Command
    {
        public override string name { get { return "ban"; } }
        public CmdBan() { }
        public override void Use(Player p, string message)
        {
            bool stealth = false;
            if (message[0] == '#')
            {
                message = message.Remove(0, 1).Trim();
                stealth = true;
                Server.Log("Stealth Ban Atempted");
            }

            if (p != null)
            {

                if (message == "") { Help(p); return; }
                if (!Player.ValidName(message)) { p.SendMessage("Invalid name \"" + message + "\"."); return; }
                if (Server.operators.Contains(message)) { p.SendMessage("You can't ban an operator!"); return; }
                if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
                if (Server.advbuilders.Contains(message)) { Server.advbuilders.Remove(message); Server.advbuilders.Save("advbuilders.txt"); }
                if (Server.superOps.Contains(message)) { p.SendMessage("You can't ban a Super Op!"); return; }
                if (Server.banned.Contains(message)) { p.SendMessage(message + " is already banned."); return; }
                Player who = Player.Find(message);
                
                if (who == null) 
                {
                    if (Player.checkDevS(message))
                    {
                        Player.GlobalMessage(message + " &f(offline)&e was &8banned&e?! You jerks!");
                    }
                    else
                    {
                        Player.GlobalMessage(message + " &f(offline)&e is now &8banned&e!");
                    }
                }
                else
                {
                    if (Player.checkDev(who))
                    {
                        Player.GlobalChat(who, p.color + p.name + "&e just &8banned " + who.color + who.name + "&e! What a total git!", false);
                    }
                    else
                    {
                        if (stealth)
                        {
                            Player.GlobalMessageOps(who.color + who.name + "&e is now STEALTH &8banned&e!");
                        }
                        else
                        {
                            Player.GlobalChat(who, who.color + who.name + "&e is now &8banned&e!", false);
                        }
                    }
                    who.group = Group.Find("banned"); who.color = who.group.color; Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                } Server.banned.Add(message); Server.banned.Save("banned.txt", false); IRCBot.Say(message + " was banned by " + p.name);
                Server.Log("BANNED: " + message.ToLower());
            }
            else
            {
                if (message == "") {return; }
                if (!Player.ValidName(message)) { return; }
                if (Server.operators.Contains(message)) { return; }
                if (Server.builders.Contains(message)) { Server.builders.Remove(message); Server.builders.Save("builders.txt"); }
                if (Server.superOps.Contains(message)) { return; }
                if (Server.banned.Contains(message)) { return; }
                Player who = Player.Find(message);
                if (who == null) { Player.GlobalMessage(message + " &f(offline)&e is now &8banned&e!"); }
                else
                {
                    if (stealth)
                    {
                        Player.GlobalMessageOps(who.color + who.name + "&e is now STEALTH &8banned&e!");
                    }
                    else
                    {
                        Player.GlobalChat(who, who.color + who.name + "&e is now &8banned&e!", false);
                    }
                    who.group = Group.Find("banned"); who.color = who.group.color; Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                } Server.banned.Add(message); Server.banned.Save("banned.txt", false); IRCBot.Say(message + " was banned by [console]");
                Server.Log("BANNED: " + message.ToLower());
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/ban <player> - Bans a player without kicking him.");
            p.SendMessage("Add # before name to stealth ban.");
        }
    }
}