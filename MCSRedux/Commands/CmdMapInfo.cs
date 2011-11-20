using System;

namespace Minecraft_Server
{
    public class CmdMapInfo : Command
    {
        public override string name { get { return "mapinfo"; } }
        public CmdMapInfo() { }
        public override void Use(Player p, string message)
        {
            if (message != "") { Help(p); return; }
            p.SendMessage("Currently on &b" + p.level.name + "&e X:" + p.level.width.ToString() + " Y:" + p.level.depth.ToString() + " Z:" + p.level.height.ToString());
            switch (p.level.physics)
            {
                case 0:
                    p.SendMessage("Physics is &cOFF&e.");
                    break;

                case 1:
                    p.SendMessage("Physics is &aNormal&e.");
                    break;

                case 2:
                    p.SendMessage("Physics is &aAdvanced&e.");
                    break;
            }

            p.SendMessage("Build rank = " + Level.PermissionToName(p.level.permissionbuild) + " : Visit rank = " + Level.PermissionToName(p.level.permissionvisit) + ".");
        }
        public override void Help(Player p)
        {
            p.SendMessage("/mapinfo - Display details of the current map.");
        }
    }
}