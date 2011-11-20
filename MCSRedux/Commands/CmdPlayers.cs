using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdPlayers : Command
    {

        public override string name { get { return "players"; } }
        public override void Use(Player p, string message)
        {
            p.SendMessage("There are " + Player.number + " players online");
            string ops = "";
            string guests = "";
            string builders = "";
            foreach (Player pl in Player.players)
            {
                if (!pl.hidden)
                {
                    switch (pl.group.name.ToLower())
                    {
                        case "guest":
                            guests += " " + pl.name + ",";
                            break;
                        case "operator":
                        case "superop":
                            ops += " " + pl.name + ",";
                            break;
                        case "builder":
                        case "advbuilder":
                            builders += " " + pl.name + ",";
                            break;
                    }
                }
            }
            p.SendMessage(":&3Operators&e:" + ops.Trim(','));
            p.SendMessage(":&2Builders&e:" + builders.Trim(','));;
            p.SendMessage(":&7Guests&e:" + guests.Trim(','));
        }

        public override void Help(Player p)
        {
            p.SendMessage("/players - Shows name and general rank of all players");
        }
    }
}
