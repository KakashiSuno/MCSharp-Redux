using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Minecraft_Server
{
    class CmdRules : Command
    {
        public override string name { get { return "rules"; } }

        public override void Use(Player p, string message)
        {
            List<string> rules = new List<string>();
            if (!File.Exists("rules.txt"))
            {
                p.SendMessage("There is no rules.txt file to show.");
                return;
            }
            StreamReader r = File.OpenText("rules.txt");
            while (!r.EndOfStream)
                rules.Add(r.ReadLine());

            r.Close();

            Player who = null;
            if (message != "")
            {
                if (p.group == Group.Find("guest") | p.group == Group.Find("banned"))
                { p.SendMessage("You cant send /rules to another player!"); return; }
                who = Player.Find(message);
            }
            else
            {
                who = p;
            }

            if (who != null)
            {
                if (who.level == Server.mainLevel && Server.mainLevel.permissionbuild == LevelPermission.Guest) { who.SendMessage("You are currently on the guest map where anyone can build"); }
                who.SendMessage("Server Rules:");
                foreach (string s in rules)
                    who.SendMessage(s);
            }
            else
            {
                p.SendMessage("There is no player \"" + message + "\"!");
            }
        }

        public override void Help(Player p)
        {
            if (p.group.name != "operator" && p.group.name != "superop")
            {
                p.SendMessage("/rules - Displays server rules");
            }
            else
            {
                p.SendMessage("/rules [player]- Displays server rules to a player");
            }
        }
    }
}
