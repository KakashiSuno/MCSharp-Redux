using System;

namespace Minecraft_Server
{
    public class CmdPermissionVisit : Command
    {
        public override string name { get { return "pervisit"; } }
        public CmdPermissionVisit() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }
                p.level.permissionvisit = Perm;
                Server.Log(p.level.name + " visit permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "visit permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }

                foreach (Level level in Server.levels)
                {
                    if (level.name.ToLower() == t.ToLower())
                    {
                        level.permissionvisit = Perm;
                        Server.Log(level.name + " visit permission changed to " + s + ".");
                        Player.GlobalMessageLevel(level, "visit permission changed to " + s + ".");
                        if (p.level != level) { p.SendMessage("visit permission changed to " + s + " on " + level.name + "."); }
                        return;
                    }
                }
                p.SendMessage("There is no level \"" + s + "\" loaded.");
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/PerVisit <rank> - Sets visiting permission for a map.");
        }
    }
}