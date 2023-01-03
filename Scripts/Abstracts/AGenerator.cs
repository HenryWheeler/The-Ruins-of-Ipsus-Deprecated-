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
        public abstract void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y);
        public abstract void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void SetAllWalls();
        public void CreateSurroundingWalls()
        {
            int h = mapHeight - 1;
            int w = mapWidth - 1;
            for (int y = mapHeight; y >= 0; y--)
            {
                for (int x = mapWidth; x >= 0; x--)
                {
                    if (CMath.CheckBounds(x, y))
                    {
                        if (y == h && x == 1) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                        else if (y == h && x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                        else if (y == 1 && x == 1) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                        else if (x == w && y == 1) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                        else if (y == 1 || y == h) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                        else if (x == 1 || x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    }
                }
            }
        }
        public void CreateConnections(int loopCount, int type)
        {
            DijkstraMaps.DiscardAll();
            bool doneConnecting = false;
            for (int v = 0; v < loopCount; v++)
            {

                while (!doneConnecting)
                {
                    Coordinate firstCoordinate = null;
                    Coordinate lastCoordinate = null;
                    List<Entity> cavernTiles = new List<Entity>();

                    foreach (Entity tile in World.tiles) { if (tile != null && tile.GetComponent<Traversable>().terrainType != 0) { firstCoordinate = tile.GetComponent<Coordinate>(); break; } }

                    DijkstraMaps.CreateMap(World.GetTraversable(firstCoordinate.vector2).entity, "CurrentRoom");
                    foreach (Node node in DijkstraMaps.maps["CurrentRoom"]) { if (node != null && node.v != 1000) { cavernTiles.Add(World.tiles[node.x, node.y]); } }
                    foreach (Entity tile in World.tiles)
                    {
                        if (tile != null && tile.GetComponent<Traversable>().terrainType != 0 && !cavernTiles.Contains(tile))
                        {
                            Coordinate coordinate = tile.GetComponent<Coordinate>();
                            if (lastCoordinate == null) { lastCoordinate = tile.GetComponent<Coordinate>(); }
                            else
                            {
                                if (CMath.Distance(coordinate.vector2.x, coordinate.vector2.y, firstCoordinate.vector2.x, firstCoordinate.vector2.y)
                                 < CMath.Distance(lastCoordinate.vector2.x, lastCoordinate.vector2.y, firstCoordinate.vector2.x, firstCoordinate.vector2.y)) { lastCoordinate = tile.GetComponent<Coordinate>(); }
                            }
                        }
                    }
                    foreach (Entity tile in cavernTiles)
                    {
                        if (tile != null && tile.GetComponent<Traversable>().terrainType != 0)
                        {
                            Coordinate coordinate = tile.GetComponent<Coordinate>();
                            if (lastCoordinate != null && CMath.Distance(coordinate.vector2.x, coordinate.vector2.y, firstCoordinate.vector2.x, firstCoordinate.vector2.y)
                                < CMath.Distance(lastCoordinate.vector2.x, lastCoordinate.vector2.y, firstCoordinate.vector2.x, firstCoordinate.vector2.y)) { firstCoordinate = tile.GetComponent<Coordinate>(); }
                        }
                    }
                    if (lastCoordinate == null) { doneConnecting = true; }
                    else
                    {
                        switch (type)
                        {
                            case 0: CreateStraightPassage(firstCoordinate.vector2.x, firstCoordinate.vector2.y, lastCoordinate.vector2.x, lastCoordinate.vector2.y); break;
                            case 1: CreateDiagonalPassage(firstCoordinate.vector2.x, firstCoordinate.vector2.y, lastCoordinate.vector2.x, lastCoordinate.vector2.y); break;
                            case 2: CreateBezierCurve(firstCoordinate.vector2.x, firstCoordinate.vector2.y, lastCoordinate.vector2.x, lastCoordinate.vector2.y); break;
                        }
                    }
                }
            }
        }
        public void CreateStairs()
        {
            List<Entity> tiles = new List<Entity>();
            foreach(Entity tile in World.tiles) { if (tile != null && tile.GetComponent<Traversable>().terrainType == 1) { tiles.Add(tile); } }
            Entity upStair = tiles[World.seed.Next(0, tiles.Count - 1)];
            tiles.Remove(upStair);
            Entity downStair = tiles[World.seed.Next(0, tiles.Count - 1)];
            Vector2 vector2 = upStair.GetComponent<Coordinate>().vector2;
            SetTile(vector2.x, vector2.y, '<', "Stairs Up", "A winding staircase upward.", "White", "Black", false, 1);
            vector2 = downStair.GetComponent<Coordinate>().vector2;
            SetTile(vector2.x, vector2.y, '>', "Stairs Down", "A winding staircase downward.", "White", "Black", false, 1);
        }
        public bool CheckIfHasSpace(int sX, int sY, int eX, int eY)
        {
            for (int y = sY - 1; y <= eY + 1; y++)
            {
                for (int x = sX - 1; x <= eX + 1; x++)
                {
                    if (x < 1 || y <= 1 || x >= mapWidth || y >= mapHeight) return false;
                    if (World.GetTraversable(new Vector2(x, y)).terrainType != 0) return false;
                }
            }
            return true;
        }
        public static void SetTile(int x, int y, char character, string name, string description, string fColor, string bColor, bool opaque, int moveType)
        {
            World.tiles[x, y] = new Entity(new List<Component>() { 
                new Coordinate(x, y), 
                new Draw(fColor, bColor, character), 
                new Description(name, description), 
                new Visibility(opaque, false, false), 
                new Traversable(moveType)});
        }
    }
}
