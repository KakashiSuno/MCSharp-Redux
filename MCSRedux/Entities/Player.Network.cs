//  
//  Player.Network.cs
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

using MCSRedux.Network;
using MCSRedux.Network.Packets;
using MCSRedux.Configuration;

namespace MCSRedux.Entities
{
	public partial class SvPlayer
	{		
		void Receive()
		{
			while(client.Connected && MCSR.isRunning)
			{
				switch((PacketID)TypeHandler.ReadByte(client.GetStream()))
				{
				case PacketID.LoginRequest:
					HandleLogin();
					break;
				}
				
				Thread.Sleep(50);
			}
		}
		
		void HandleLogin()
		{
			if((byte)TypeHandler.ReadInt(client.GetStream()) != Properties.version)
			{
				Kick("Wrong protocol version! Update your client!");
				return;
			}
			
			string nameauth = TypeHandler.ReadString(client.GetStream());
			if(nameauth != Name)
				Kick("Last I checked, you only had one name!");
			
			MCSR.players.Add(this);
			id = GetNextID();
			
			byte[] buffer = new byte[23];
			
			buffer[0] = (byte)PacketID.LoginRequest;
			Array.Copy(TypeHandler.GetBytes(Properties.version), 0, buffer, 1, 4);
			Array.Copy(TypeHandler.GetBytes((short)0), 0, buffer, 5, 2);
			Array.Copy(TypeHandler.GetBytes(MCSR.World.seed), 0, buffer, 7, 8);
			Array.Copy(TypeHandler.GetBytes(0), 0, buffer, 15, 4);
			buffer[19] = 0;
			buffer[20] = 0;
			buffer[21] = 128;
			buffer[22] = (byte)Properties.maxplayers;
			
			client.SendRaw(buffer);
		}
		
		// Temporary method to assign players IDs while I work out an entity spawning system
		int GetNextID()
		{
			int i = 0;
			foreach(SvPlayer p in MCSR.players)
			{
				if(p.id == i)
					i++;
			}
			return i;
		}
	}
}

