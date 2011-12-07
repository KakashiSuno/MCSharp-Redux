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
using System.IO;
using System.Threading;

using MCSRedux.Network;
using MCSRedux.Network.Packets;
using MCSRedux.Configuration;
using MCSRedux.Maps;

using Ionic.Zlib;

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
		
		#region Maps/Chunks
		public void SendMap(Map m)
		{
			int x, z;
			x = 20;
			z = 20;
			PreChunk(x, z, false);
			MapChunk(m);
			SendSpawn(m.Spawn.X, m.Spawn.Y, m.Spawn.Z);
			PositionLook(m.Spawn.X, m.Spawn.Y, m.Spawn.Z, 0, 0, true);
		}
		void PreChunk(int x, int z, bool unload)
		{
			byte[] buffer = new byte[10];
			buffer[0] = (byte)PacketID.PreChunk;
			Array.Copy(TypeHandler.GetBytes(x), 0, buffer, 1, 4);
			Array.Copy(TypeHandler.GetBytes(z), 0, buffer, 5, 4);
			buffer[9] = (byte)((unload) ? 0 : 1);
			client.SendRaw(buffer);
		}
		void MapChunk(Map m)
		{
			// TODO: investigate how Substrate uses Zlib to compress chunks to files
			
			byte[] chunkdata = m.GetCompressedData(m.Spawn.X, m.Spawn.Y, m.Spawn.Z, 15, 127, 15);
			byte[] buffer = new byte[18 + chunkdata.Length];
			
			buffer[0] = (byte)PacketID.MapChunk;
			Array.Copy(TypeHandler.GetBytes(m.Spawn.X), 0, buffer, 1, 4);
			Array.Copy(TypeHandler.GetBytes(m.Spawn.Y), 0, buffer, 5, 4);
			buffer[9] = 15;
			buffer[10] = 127;
			buffer[11] = 15;
			Array.Copy(TypeHandler.GetBytes(chunkdata.Length), 0, buffer, 12, 4);
			Array.Copy(chunkdata, 0, buffer, 15, chunkdata.Length);
			
			// Pray this works
			client.SendRaw(buffer);
		}
		#endregion
		#region Position and Spawning
		public void SendSpawn(int x, int y, int z)
		{
			byte[] buffer = new byte[13];
			buffer[0] = (byte)PacketID.SpawnPosition;
			Array.Copy(TypeHandler.GetBytes(x), 0, buffer, 1, 4);
			Array.Copy(TypeHandler.GetBytes(y), 0, buffer, 5, 4);
			Array.Copy(TypeHandler.GetBytes(z), 0, buffer, 9, 4);
			
			client.SendRaw(buffer);
		}
		public void PositionLook(double x, double y, double z, float yaw, float pitch, bool onground)
		{
			byte[] buffer = new byte[42];
			
			buffer[0] = (byte)PacketID.PlayerPositionLook;
			Array.Copy(TypeHandler.GetBytes(x), 0, buffer, 1, 8);		// X
			Array.Copy(TypeHandler.GetBytes(y), 0, buffer, 9, 8);		// Stance = Y (confirm)
			Array.Copy(TypeHandler.GetBytes(y), 0, buffer, 17, 8);		// Y
			Array.Copy(TypeHandler.GetBytes(z), 0, buffer, 25, 8);		// Z
			Array.Copy(TypeHandler.GetBytes(yaw), 0, buffer, 33, 4);	// Yaw
			Array.Copy(TypeHandler.GetBytes(pitch), 0, buffer, 37, 4);	// Pitch
			buffer[41] = (byte)((onground) ? 1 : 0);
			
			client.SendRaw(buffer);
		}
		#endregion
		
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

