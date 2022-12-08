using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class OverworldGeneration: AGenerator
    {
        private int wallsNeeded = 4;
        private int randomFill = 52;
        private int smooth = 50;
        public Entity[,] CreateOverworld(int _mapWidth, int _mapHeight)
        {
            mapWidth = _mapWidth; mapHeight = _mapHeight;
            SetAllWalls();

            Entity[,] entities = new Entity[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        if (World.seed.Next(0, 100) < randomFill)
                        { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
                    }
                }
            } for (int z = 0; z < smooth; z++) { SmoothMap(); }

            List<Entity> water = new List<Entity>();
            List<Entity> land = new List<Entity>();
            foreach (Entity tile in World.tiles) { if (tile != null) { switch (tile.GetComponent<Traversable>().terrainType) 
                    { case 1: { land.Add(tile); break; } 
                        case 2: { water.Add(tile); break; } } } }
            DijkstraMaps.CreateMap(water, "Water", 500); DijkstraMaps.CreateMap(land, "Land", 500);
            DijkstraMaps.ReverseMap(DijkstraMaps.maps["Land"], "Land");
            DijkstraMaps.CombineThenCreate(DijkstraMaps.maps["Water"], DijkstraMaps.maps["Land"], "HeightMap");
            foreach (Node node in DijkstraMaps.maps["HeightMap"])
            {
                if (node != null)
                {
                    List<Component> components = new List<Component>();
                    components.Add(new Coordinate(node.x, node.y));
                    components.Add(new Visibility(false, true, true));
                    int random1 = World.seed.Next(6, 9);
                    int random3 = World.seed.Next(3, 4);
                    int random4 = World.seed.Next(-3, -1);
                    if (node.v > random1) { components.Add(new Draw("Gray", "Black", '^')); components.Add(new Description("Mountain", "A tall spiny peak of stone.")); components.Add(new Chunk(mapWidth, mapWidth, "Mountain", 1)); }
                    else if (node.v > 6 && node.v <= random1) { components.Add(new Draw("Brown", "Black", '*')); components.Add(new Description("Hills", "A series of rolling hills.")); components.Add(new Chunk(mapWidth, mapWidth, "Hills", 1)); }
                    else if (node.v > random3 && node.v <= 6) { components.Add(new Draw("Dark_Green", "Black", (char)20)); components.Add(new Description("Forest", "A dense patch of scruffy trees.")); components.Add(new Chunk(mapWidth, mapWidth, "Forest", 1)); }
                    else if (node.v > 1 && node.v <= random3) { components.Add(new Draw("Light_Green", "Black", '`')); components.Add(new Description("Grasslands", "An open field full of life.")); components.Add(new Chunk(mapWidth, mapWidth, "Field", 1)); }
                    else if (node.v == 1) { components.Add(new Draw("Light_Gray", "Dark_Yellow", (char)176)); components.Add(new Description("Beach", "A grainy shore of white sand.")); components.Add(new Chunk(mapWidth, mapWidth, "Shore", 1)); }
                    else if (node.v <= 0 && node.v >= random4) { components.Add(new Draw("Light_Blue", "Blue", (char)247)); components.Add(new Description("Shallow Water", "The light water close to shore.")); components.Add(new Chunk(mapWidth, mapWidth, "Ocean", 1)); }
                    else { components.Add(new Draw("Blue", "Dark_Blue", (char)247)); components.Add(new Description("Ocean Strong", "The most any sane man will travel in the ocean.")); components.Add(new Chunk(mapWidth, mapWidth, "Ocean", 2)); }
                    entities[node.x, node.y] = new Entity(components);
                }
            }

            return entities;
        }
        public override void SetAllWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
            }
        }
        public void SmoothMap(bool land = true)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        int walls = WaterCount(x, y);
                        if (walls > wallsNeeded)
                        { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Black", false, 2); }
                        else if (walls < wallsNeeded && land)
                        { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
                    }
                }
            }
        }
        public static int WaterCount(int sX, int sY)
        {
            int walls = 0;

            for (int x = sX - 1; x <= sX + 1; x++)
            {
                for (int y = sY - 1; y <= sY + 1; y++)
                {
                    if (x != sX || y != sY) { if (CMath.CheckBounds(x, y) && World.tiles[x, y, 0].GetComponent<Traversable>().terrainType == 2) { walls++; } }
                }
            }

            return walls;
        }
        public override void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y) { }
        public override void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y) { }
        public override void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y) { }
    }
}
