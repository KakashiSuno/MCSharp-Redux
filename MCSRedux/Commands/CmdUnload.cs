using System;
using System.IO;
using System.Collections.Generic;

namespace Minecraft_Server {
	public class CmdUnload : Command {
		public override string name { get { return "unload"; } }
		public CmdUnload() {  }
		public override void Use(Player p,string message)  {
			foreach (Level level in Server.levels) {
				if (level.name.ToLower() == message.ToLower()) {
					if (level == Server.mainLevel) { p.SendMessage("You can't unload the main level."); return; }
					Player.players.ForEach(delegate(Player pl) { if (pl.level == level) { Player.GlobalDie(pl,true); } });
                    PlayerBot.playerbots.ForEach(delegate(PlayerBot b) { if (b.level == level) { b.GlobalDie(); } });       //destroy any bots on the level
					Player.players.ForEach(delegate(Player pl) { if (pl.level == level) { pl.SendMotd(); } });
                    ushort x = (ushort)((0.5 + Server.mainLevel.spawnx) * 32);
                    ushort y = (ushort)((1 + Server.mainLevel.spawny) * 32);
                    ushort z = (ushort)((0.5 + Server.mainLevel.spawnz) * 32);
					Player.players.ForEach(delegate(Player pl) 
                    { 
                        if (pl.level == level) 
                        {
                            if (pl != p)
                            {
                                pl.Kick("Level unloaded, please rejoin.");
                            }
                        } 
                    });
					Server.levels.Remove(level);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (p.level != level)
                    {
                        p.SendMessage("Level \"" + level.name + "\" unloaded.");
                    }
                    else
                    {
                        p.Kick("Level unloaded, please rejoin.");
                    }
					return;
				}
			} p.SendMessage("There is no level \""+message+"\" loaded.");
		} public override void Help(Player p)  {
			p.SendMessage("/unload [level] - Unloads a level.");
		}
	}
}