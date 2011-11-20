using System;

namespace Minecraft_Server
{
    public class CmdWhowas : Command
    {
        public override string name { get { return "whowas"; } }
        public CmdWhowas() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            Player pl = Player.Find(message); if (pl != null)
            { p.SendChat(p, pl.color + pl.name + "&e is online, use /whois instead."); return; }
            if (!Player.left.ContainsKey(message.ToLower()))
            { p.SendMessage("No entry found for \"" + message + "\"."); return; }
            LeftPlayer who = Player.left[message.ToLower()];
            message = "&e" + who.name + " is " + Player.GetColor(who.name) + Player.GetGroup(who.name).name + "&e.";
            if (p.group == Group.Find("operator") || p.group == Group.Find("superOp"))
            {
                message += " IP: " + who.ip + ".";
                /*if (Player.GetGroup(who.name) != Group.Find("operator"))
                {
                    p.SendChat(p, message);
                    //message = "&eActions: " + who.actions.Count + ".";
                    message += " Write \"/undo " + who.name + "\" to undo actions.";
                }
                //else { message += " Actions: " + who.actions.Count + "."; }*/
            } p.SendChat(p, message);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/whowas <name> - Displays information about someone who left.");
        }
    }
}