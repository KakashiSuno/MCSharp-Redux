using System;

namespace Minecraft_Server
{
    public class CmdUndo : Command
    {
        //public override string name { get { return "undo"; } }
        //public CmdUndo() { }
        //public override void Use(Player p, string message)
        //{
        //    if (message == "") { Help(p); return; }
        //    if (Player.Exists(message))
        //    {
        //        Player who = Player.Find(message);
        //        if (who.group == Group.Find("operator"))
        //        {
        //            p.SendMessage("You can't undo actions of an operator!"); return;
        //        } if (who.actions.Count == 0)
        //        {
        //            p.SendMessage("There are no actions to undo!"); return;
        //        } int actions = who.actions.Count;
        //        //who.actions.ForEach(delegate(Edit e) { e.Undo(); });
        //        who.actions.Clear();
        //        Player.GlobalChat(p, p.color + "*" + p.name + "&e undid " + actions + " actions of " + who.color + who.name + "&e.", false);
        //    }
        //    else if (Player.left.ContainsKey(message.ToLower()))
        //    {
        //        LeftPlayer who = Player.left[message.ToLower()];
        //        if (Player.GetGroup(message) == Group.Find("operator"))
        //        {
        //            p.SendMessage("You can't undo actions of an operator!"); return;
        //        } if (who.actions.Count == 0)
        //        {
        //            p.SendMessage("There are no actions to undo!"); return;
        //        } int actions = who.actions.Count;
        //        //who.actions.ForEach(delegate(Edit e) { e.Undo(); });
        //        who.actions.Clear();
        //        Player.GlobalChat(p, p.color + "*" + p.name + "&e undid " + actions + " actions of " +
        //                          Player.GetColor(who.name) + who.name + " &f(offline)&e.", false);
        //    }
        //    else { p.SendMessage("No entry found for \"" + message + "\"."); }
        //}
        //public override void Help(Player p)
        //{
        //    p.SendMessage("/undo <name> - Undoes all actions of a player.");
        //}
    }
}