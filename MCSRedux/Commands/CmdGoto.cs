using System;

namespace Minecraft_Server {
	public class CmdGoto : Command {
		public override string name { get { return "goto"; } }
		public CmdGoto() {  }
		public override void Use(Player p,string message)  {
			if (message == "") { Help(p); return; }
			
			foreach (Level level in Server.levels) {
				if (level.name.ToLower() == message.ToLower()) 
                {
					if (p.level == level) { p.SendMessage("You are already in \""+level.name+"\"."); return; }
                    if (p.group.Permission < level.permissionvisit) { p.SendMessage("Your not allowed to goto " + level.name + "."); return; }
                    p.Loading = true;
					foreach (Player pl in Player.players) 
					{
						if (p.level == pl.level && p != pl) 
						{ 
							p.SendDie(pl.id); 
						}       //Kills current player list for player, should be now fixed.
					}
                    foreach (PlayerBot b in PlayerBot.playerbots)
                    {
                        if (p.level == b.level) 
						{ 
							p.SendDie(b.id); 
						}       //Kills current bot list for player
                    } 
                    p.ClearBlockchange();
                    p.BlockAction = 0;
                    p.painting = false;
                    Player.GlobalDie(p,true);
					p.level = level; 
					p.SendMotd(); 
					p.SendMap();
					ushort x = (ushort)((0.5+level.spawnx)*32);
					ushort y = (ushort)((1+level.spawny)*32);
					ushort z = (ushort)((0.5+level.spawnz)*32);
					if (!p.hidden)
					{
						Player.GlobalSpawn(p, x, y, z, level.rotx, level.roty, true);
					}
					else unchecked
						{
							p.SendPos((byte)-1, x, y, z, level.rotx, level.roty);
						}
					foreach (Player pl in Player.players) 
					{
						if (pl.level == p.level && p != pl && !pl.hidden)
						{ 
							p.SendSpawn(pl.id,pl.color+pl.name,pl.pos[0],pl.pos[1],pl.pos[2],pl.rot[0],pl.rot[1]); 
						}
					}
                    foreach (PlayerBot b in PlayerBot.playerbots)   //Send bots to the player
                    {
                        if (b.level == p.level)
                        { 
							p.SendSpawn(b.id, b.color + b.name, b.pos[0], b.pos[1], b.pos[2], b.rot[0], b.rot[1]); 
						}
                    }
                    if (!p.hidden) 
					{ 
						Player.GlobalChat(p, p.color + "*" + p.name + "&e went to \"" + level.name + "\".", false); 
					}
                    p.Loading = false;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
					return;
				}
			} p.SendMessage("There is no level \""+message+"\" loaded.");
		} public override void Help(Player p)  {
			p.SendMessage("/goto <mapname> - Teleports yourself to a different level.");
		}
	}
}