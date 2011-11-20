using System;
using System.IO;
using System.Collections.Generic;

namespace Minecraft_Server {
	public class CmdLevels : Command {
		public override string name { get { return "levels"; } }
		public CmdLevels() {  }
		public override void Use(Player p,string message)  { // TODO
			if (message != "") { Help(p); return; }
			List<string> levels = new List<string>(Server.levels.Count);
			message = Server.mainLevel.name;
            string message2 = "";
            levels.Add(Server.mainLevel.name.ToLower());
            bool Once = false;
			Server.levels.ForEach(delegate(Level level) 
            { 
                if (level != Server.mainLevel) 
                {
                    if (level.permissionvisit <= p.group.Permission)
                    {
                        message += ", " + level.name;
                        levels.Add(level.name.ToLower());
                    }
                    else
                    {
                        if (!Once)
                        {
                            Once = true;
                            message2 += level.name;
                        }
                        else
                        {
                            message2 += ", " + level.name;
                        }
                    }
                } 
            });
            p.SendMessage("Loaded: &2" + message);
            p.SendMessage("Can't Goto: &c" + message2);
			message = "";
			DirectoryInfo di = new DirectoryInfo("levels/");
			FileInfo[] fi = di.GetFiles("*.lvl");
            Once = false;
            foreach (FileInfo file in fi)
            {
                if (!levels.Contains(file.Name.Replace(".lvl", "").ToLower()))
                {
                    if (!Once)
                    {
                        Once = true;
                        message += file.Name.Replace(".lvl", "");
                    }
                    else
                    {
                        message += ", " + file.Name.Replace(".lvl", "");
                    }
                }
            }
            p.SendMessage("Unloaded: &4" + message);
		} public override void Help(Player p)  {
			p.SendMessage("/levels - Lists all levels.");
		}
	}
}