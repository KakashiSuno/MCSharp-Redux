using System;

namespace Minecraft_Server
{
    public class CmdPermissionBuild : Command
    {
        public override string name { get { return "perbuild"; } }
        public CmdPermissionBuild() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number > 2 || number < 1) { Help(p); return; }
            if (number == 1)
            {
                LevelPermission Perm = Level.PermissionFromName(message);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }
                p.level.permissionbuild = Perm;
                Server.Log(p.level.name + " build permission changed to " + message + ".");
                Player.GlobalMessageLevel(p.level, "build permission changed to " + message + ".");
            }
            else
            {
                int pos = message.IndexOf(' ');
                string t = message.Substring(0, pos).ToLower();
                string s = message.Substring(pos + 1).ToLower();
                LevelPermission Perm = Level.PermissionFromName(s);
                if (Perm == LevelPermission.Null) { p.SendMessage("Not a valid rank"); return; }

                foreach (Level level in Server.levels) {
				    if (level.name.ToLower() == t.ToLower()) 
                    {
                        level.permissionbuild = Perm;
                        Server.Log(level.name + " build permission changed to " + s + ".");
                        Player.GlobalMessageLevel(level, "build permission changed to " + s + ".");
                        if (p.level != level) { p.SendMessage("build permission changed to " + s + " on " + level.name + "."); }
                        return;
				    }
			    } 
                p.SendMessage("There is no level \""+s+"\" loaded.");
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/PerBuild <rank> - Sets build permission for a map.");
        }
    }
}