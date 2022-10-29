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
        public int mapWidth = 80;
        public int mapHeight = 70;
        public abstract void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y);
        public abstract void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y);
        public abstract void SetAllWalls();
        public void CreateSurroundingWalls()
        {
            int h = mapHeight - 1;
            int w = mapWidth - 1;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == h && x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == 0 && x == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (x == w && y == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (y == 0 || y == h) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                    else if (x == 0 || x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0); }
                }
            }
        }
        public void CreateConnections(int loopCount, int type)
        {
            bool doneConnecting = false;

            while (!doneConnecting)
            {
                Tile firstTile = null;
                Tile lastTile = null;
                List<Tile> cavernTiles = new List<Tile>();

                foreach (Tile tile in Map.map) { if (tile != null && tile.moveType != 0) { firstTile = tile; break; } }

                DijkstraMaps.CreateMap(new Node(firstTile.x, firstTile.y), "CurrentRoom");
                foreach (Node node in DijkstraMaps.maps["CurrentRoom"]) { if (node != null && node.v != 1000) { cavernTiles.Add(Map.map[node.x, node.y]); } }
                for (int v = 0; v < loopCount; v++)
                {
                    foreach (Tile tile in Map.map)
                    {
                        if (tile != null && tile.moveType != 0 && !cavernTiles.Contains(tile))
                        {
                            if (lastTile == null) { lastTile = tile; }
                            else { if (CMath.Distance(tile.x, tile.y, firstTile.x, firstTile.y) < CMath.Distance(lastTile.x, lastTile.y, firstTile.x, firstTile.y)) { lastTile = tile; } }
                        }
                    }
                    foreach (Tile tile in cavernTiles)
                    {
                        if (tile != null && tile.moveType != 0)
                        {
                            if (lastTile != null && CMath.Distance(tile.x, tile.y, firstTile.x, firstTile.y) < CMath.Distance(lastTile.x, lastTile.y, firstTile.x, firstTile.y)) { firstTile = tile; }
                        }
                    }
                }
                if (lastTile == null) { doneConnecting = true; }
                else
                {
                    switch (type)
                    {
                        case 0: CreateStraightPassage(firstTile.x, firstTile.y, lastTile.x, lastTile.y); break;
                        case 1: CreateDiagonalPassage(firstTile.x, firstTile.y, lastTile.x, lastTile.y); break;
                        case 2: CreateBezierCurve(firstTile.x, firstTile.y, lastTile.x, lastTile.y); break;
                    }
                }
            }
        }
        public bool CheckIfHasSpace(int sX, int sY, int eX, int eY)
        {
            for (int y = sY - 1; y <= eY + 1; y++)
            {
                for (int x = sX - 1; x <= eX + 1; x++)
                {
                    if (x < 1 || y < 1 || x >= 79 || y >= 69) return false;
                    if (Map.map[x, y].moveType != 0) return false;
                }
            }
            return true;
        }
        public void SetTile(int x, int y, char character, string name, string description, string fColor, string bColor, bool opaque, int moveType)
        {
            Map.map[x, y] = new Tile(x, y, character, name, description, fColor, bColor, opaque, moveType);
        }
    }
}
