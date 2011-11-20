using System;

namespace Minecraft_Server
{
    public class CmdColor : Command
    {
        public override string name { get { return "color"; } }
        public CmdColor() { }
        public override void Use(Player p, string message)
        {
            if (message == "") { Help(p); return; }
            if (message.Split(' ').Length > 2) { Help(p); return; }
            int pos = message.IndexOf(' ');
            if (pos != -1)
            {
                Player who = Player.Find(message.Substring(0, pos));
                if (who == null) { p.SendMessage("There is no player \"" + message.Substring(0, pos) + "\"!"); return; }
                string color = c.Parse(message.Substring(pos + 1));
                if (color == "") { p.SendMessage("There is no color \"" + message + "\"."); }
                else if (color == who.color) { p.SendMessage(who.name + " already has that color."); }
                else
                {
                    //Player.GlobalChat(who, p.color + "*" + p.name + "&e changed " + who.color + Name(who.name) +
                    //                  " color to " + color +
                    //                  c.Name(color) + "&e.", false);
                    Player.GlobalChat(who, who.color + "*" + Name(who.name) +
                  " color changed to " + color +
                  c.Name(color) + "&e.", false);
                    who.color = color; 
					
					Player.GlobalDie(who, false);
                    Player.GlobalSpawn(who, who.pos[0], who.pos[1], who.pos[2], who.rot[0], who.rot[1], false);
                }
            }
            else
            {
                string color = c.Parse(message);
                if (color == "") { p.SendMessage("There is no color \"" + message + "\"."); }
                else if (color == p.color) { p.SendMessage("You already have that color."); }
                else
                {
                    Player.GlobalChat(p, p.color + "*" + Name(p.name) +
                                      " color changed to " + color +
                                      c.Name(color) + "&e.", false);
                    p.color = color; Player.GlobalDie(p, false);
                    Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                }
            }
        }
        public override void Help(Player p)
        {
            p.SendMessage("/color [player] <color> - Changes the nick color.");
            p.SendChat(p, "&0black &1navy &2green &3teal &4maroon &5purple &6gold &7silver");
            p.SendChat(p, "&8gray &9blue &alime &baqua &cred &dpink &eyellow &fwhite");
        }
        static string Name(string name)
        {
            string ch = name[name.Length - 1].ToString().ToLower();
            if (ch == "s" || ch == "x") { return name + "&e'"; }
            else { return name + "&e's"; }
        }
    }
}