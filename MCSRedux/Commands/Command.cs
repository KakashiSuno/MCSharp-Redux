using System;

namespace Minecraft_Server
{
    public abstract class Command
    {
        public abstract string name { get; }
        public abstract void Use(Player p, string message);
        //public abstract void Use(string message);
        //public abstract void UseCon(string message);
        public abstract void Help(Player p);

        public static CommandList all = new CommandList();
        public static void InitAll()
        {
            all.Add(new CmdAbout());
            all.Add(new CmdAbort());
            all.Add(new CmdBan());
            all.Add(new CmdBanip());
            all.Add(new CmdBanned());
            all.Add(new CmdBannedip());
            all.Add(new CmdBind());
            all.Add(new CmdCuboid());
            all.Add(new CmdColor());
            //all.Add(new CmdDeop());
            all.Add(new CmdDna());
            all.Add(new CmdGoto());
            all.Add(new CmdHide());
            all.Add(new CmdHelp());
            all.Add(new CmdInfo());
            all.Add(new CmdKick());
            all.Add(new CmdKickban());
            all.Add(new CmdLevels());
            all.Add(new CmdLoad());
            all.Add(new CmdMe());
            all.Add(new CmdOp());
            all.Add(new CmdOps());
            all.Add(new CmdPaint());
            all.Add(new CmdPlayers());
            all.Add(new CmdSave());
            all.Add(new CmdSetspawn());
            all.Add(new CmdSpawn());
            all.Add(new CmdSummon());
            all.Add(new CmdTp());
            all.Add(new CmdUnban());
            all.Add(new CmdUnbanip());
            //all.Add(new CmdUndo());
            all.Add(new CmdUnload());
            //all.Add(new CmdWhodid());
            all.Add(new CmdWhois());
            all.Add(new CmdWhowas());
            all.Add(new CmdSolid());
            all.Add(new CmdLava());
            all.Add(new CmdWater());
            all.Add(new CmdSay());
            all.Add(new CmdAfk());
            all.Add(new CmdBuilder());
            all.Add(new CmdResetBot());
            all.Add(new CmdNewLvl());
            //all.Add(new CmdDeleteLvl());
            all.Add(new CmdGuest());
            all.Add(new CmdPhysics());
            all.Add(new CmdMapInfo());
            all.Add(new CmdReplace());
            all.Add(new CmdRules());

            all.Add(new CmdActiveWater());
            all.Add(new CmdActiveLava());

            all.Add(new CmdAdmins());
            all.Add(new CmdOpGlass());

            all.Add(new CmdRestore());

            all.Add(new CmdTime());
            all.Add(new CmdCircle());

            all.Add(new CmdAdvBuilder());

            all.Add(new CmdBotSummon());
            all.Add(new CmdBotAdd());
            all.Add(new CmdBotRemove());

            all.Add(new CmdPermissionBuild());
            all.Add(new CmdPermissionVisit());
        }
    }
}