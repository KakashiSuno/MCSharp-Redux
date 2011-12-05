//  
//  McClient.cs
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
using System.Net;
using System.Net.Sockets;

using MCSRedux.Network;
using MCSRedux.Network.Packets;
using MCSRedux.Configuration;

namespace MCSRedux.Entities
{
	public class McClient
	{
		TcpClient client;
		public string ip;
		
		public bool Connected { get{ return client.Connected; }}
		
		public McClient (TcpClient cl)
		{
			client = cl;
			ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
			
			PacketID firstid = (PacketID)TypeHandler.ReadByte(client.GetStream());
			
			if(firstid == PacketID.ServerListPing)
			{
				string tmpstr = Properties.motd + "§" + 0 + "§" + Properties.maxplayers;
				short length = (short)tmpstr.Length;
				
				byte[] buffer = new byte[length * 2 + 3]; // 3 bytes, plus length of string (in bytes)
				
				buffer[0] = (byte)PacketID.Kick;
				Array.Copy(TypeHandler.GetBytes(length), 0, buffer, 1, 2);
				Array.Copy(TypeHandler.GetBytes(tmpstr), 0, buffer, 3, length * 2);
				
				SendRaw(buffer);
			}
			else if(firstid == PacketID.Handshake)
			{
				new SvPlayer(this);
			}
		}
		
		public void Disconnect()
		{
			client.Close();
		}
		
		public Stream GetStream()
		{
			return client.GetStream();
		}
		
		public void SendRaw(byte[] buffer)
		{
			client.GetStream().Write(buffer, 0, buffer.Length);
		}
	}
}

