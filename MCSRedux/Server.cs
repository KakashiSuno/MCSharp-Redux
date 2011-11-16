//  
//  Server.cs
//  
//  Author:
//       Caleb Gibbs <cmacgibbs@gmail.com>
// 
//  Copyright (c) 2011 Caleb Gibbs
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

using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace MCSRedux
{
	public class Server
	{
		public static string Version{ get{ return "0.1"; } }
		
		static bool isRunning = true;
		
		static Socket listen;
		static System.Diagnostics.Process process;
		static System.Timers.Timer updateTimer = new System.Timers.Timer(50); // 20 ticks per second
		
		static Thread inputThread;
		static Thread physThread;
		
		// Player Rank lists
		
		// More stuff here...
	}
}

