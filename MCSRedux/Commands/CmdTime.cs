using System;

namespace Minecraft_Server
{
    public class CmdTime : Command
    {
        public override string name { get { return "time"; } }
        public CmdTime() { }
        public override void Use(Player p, string message)
        {
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            message = "Server time is " + time;
            p.SendMessage(message);
            //message = "full Date/Time is " + DateTime.Now.ToString();
            //p.SendMessage(message);
        }
        public override void Help(Player p)
        {
            p.SendMessage("/time - Shows the server time.");
        }
    }
}