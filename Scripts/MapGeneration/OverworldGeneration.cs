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
        public void CreateOverworld(int _mapWidth, int _mapHeight)
        {
            mapWidth = _mapWidth; mapHeight = _mapHeight;
            Map.outside = true;
            SetAllWalls();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        if (CMath.seed.Next(0, 100) < randomFill)
                        { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
                    }
                }
            } for (int z = 0; z < smooth; z++) { SmoothMap(); }

            List<Entity> water = new List<Entity>();
            List<Entity> land = new List<Entity>();
            foreach (Tile tile in Map.map) { if (tile != null) { switch (tile.moveType) 
                    { case 1: { land.Add(tile); break; } 
                        case 2: { water.Add(tile); break; } } } }
            DijkstraMaps.CreateMap(water, "Water", 500); DijkstraMaps.CreateMap(land, "Land", 500);
            DijkstraMaps.ReverseMap(DijkstraMaps.maps["Land"], "Land");
            DijkstraMaps.CombineThenCreate(DijkstraMaps.maps["Water"], DijkstraMaps.maps["Land"], "HeightMap");
            foreach (Node node in DijkstraMaps.maps["HeightMap"])
            {
                if (node != null)
                {
                    int random1 = CMath.seed.Next(6, 9);
                    int random3 = CMath.seed.Next(3, 4);
                    int random4 = CMath.seed.Next(-3, -1);
                    if (node.v > random1) { SetTile(node.x, node.y, '^', "Mountain", "A tall spiny peak of stone.", "Gray", "Black", false, 1); }
                    else if (node.v > 6 && node.v <= random1) { SetTile(node.x, node.y, '*', "Hills", "A series of rolling hills.", "Brown", "Black", true, 0); }
                    else if (node.v > random3 && node.v <= 6) { SetTile(node.x, node.y, (char)20, "Forest", "A dense patch of scruffy trees.", "Dark_Green", "Black", true, 0); }
                    else if (node.v > 1 && node.v <= random3) { SetTile(node.x, node.y, '`', "Grasslands", "An open field full of life.", "Light_Green", "Black", false, 1); }
                    else if (node.v == 1) { SetTile(node.x, node.y, (char)176, "Beach", "A grainy shore of white sand.", "Light_Gray", "Dark_Yellow", false, 1); }
                    else if (node.v <= 0 && node.v >= random4) { SetTile(node.x, node.y, (char)247, "Shallow Water", "The light water close to shore.", "Light_Blue", "Blue", false, 2); }
                    else { SetTile(node.x, node.y, (char)247, "Ocean Strong", "The most any sane man will travel in the ocean.", "Blue", "Dark_Blue", false, 2); }
                }
            }
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
                    if (x != sX || y != sY) { if (CMath.CheckBounds(x, y) && Map.map[x, y].moveType == 2) { walls++; } }
                }
            }

            return walls;
        }
        public override void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y) { }
        public override void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y) { }
        public override void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y) { }
    }
}
