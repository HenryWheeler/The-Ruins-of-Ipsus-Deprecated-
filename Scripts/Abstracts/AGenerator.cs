using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheRuinsOfIpsus
{
    public abstract class AGenerator
    {
        public int mapWidth;
        public int mapHeight;
        public int startX;
        public int startY;
        public int startZ;
        public abstract void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y);
        public abstract void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void SetAllWalls();
        public void CreateSurroundingWalls(int depth)
        {
            int h = startY * mapHeight - 1;
            int w = startX * mapWidth - 1;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == h && x == w) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == 0 && x == 0) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (x == w && y == 0) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == 0 || y == h) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (x == 0 || x == w) { SetTile(x, y, depth, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                }
            }
        }
        public void CreateConnections(int loopCount, int type)
        {
            bool doneConnecting = false;

            while (!doneConnecting)
            {
                Coordinate firstCoordinate = null;
                Coordinate lastCoordinate = null;
                List<Tile> cavernTiles = new List<Tile>();

                foreach (Tile tile in Map.map) { if (tile != null && tile.moveType != 0) { firstCoordinate = tile.GetComponent<Coordinate>(); break; } }

                DijkstraMaps.CreateMap(Map.map[firstCoordinate.vector3.x, firstCoordinate.vector3.y], "CurrentRoom");
                foreach (Node node in DijkstraMaps.maps["CurrentRoom"]) { if (node != null && node.v != 1000) { cavernTiles.Add(Map.map[node.x, node.y]); } }
                for (int v = 0; v < loopCount; v++)
                {
                    foreach (Tile tile in Map.map)
                    {
                        if (tile != null && tile.moveType != 0 && !cavernTiles.Contains(tile))
                        {
                            Coordinate coordinate = tile.GetComponent<Coordinate>();
                            if (lastCoordinate == null) { lastCoordinate = tile.GetComponent<Coordinate>(); }
                            else { if (CMath.Distance(coordinate.vector3.x, coordinate.vector3.y, firstCoordinate.vector3.x, firstCoordinate.vector3.y) 
                                    < CMath.Distance(lastCoordinate.vector3.x, lastCoordinate.vector3.y, firstCoordinate.vector3.x, firstCoordinate.vector3.y)) { lastCoordinate = tile.GetComponent<Coordinate>(); } }
                        }
                    }
                    foreach (Tile tile in cavernTiles)
                    {
                        if (tile != null && tile.moveType != 0)
                        {
                            Coordinate coordinate = tile.GetComponent<Coordinate>();
                            if (lastCoordinate != null && CMath.Distance(coordinate.vector3.x, coordinate.vector3.y, firstCoordinate.vector3.x, firstCoordinate.vector3.y) 
                                < CMath.Distance(lastCoordinate.vector3.x, lastCoordinate.vector3.y, firstCoordinate.vector3.x, firstCoordinate.vector3.y)) { firstCoordinate = tile.GetComponent<Coordinate>(); }
                        }
                    }
                }
                if (lastCoordinate == null) { doneConnecting = true; }
                else
                {
                    switch (type)
                    {
                        case 0: CreateStraightPassage(firstCoordinate.vector3.x, firstCoordinate.vector3.y, lastCoordinate.vector3.x, lastCoordinate.vector3.y); break;
                        case 1: CreateDiagonalPassage(firstCoordinate.vector3.x, firstCoordinate.vector3.y, lastCoordinate.vector3.x, lastCoordinate.vector3.y); break;
                        case 2: CreateBezierCurve(firstCoordinate.vector3.x, firstCoordinate.vector3.y, lastCoordinate.vector3.x, lastCoordinate.vector3.y); break;
                    }
                }
            }
        }
        public void FillChunk(string table, int amount)
        { 
            EntityManager.FillChunk(table, amount);
        }
        public bool CheckIfHasSpace(int sX, int sY, int eX, int eY)
        {
            for (int y = sY - 1; y <= eY + 1; y++)
            {
                for (int x = sX - 1; x <= eX + 1; x++)
                {
                    if (x < 1 || y <= 1 || x >= mapWidth || y >= mapHeight) return false;
                    if (Map.map[x, y].moveType != 0) return false;
                }
            }
            return true;
        }
        public void SetTile(int x, int y, int z, char character, string name, string description, string fColor, string bColor, bool opaque, int moveType)
        {
            World.tiles[x, y, 0] = new Entity(new List<Component>() 
            { new Coordinate(x, y, z), new Draw(fColor, bColor, character), new Description(name, description), 
                new Visibility(opaque, false, false), new Traversable(moveType)});
        }
    }
}
