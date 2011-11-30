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

using MCSRedux.Entities;
using MCSRedux.Maps;

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
		
		//public static List<Player> players = new List<Player>();
		
		static Map world;
		
		public static Map World { get{ return world; }}
		
		public MCSR ()
		{
			log = new Logger();
			
			log.Write("Loading " + Properties.configfile);
			Properties.Load(Properties.configfile);
			
			
			if(!Directory.Exists("Worlds"))
			{
				log.Write("Creating Directory 'Worlds'");
				Directory.CreateDirectory("Worlds");
			}
			
			if(Directory.Exists("Worlds/overworld"))
			{
				log.Write("Loading Map: Overworld");
				world = Map.LoadMap(Directory.GetCurrentDirectory() + "/Worlds/overworld");
			}
			else
			{
				log.Write("Creating Map: Overworld");
				Directory.CreateDirectory("Worlds/overworld");
				world = Map.Create(Directory.GetCurrentDirectory() + "/Worlds/overworld", "Overworld");
				MapGenerator.GenerateFlat(world);
			}
		}
	}
}

