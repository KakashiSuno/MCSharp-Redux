using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using LuaInterface;

namespace Minecraft_Server
{
    public class LuaScripts
    {
        static Lua lua = new Lua();

        public void Load(string sourcepath)
        {
            lua.RegisterFunction("GlobalMessage", this, this.GetType().GetMethod("Player.GlobalMessage"));
            lua.DoFile(sourcepath + "/init.lua");

            foreach (string s in Directory.GetDirectories(sourcepath))
                lua.DoFile(s + "/init.lua");
        }
    }
}
