using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdDna : Command
    {
        public override string name { get { return "dna"; } }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                string id = DNA.GetData(p.name, TableData.ID);
                string ip = DNA.GetData(p.name, TableData.IP);
                string rank = DNA.GetData(p.name, TableData.Rank);

                p.SendMessage(p.name);
                p.SendMessage(id);
                p.SendMessage(ip);
                p.SendMessage(rank);
            }
            else
            {
                Player who = Player.Find(message);

                string id = DNA.GetData(who.name, TableData.ID);
                string ip = DNA.GetData(who.name, TableData.IP);
                string rank = DNA.GetData(who.name, TableData.Rank);

                p.SendMessage(who.name);
                p.SendMessage(id);
                p.SendMessage(ip);
                p.SendMessage(rank);
            }
        }

        public override void Help(Player p)
        {
            p.SendMessage("/dna <name> - displays the DNA iformation of the specified player");
        }
    }
}
