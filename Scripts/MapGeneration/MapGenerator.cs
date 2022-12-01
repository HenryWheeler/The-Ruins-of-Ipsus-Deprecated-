using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class MapGenerator
    {
        private static int mapWidth;
        private static int mapHeight;
        public static void CreateMap(int _mapWidth, int _mapHeight)
        {
            Map map = new Map(_mapWidth, _mapHeight);

            mapWidth = _mapWidth;
            mapHeight = _mapHeight;

            int type = CMath.seed.Next(0, 5);
            switch (type)
            {
                case 0: DungeonGenerator generator0 = new DungeonGenerator(); generator0.CreateMap(mapWidth, mapHeight); break;
                case 1: CaveGenerator generator1 = new CaveGenerator(); generator1.CreateMap(mapWidth, mapHeight); break;
                case 2: FieldGenerator generator2 = new FieldGenerator(); generator2.CreateMap(mapWidth, mapHeight); break;
                case 3: IslandGenerator generator3 = new IslandGenerator(); generator3.CreateMap(mapWidth, mapHeight); break;
                case 4: OverworldGeneration generator4 = new OverworldGeneration(); generator4.CreateOverworld(mapWidth, mapHeight); break;
            }
            DijkstraMaps.DiscardAll();
        }
    }
}
