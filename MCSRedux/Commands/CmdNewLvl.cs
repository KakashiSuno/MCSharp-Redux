using System;
using System.Collections.Generic;
using System.Text;

namespace Minecraft_Server
{
    class CmdNewLvl : Command
    {
        public override string name { get { return "newlvl"; } }
        public CmdNewLvl() { }

        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }

            string[] parameters = message.Split(' '); // Grab the parameters from the player's message
            if (parameters.Length == 5) // make sure there are 5 params
            {
                switch(parameters[4])
                {
                    case "flat":
                    case "pixel":
                    case "island":
                    case "mountains":
                    case "ocean":
                    case "forest":

                        break;

                    default:
                        p.SendMessage("Valid types: island, mountains, forest, ocean, flat, pixel"); return;
                }

                string name = parameters[0];
                // create a new level...
                try
                {
                    Level lvl = new Level(name,
                                          Convert.ToUInt16(parameters[1]),
                                          Convert.ToUInt16(parameters[2]),
                                          Convert.ToUInt16(parameters[3]),
                                          parameters[4]);
                    lvl.Save(); //... and save it.
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
                Player.GlobalMessage("Level " + name + " created"); // The player needs some form of confirmation.
            }
            else
                p.SendMessage("Not enough parameters! <name> <x> <y> <z> <type>"); // Yell at the player for failing
        }
        public override void Help(Player p)
        {
            p.SendMessage("/newlvl - creates a new level.");
            p.SendMessage("/newlvl mapname 128 64 128 type");
        }
    }
}
