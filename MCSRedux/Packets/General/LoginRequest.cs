//  
//  LoginRequest.cs
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

// PacketID		(byte)
// Username		(string)	[Cient to Server; Max Length of 16]
// Map Seed		(long)		[Server to Client]
// Server Mode	(int)		[Server to Client; 0 for survival, 1 for creative]
// Dimension	(byte) 		[Server to Client; -1 = Nether, 0 = Normal, 1 = End]
// Difficulty	(byte)		[Server to Client; 0-3 for Peaceful, Easy, Normal, Hard]
// World Height	(ubyte)		[Server to Client; Defaults to 128]
// Max Players	(ubyte)		[Server to Client]

// Total Length in bytes: 23 + length of strings

using System;

namespace MCSRedux.Packets.General
{
	public class LoginRequest : Packet
	{
		public LoginRequest(){}
	}
}

