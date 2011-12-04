//  
//  Server.cs
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
using System.Net;
using System.Net.Sockets;
using System.Threading;

using MCSRedux.Configuration;
using MCSRedux.Entities;
using MCSRedux.Network.Packets;

namespace MCSRedux.Network
{
	public class Server
	{
		TcpListener listen;
		IPEndPoint endpoint;
		
		Thread listenThread;
		
		public Server(string ip, int port)
		{
			IPAddress tmp;
			if(!IPAddress.TryParse(ip, out tmp))
				tmp = IPAddress.Any;
			
			endpoint = new IPEndPoint(tmp, port);
			
			listen = new TcpListener(endpoint);
		}
		
		public void Listen()
		{
			listenThread = new Thread(_listen);
			listenThread.Start();
		}
		
		void _listen()
		{
			listen.Start();
			
			while(MCSR.isRunning)
			{
				if(listen.Pending())
				{
					TcpClient cl = listen.AcceptTcpClient();
					if(cl != null)
					{
						byte firstpacket = TypeHandler.ReadByte(cl.GetStream());
						if(firstpacket == (byte)PacketID.Handshake)
							new SvPlayer(cl);
						else if(firstpacket == (byte)PacketID.ServerListPing)
						{
							string tmpstr = Properties.motd + "§" + 0 + "§" + Properties.maxplayers;
							
							byte[] buffer, tmp;
							
							tmp = TypeHandler.GetBytes(tmpstr);
							buffer = new byte[tmp.Length + 3];
							
							buffer[0] = (byte)PacketID.Kick;
							Array.Copy(TypeHandler.GetBytes((short)tmpstr.Length), 0, buffer, 1, 2);
							Array.Copy(tmp, 0, buffer, 3, tmp.Length);
							
							cl.GetStream().Write(buffer, 0, buffer.Length);
							cl.Close();
						}
					}
				}
				Thread.Sleep(100);
			}
		}
	}
}

