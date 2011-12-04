//  
//  Generator.cs
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
	public static class MapGenerator
	{
		public static int seed;
		
		static int xmin = -20;
		static int xmax = 20;
		static int zmin = -20;
		static int zmax = 20;
		
		public static void GenerateFlat(Map map)
		{
			// create chunks at chunk coords xmin,zmin to xmax,zmax
			for(int xi = xmin; xi < xmax; xi++)
			{
				for(int zi = zmin; zi < zmax; zi++)
				{
					ChunkRef chunk = map.ChunkManager.CreateChunk(xi, zi);
					chunk.IsTerrainPopulated = true;
					chunk.Blocks.AutoLight = false;
					
					FlatChunk(chunk, 64);
					
					chunk.Blocks.RebuildBlockLight();
					chunk.Blocks.RebuildSkyLight();
					
					// Debug line
					MCSR.log.Write(LogType.Message, String.Format("Build Chunk {0},{1}", chunk.X, chunk.Z));
					
					map.ChunkManager.Save();
					map.Save();
				}
			}
		}
		
		static void FlatChunk(ChunkRef chunk, int height)
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
	}
}

