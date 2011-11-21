//  
//  Logger.cs
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
using System.Threading;

namespace MCSRedux
{
	public class Logger
	{
		public string[] queue = new string[Console.WindowHeight];
		bool newmsg = false;
		
		Thread writeThread;
		
		public Logger ()
		{
			for(int i=0; i<queue.Length; i++)
				queue[i] = "";
			
			writeThread = new Thread(log_update);
			writeThread.Start();
		}
		
		void log_update()
		{
			while(MCSR.isRunning)
			{
				if(newmsg)
				{
					Console.Clear();
					foreach(string msg in queue)
						Console.WriteLine(msg);
					newmsg = false;
				}
				Thread.Sleep(50);
			}
		}
		
		public void Write(string msg)
		{
			string tmp = DateTime.Now.ToString("[HH:MM:ss] ") + msg;
			queue[GetNextAvailable()] = tmp;
			newmsg = true;
		}
		
		int GetNextAvailable()
		{
			for(int i=0; i<queue.Length; i++)
			{
				if(queue[i].Length == 0) return i;
				if(i == queue.Length - 1)
				{
					CycleQueue();
					return i;
				}
			}
			return -1;
		}
		void CycleQueue()
		{
			string tmp = "";
			
			for(int i=0; i<queue.Length; i++)
			{
				tmp = queue[i];
				if(i == queue.Length - 1)
					queue[i] = "";
				else
					queue[i] = queue[i+1];
			}
		}
	}
}

