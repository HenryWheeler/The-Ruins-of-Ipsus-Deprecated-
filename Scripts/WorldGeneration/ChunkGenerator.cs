using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class ChunkGenerator
    {
        public static void CreateChunk(Chunk chunk, int x, int y, int z)
        {
            switch (chunk.environment)
            {
                case "Dungeon": DungeonGenerator generator0 = new DungeonGenerator(); generator0.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Cave": CaveGenerator generator1 = new CaveGenerator(); generator1.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Field": FieldGenerator generator2 = new FieldGenerator(); generator2.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Forest": FieldGenerator generator3 = new FieldGenerator(); generator3.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Hills": FieldGenerator generator4 = new FieldGenerator(); generator4.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Mountain": FieldGenerator generator5 = new FieldGenerator(); generator5.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Island": IslandGenerator generator6 = new IslandGenerator(); generator6.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
                case "Ocean": IslandGenerator generator7 = new IslandGenerator(); generator7.CreateMap(x, y, z, chunk.chunkWidth, chunk.chunkHeight, chunk.strength); break;
            }
        }
    }
}
