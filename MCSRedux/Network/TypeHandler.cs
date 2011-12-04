//  
//  TypeHandler.cs
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
using System.Text;
using System.IO;

namespace MCSRedux.Network
{
	public static class TypeHandler
	{
		// Credit for most of this class goes to copyboy
		// https://github.com/copyboy/obsidian-forge/
		
		static byte[] Read(Stream stream, int length, bool reverse)
		{
			byte[] tmp = new byte[length];
			
			if(stream.Read(tmp, 0, length) < length)
				throw new EndOfStreamException();
			if(BitConverter.IsLittleEndian && reverse)
				Array.Reverse(tmp);
			return tmp;
		}
		
		#region Read methods
		public static byte ReadByte(Stream stream)
		{
			int b = stream.ReadByte();
			if(b == -1)
				throw new EndOfStreamException();
			return (byte)b;
		}
		public static short ReadShort(Stream stream)
		{
			return BitConverter.ToInt16(Read(stream, 2, true), 0);
		}
		public static int ReadInt(Stream stream)
		{
			return BitConverter.ToInt32(Read(stream, 4, true), 0);
		}
		public static long ReadLong(Stream stream)
		{
			return BitConverter.ToInt64(Read(stream, 8, true), 0);
		}
		public static float ReadFloat(Stream stream)
		{
			return BitConverter.ToSingle(Read(stream, 4, true), 0);
		}
		public static double ReadDouble(Stream stream)
		{
			return BitConverter.ToDouble(Read(stream, 8, true), 0);
		}
		public static string ReadString(Stream stream)
		{
			int length = ReadShort(stream);
			
			if(length < 0 || length > 119)
				MCSR.log.Write(LogType.Error, "Invalid string length: " + length);
			byte[] bytes = Read(stream, length * 2, false);
			return Encoding.BigEndianUnicode.GetString(bytes);
		}
		public static bool ReadBool(Stream stream)
		{
			return (ReadByte(stream) == 1);
		}
		// TODO: Metadata
		#endregion
		
		#region GetBytes
		public static byte[] GetBytes(short s)
		{
			byte[] tmp = BitConverter.GetBytes(s);
			if(BitConverter.IsLittleEndian)
				Array.Reverse(tmp);
			return tmp;
		}
		public static byte[] GetBytes(string s)
		{
			byte[] tmp = Encoding.BigEndianUnicode.GetBytes(s);
			return tmp;
		}
		#endregion
	}
}

