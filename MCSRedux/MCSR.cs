//  
//  MCSR.cs
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
using System.IO;
using System.Collections.Generic;

using MCSRedux.Configuration;
using MCSRedux.Entities;
using MCSRedux.Maps;

using MCSRedux.Network;

namespace MCSRedux
{
	public class MCSR
	{
		public static void Main(string[] args)
		{
			new MCSR();
		}
		
		public static bool isRunning = true;
		public static Logger log;
		
		public static List<SvPlayer> players = new List<SvPlayer>();
		
		static Map world;
		
		public static Map World { get{ return world; }}
		
		public MCSR ()
		{
			Properties.Load(Properties.configfile);
			
			string logfilename = DateTime.Today.ToShortDateString().Replace("/", "-") + ".log";
			log = new Logger(logfilename, "error.log");
			
			log.Write(LogType.Message, "Initializing Server");
			Server sv = new Server(Properties.serverip, Properties.serverport);
			sv.Listen();
			
			while(isRunning) { System.Threading.Thread.Sleep(100); }
		}
	}
}

