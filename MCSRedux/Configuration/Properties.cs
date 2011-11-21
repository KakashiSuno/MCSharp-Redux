//  
//  Properties.cs
//  
//  Author:
//       MCSR Team <day7tech@gmail.com>
// 
//  Copyright (c) 2011 MCSR Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MCSRedux
{
    public static class Properties
    {
        // MCSR Properties 
        public const byte version = 22;
        public static string configfile = "server.properties";
        public static string errorlog = "server.log";
        public static int loglevel = 3;
        public static bool gui = false;

        // General Properties
        public static string motd = "A Minecraft Server";
        public static bool allowflight = false;
        public static bool whitelist = false;
        public static bool pvp = true;
        public static byte difficulty = 1;
        public static byte gamemode = 0;
        public static short maxplayers = 20;
        public static byte viewdistance = 10;

        // Network Properties
        public static string serverip = "";
        public static ushort serverport = 25565;
        public static bool enablercon = false;
        public static bool enablequery = false;
        public static bool onlinemode = true;

        // World Properties
        public static string levelname = "world";
        public static string levelseed = "";
        public static bool spawnanimals = false;
        public static bool spawnmonsters = false;
        public static bool allownether = true;

        public static void generate()
        {
            StreamWriter config = new StreamWriter(File.Create(configfile));

            config.WriteLine("# MCSR Default Configuration File");
            config.WriteLine();
            config.WriteLine("# General Settings");
            config.WriteLine("allow-flight=" + allowflight.ToString().ToLower());
            config.WriteLine("difficulty=" + difficulty);
            config.WriteLine("gamemode=" + gamemode);
            config.WriteLine("max-players=" + maxplayers);
            config.WriteLine("motd=" + motd);
            config.WriteLine("pvp=" + pvp.ToString().ToLower());
            config.WriteLine("view-distance=" + viewdistance);
            config.WriteLine("white-list=" + whitelist.ToString().ToLower());
            config.WriteLine();
            config.WriteLine("# Network Settings");
            config.WriteLine("enable-query=" + enablequery.ToString().ToLower());
            config.WriteLine("enable-rcon=" + enablercon.ToString().ToLower());
            config.WriteLine("online-mode=" + onlinemode.ToString().ToLower());
            config.WriteLine("server-ip=" + serverip);
            config.WriteLine("server-port=" + serverport);
            config.WriteLine();
            config.WriteLine("# World Settings");
            config.WriteLine("allow-nether=" + allownether.ToString().ToLower());
            config.WriteLine("level-name=" + levelname);
            config.WriteLine("level-seed=" + levelseed);
            config.WriteLine("spawn-animals=" + spawnanimals.ToString().ToLower());
            config.WriteLine("spawn-monsters=" + spawnmonsters.ToString().ToLower());           

            config.Close();
        }
    }
}
