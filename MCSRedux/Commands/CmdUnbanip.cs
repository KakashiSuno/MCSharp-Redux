using System;
using System.Text.RegularExpressions;

namespace Minecraft_Server
{
    public class CmdUnbanip : Command
    {
        Regex regex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\." +
                                "([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
        public override string name { get { return "unbanip"; } }
        public CmdUnbanip() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (!regex.IsMatch(message)) { p.SendMessage("Not a valid ip!"); return; }
            if (p.ip == message) { p.SendMessage("You shouldn't be able to use this command..."); return; }
            if (!Server.bannedIP.Contains(message)) { p.SendMessage(message + " doesn't seem to be banned..."); return; }
            Player.GlobalMessage(message + " got &8unip-banned&e!");
            Server.bannedIP.Remove(message); Server.bannedIP.Save("banned-ip.txt", false);
            Server.Log("IP-UNBANNED: " + message.ToLower());
        }
        public override void Help(Player p)
        {
            p.SendMessage("/unbanip <ip> - Un-bans an ip.");
        }
    }
}