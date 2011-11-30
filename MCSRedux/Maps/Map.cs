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

namespace MCSRedux.Maps
{
	public class Map
	{
		BetaWorld world;
		BetaChunkManager cm;
		
		int xmin = -20;
		int xmax = 20;
		int zmin = -20;
		int zmax = 20;
		
		public string Name { get{ return world.Level.LevelName; }}
		public string Path { get{ return world.Path; }}
		public SpawnPoint Spawn { get{ return world.Level.Spawn; }}
		
		public void GenerateFlat()
		{
			// create chunks at chunk coords xmin,zmin to xmax,zmax
			for(int xi = xmin; xi < xmax; xi++)
			{
				for(int zi = zmin; zi < zmax; zi++)
				{
					ChunkRef chunk = cm.CreateChunk(xi, zi);
					chunk.IsTerrainPopulated = true;
					chunk.Blocks.AutoLight = false;
					
					FlatChunk(chunk, 64);
					
					chunk.Blocks.RebuildBlockLight();
					chunk.Blocks.RebuildSkyLight();
					
					// Debug line
					MCSR.log.Write(String.Format("Build Chunk {0},{1}", chunk.X, chunk.Z));
					
					cm.Save();
					world.Save();
				}
			}
		}
		
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
		
		#region Non-public methods
		void FlatChunk(ChunkRef chunk, int height)
		{
			// create bedrock
			for(int y=0; y<2; y++){
				for(int x=0; x<16; x++){
					for(int z=0; z<16; z++){
						chunk.Blocks.SetID(x,y,z, (int)BlockType.BEDROCK);
					}
				}
			}
			
			//create stone
			for(int y=2; y<height-5; y++){
				for(int x=0; x<16; x++){
					for(int z=0; z<16; z++){
						chunk.Blocks.SetID(x,y,z, (int)BlockType.STONE);
					}
				}
			}
			
			// create dirt
			for(int y=height-5; y<height-1; y++){
				for(int x=0; x<16; x++){
					for(int z=0; z<16; z++){
						chunk.Blocks.SetID(x,y,z, (int)BlockType.DIRT);
					}
				}
			}
			
			// create grass
			for(int y=height-1; y<height; y++){
				for(int x=0; x<16; x++){
					for(int z=0; z<16; z++){
						chunk.Blocks.SetID(x,y,z, (int)BlockType.GRASS);
					}
				}
			}
		} // END FlatChunk()
		#endregion
	}
}

