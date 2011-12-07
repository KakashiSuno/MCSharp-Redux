//  
//  Map.cs
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

using Substrate;
using Substrate.Data;

using Ionic.Zlib;

namespace MCSRedux.Maps
{
	public class Map
	{
		BetaWorld world;
		BetaChunkManager cm;
		PlayerManager pm;
		
		public string Name { get{ return world.Level.LevelName; }}
		public string Path { get{ return world.Path; }}
		public SpawnPoint Spawn { get{ return world.Level.Spawn; }}
		
		//public BetaWorld World { get{ return world; }}
		public BetaChunkManager ChunkManager { get{ return cm; }}
		public PlayerManager PlayerMgr { get{ return pm; }}
		
		public long seed = MCSRedux.Configuration.Properties.levelseed.GetHashCode();
		
		#region Member functions
		public void Save()
		{
			world.Save();
		}
		public byte[] GetCompressedData(int x, int y, int z, int sx, int sy, int sz)
		{
			Chunk spawnchunk = cm.GetChunk(x, z);
			byte[] data = new byte[sx * sy * sz];
			
			for(int lx=x; lx < sx; lx++)
			{
				for(int lz=z; lz < sz; lz++)
				{
					for(int ly=y; ly < sy; ly++)
					{
						// x + z * width + y * width * height;
						data[x + z * sx + y * sz * sz] = (byte)spawnchunk.Blocks.GetBlock(lx, ly, lz).ID;
					}
				}
			}
			
			return DeflateStream.CompressBuffer(data);
		}
		#endregion
		
		#region Static methods
		public static Map LoadMap(string path)
		{
			Map m = new Map();
			
			m.world = BetaWorld.Open(path);
			m.cm = m.world.GetChunkManager();
			
			return m;
		}
		public static Map Create(string path, string name)
		{
			System.IO.Directory.CreateDirectory(path);
			Map m = new Map();
			m.world = BetaWorld.Create(path);
			m.cm = m.world.GetChunkManager();
			
			m.world.Level.LevelName = name;
			m.world.Level.Spawn = new SpawnPoint(20, 20, 70);
			
			return m;
		}
		#endregion
	}
}

