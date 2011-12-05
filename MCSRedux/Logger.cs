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
using System.Collections.Generic;
using System.IO;
using System.Text;

using MCSRedux.Configuration;

namespace MCSRedux
{
	// Class made so that I don't have to mess around with too many array calcs
	public class OverflowBuffer
	{
		string[] array;
		public OverflowBuffer(int len)
		{
			array = new string[len];
		}
		public void Add(LogType type, string str)
		{
			int nextindex = GetNext();
			if(nextindex == -1) {
				array = new string[array.Length]; // I'll be amazed if this works
				nextindex = 0;
			}
			array[nextindex] = "&" + type + str;
		}
		int GetNext()
		{
			for(int i=0; i< array.Length; i++) {
				if(array[i] == String.Empty)
					return i;
			}
			return -1;
		}
	}
	public enum LogType
	{
		Message = 0,
		Warning = 1,
		Error = 2
	}
	public class Logger
	{
		OverflowBuffer overflow; // If a message fails to write, it's stored here
		
		FileStream messageLog;	// Everything you see in the console will go through here
		FileStream errorLog;	// Everything else goes through here
		
		const string logdir = "Logs";
		const byte obufflen = 5;
		
		public Logger(string mlog, string elog)
		{
			overflow = new OverflowBuffer(obufflen); // 5 might be too low, need to experiment
			
			if(!Directory.Exists(logdir))
				Directory.CreateDirectory(logdir);
			
			if(!File.Exists(logdir + "/" + mlog))
				File.Create(logdir + "/" + mlog);
			if(!File.Exists(elog))
				File.Create(elog);
			
			messageLog = File.Open(logdir + "/" + mlog, FileMode.Append, FileAccess.Write); 
			errorLog = File.Open(elog, FileMode.Append, FileAccess.Write);
		}
		
		/// <summary>
		/// Writes to the log as specified by 'type'
		/// </summary>
		/// <param name='type'>
		/// Determines how to handle the message
		/// </param>
		/// <param name='message'>
		/// The message to pass to the logger
		/// </param>
		public void Write(LogType type, string message)
		{
			switch(type) {
			case LogType.Message:
				WriteMsg(message, true);
				break;
			case LogType.Warning:
				WriteWerror(message, true);
				break;
			}
		}
		
		void WriteMsg(string message, bool timestamp)
		{
			string tmp = ((timestamp) ? DateTime.Now.ToString("[HH:MM:ss] ") : "") + message;
			try {
				byte[] btmp = Encoding.ASCII.GetBytes(tmp);
				messageLog.Write(btmp, 0, btmp.Length);
			}
			catch(IOException ex) {
				overflow.Add(LogType.Message, tmp);
				Error(ex.ToString());
			}
			
			Console.WriteLine(tmp);
		}
		void WriteWerror(string message, bool timestamp)
		{
			string tmp = ((timestamp) ? DateTime.Now.ToString("[HH:MM:ss] ") : "") + "WARNING: " + message;
			try {
				byte[] btmp = Encoding.ASCII.GetBytes(tmp);
				messageLog.Write(btmp, 0, btmp.Length);
			}
			catch(IOException ex) {
				overflow.Add(MCSRedux.LogType.Warning, message);
				Error(ex.ToString());
			}
			
			Console.WriteLine(tmp);
		}
		
		void Error(string msg)
		{
			string logannounce = "ERROR at time " + DateTime.Now.ToString("HH:MM:ss") + ". See " + Properties.errorlog + " for details";
			
			try{
				byte[] btmp = Encoding.ASCII.GetBytes(logannounce);
				messageLog.Write(btmp, 0, btmp.Length);
				
				btmp = Encoding.ASCII.GetBytes(msg);
				errorLog.Write(btmp, 0, btmp.Length);
			}
			catch(Exception ex) { Console.WriteLine("Errorception"); }
		}
		
		
	}
}

