using System;
using System.Collections.Generic;

namespace Minecraft_Server
{
    public abstract class Group
    {
        public abstract string name { get; }
        public abstract string color { get; }
        public abstract bool canChat { get; }
        //public abstract bool canBuild { get; }
        public abstract LevelPermission Permission { get; }
        public abstract CommandList commands { get; }
        public bool CanExecute(Command cmd) { return commands.Contains(cmd); }

        public static List<Group> GroupList = new List<Group>();
        public static Group standard;
        public static void InitAll()
        {
            GroupList.Add(new GrpBanned());
            GroupList.Add(new GrpGuest());
            GroupList.Add(new GrpOperator());
            GroupList.Add(new GrpBuilder());
            GroupList.Add(new GrpAdvBuilder());
            GroupList.Add(new GrpSuperOp());
            GroupList.Add(new GrpBot());
            standard = GroupList[1];
        }
        public static bool Exists(string name)
        {
            name = name.ToLower(); foreach (Group gr in GroupList)
            {
                if (gr.name == name.ToLower()) { return true; }
            } return false;
        }
        public static Group Find(string name)
        {
            name = name.ToLower(); foreach (Group gr in GroupList)
            {
                if (gr.name == name.ToLower()) { return gr; }
            } return null;
        }
    }
}