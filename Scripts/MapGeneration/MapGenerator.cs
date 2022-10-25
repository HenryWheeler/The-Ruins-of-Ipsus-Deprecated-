using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class MapGenerator
    {
        private static int mapWidth;
        private static int mapHeight;
        private static int maxRoomSize;
        private static int minRoomSize;
        private static int roomsToGenerate;
        private static int wallsNeeded;
        private static int randomFill;
        private static int smooth;
        private static bool doneConnecting = false;
        public static void CreateMap(int _mapWidth, int _mapHeight, int _minRoomSize, int _maxRoomSize, int _roomsToGenerate)
        {
            Map map = new Map(new Tile[_mapWidth, _mapHeight]);

            mapWidth = _mapWidth;
            mapHeight = _mapHeight;
            maxRoomSize = _maxRoomSize;
            minRoomSize = _minRoomSize;
            roomsToGenerate = _roomsToGenerate;

            randomFill = CMath.seed.Next(46, 52);
            wallsNeeded = 4;
            smooth = 5;

            int type = CMath.seed.Next(0, 3);

            switch (type)
            {
                case 0: CreateDungeon(); break;
                case 1: CreateCave(); break;
                case 2: CreateField(); break;
            }
        }
        public static void SetAllWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false);
                }
            }
        }
        public static void CreateSurroundingWalls()
        {
            int h = 69;
            int w = 79;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else if (y == h && x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else if (y == 0 && x == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else if (x == w && y == 0) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else if (y == 0 || y == h) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else if (x == 0 || x == w) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                    else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, true); }
                }
            }
        }
        public static void CreateDungeon()
        {
            Map.outside = false;

            SetAllWalls();

            for (int i = 0; i < roomsToGenerate; i++)
            {
                int xSP = CMath.seed.Next(0, mapWidth);
                int ySP = CMath.seed.Next(0, mapHeight);
                int rW = CMath.seed.Next(minRoomSize, maxRoomSize);
                int rH = CMath.seed.Next(minRoomSize, maxRoomSize);

                if (!CheckIfHasSpace(xSP, ySP, xSP + rW - 1, ySP + rH - 1)) { i--; continue; }
                CreateRoom(xSP, ySP, rW, rH);
            }
            CreateConnections(2, false);
        }
        public static void CreateCave()
        {
            Map.outside = false;

            SetAllWalls();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        if (CMath.seed.Next(0, 100) < randomFill) { SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true); }
                    }
                }
            }
            for (int z = 0; z < smooth; z++) { SmoothMap(); }
            CreateConnections(1, true);
        }
        public static void CreateField()
        {
            Map.outside = true;

            CreateSurroundingWalls();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
                    {
                        int probability = CMath.seed.Next(0, 100);
                        if (probability > 80) { SetTile(x, y, '"', "Grass", "Soft Green Grass.", "Dark_Green", "Black", false, true); }
                        else if (probability > 60) { SetTile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, true); }
                        else if (probability == 1) { CreateTreePatch(x, y); }
                        else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, true); }
                    }
                }
            }
        }
        public static void CreateTreePatch(int _x, int _y)
        {
            int size = CMath.seed.Next(1, 4);

            for (int x = _x - size; x < _x + size; x++)
            {
                for (int y = _y - size; y < _y + size; y++)
                {
                    if (CMath.CheckBounds(x, y))
                    {
                        if (CMath.seed.Next(1, 100) > 50) { SetTile(x, y, (char)20, "Oak Tree", "A solid sturdy oak.", "Dark_Green", "Black", true, false); }
                    }
                }
            }
        }
        public static void SmoothMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        int walls = WallCount(x, y);

                        if (walls > wallsNeeded) { SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, false); }
                        else if (walls < wallsNeeded) { SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true); } 
                    }
                }
            }
        }
        public static int WallCount(int sX, int sY)
        {
            int walls = 0;

            for (int x = sX - 1; x <= sX + 1; x++)
            {
                for (int y = sY - 1; y <= sY + 1; y++)
                {
                    if (x != sX || y != sY) { if (CMath.CheckBounds(x, y) && !Map.map[x, y].walkable) { walls++; } }
                }
            }

            return walls;
        }
        public static void CreateRoom(int _x, int _y, int roomWidth, int roomHeight)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    int _X = _x + x;
                    int _Y = _y + y;

                    SetTile(_X, _Y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                }
            }
        }
        public static void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y)
        {
            int t;
            int x = r1x; int y = r1y;
            int delta_x = r2x - r1x; int delta_y = r2y - r1y;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);
            bool hasConnected = false;

            if (abs_delta_x > abs_delta_y)
            {
                t = abs_delta_y * 2 - abs_delta_x;
                do
                {
                    if (t >= 0) { y += sign_y; t -= abs_delta_x * 2; }
                    x += sign_x;
                    t += abs_delta_y * 2;
                    if (!Map.map[x, y].walkable)
                    {
                        SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                        SetTile(x + 1, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                    }
                    if (x == r2x && y == r2y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
            else
            {
                t = abs_delta_x * 2 - abs_delta_y;
                do
                {
                    if (t >= 0) { x += sign_x; t -= abs_delta_y * 2; }
                    y += sign_y;
                    t += abs_delta_x * 2;
                    if (!Map.map[x, y].walkable)
                    {
                        SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                        SetTile(x, y + 1, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                    }
                    if (x == r2x && y == r2y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
        }
        public static void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y)
        {
            int r1x; int r1y;
            if (CMath.seed.Next(1, 100) <= 50) { r1x = r0x; r1y = r2y; }
            else { r1x = r2x; r1y = r0y; }

            for (float t = 0; t < 1; t += .001f)
            {
                int x = (int)((1 - t) * ((1 - t) * r0x + t * r1x) + t * ((1 - t) * r0x + t * r2x));
                int y = (int)((1 - t) * ((1 - t) * r0y + t * r1y) + t * ((1 - t) * r0y + t * r2y));
                if (CMath.CheckBounds(x, y)) { SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true); }
            }
        }
        public static void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y)
        {
            if (CMath.seed.Next(0, 1) == 0)
            {
                for (int x = Math.Min(r1x, r2x); x <= Math.Max(r1x, r2x); x++)
                {
                    SetTile(x, r1y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                }
                for (int y = Math.Min(r1y, r2y); y <= Math.Max(r1y, r2y); y++)
                {
                    SetTile(r2x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                }
            }
            else
            {
                for (int y = Math.Min(r1y, r2y); y <= Math.Max(r1y, r2y); y++)
                {
                    SetTile(r1x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                }
                for (int x = Math.Min(r1x, r2x); x <= Math.Max(r1x, r2x); x++)
                {
                    SetTile(x, r2y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, true);
                }
            }
        }
        private static void CreateConnections(int loopCount, bool natural)
        {
            while (!doneConnecting)
            {
                Tile firstTile = null;
                Tile lastTile = null;
                List<Tile> cavernTiles = new List<Tile>();

                foreach (Tile tile in Map.map) { if (tile != null && tile.walkable) { firstTile = tile; break; } }

                DijkstraMaps.CreateMap(new Node(firstTile.x, firstTile.y), "CurrentRoom");
                foreach (Node node in DijkstraMaps.maps["CurrentRoom"]) { if (node != null && node.v != 1000) { cavernTiles.Add(Map.map[node.x, node.y]); } }
                for (int v = 0; v < loopCount; v++)
                {
                    foreach (Tile tile in Map.map)
                    {
                        if (tile != null && tile.walkable && !cavernTiles.Contains(tile))
                        {
                            if (lastTile == null) { lastTile = tile; }
                            else { if (CMath.Distance(tile.x, tile.y, firstTile.x, firstTile.y) < CMath.Distance(lastTile.x, lastTile.y, firstTile.x, firstTile.y)) { lastTile = tile; } }
                        }
                    }
                    foreach (Tile tile in cavernTiles)
                    {
                        if (tile != null && tile.walkable && cavernTiles.Contains(tile))
                        {
                            if (lastTile != null && CMath.Distance(tile.x, tile.y, firstTile.x, firstTile.y) < CMath.Distance(lastTile.x, lastTile.y, firstTile.x, firstTile.y)) { firstTile = tile; }
                        }
                    }
                }
                if (lastTile == null) { doneConnecting = true; }
                else if (!natural) { CreateStraightPassage(firstTile.x, firstTile.y, lastTile.x, lastTile.y); }
                else { CreateBezierCurve(firstTile.x, firstTile.y, lastTile.x, lastTile.y); }
            }
        }
        private static bool CheckIfHasSpace(int sX, int sY, int eX, int eY)
        {
            for (int y = sY - 1; y <= eY + 1; y++)
            {
                for (int x = sX - 1; x <= eX + 1; x++)
                {
                    if (x < 1 || y < 1 || x >= mapWidth - 1|| y >= mapHeight - 1) return false;
                    if (Map.map[x, y].walkable) return false;
                }
            }
            return true;
        }
        private static void SetTile(int x, int y, char character, string name, string description, string fColor, string bColor, bool opaque, bool walkable)
        {
            Map.map[x, y] = new Tile(x, y, character, name, description, fColor, bColor, opaque, walkable);
        }

    }
}
