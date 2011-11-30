//  
//  Packet.cs
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

namespace MCSRedux.Packets
{
	public abstract class Packet
	{
		public byte ID;
		public object[] payload;
		
		public static Packet ParseIncoming(byte[] data)
		{
			switch((PacketID)data[0])
			{
			case PacketID.LoginRequest:
				General.LoginRequest tmp = new General.LoginRequest();
				Array.Copy(data, 1, tmp.payload, 0, data.Length - 1);
				break;
			}
			return null;
		}
	}
}

