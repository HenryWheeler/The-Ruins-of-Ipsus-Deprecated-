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
        private static bool isFirst = true;
        public static Random random;
        public static List<Room> rooms;
        private static List<Room> currentRooms;
        public static void CreateMap(int _mapWidth, int _mapHeight, int _minRoomSize, int _maxRoomSize, int _roomsToGenerate, Random _random)
        {
            Map.map = new Tile[_mapWidth, _mapHeight];

            mapWidth = _mapWidth;
            mapHeight = _mapHeight;
            maxRoomSize = _maxRoomSize;
            minRoomSize = _minRoomSize;
            roomsToGenerate = _roomsToGenerate;
            random = _random;
            rooms = new List<Room>();
            currentRooms = new List<Room>();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", RLColor.White, RLColor.Gray, true, false);
                }
            }
            for (int i = 0; i < roomsToGenerate; i++)
            {
                int xSP = random.Next(0, _mapWidth);
                int ySP = random.Next(0, _mapHeight);
                int rW = random.Next(minRoomSize, maxRoomSize);
                int rH = random.Next(minRoomSize, maxRoomSize);

                if (!CheckIfHasSpace(xSP, ySP, xSP + rW - 1, ySP + rH - 1)) { i--; continue; }
                CreateRoom(xSP, ySP, rW, rH);
                if (!isFirst) { CreatePassage(currentRooms[0], currentRooms[1]); currentRooms.Remove(currentRooms[0]); }
                else isFirst = false;
            }
        }
        public static void CreateRoom(int _x, int _y, int roomWidth, int roomHeight)
        {
            Room room = new Room();
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    int _X = _x + x;
                    int _Y = _y + y;

                    SetTile(_X, _Y, '.', "Stone Floor", "A simple stone floor.", RLColor.Brown, RLColor.Black, false, true);
                }
            }

            room.x = (roomWidth / 2) + _x;
            room.y = (roomHeight / 2) + _y;

            rooms.Add(room);
            currentRooms.Add(room);
        }
        public static void CreatePassage(Room r1, Room r2)
        {
            if (random.Next(0, 1) == 0)
            {
                for (int x = Math.Min(r1.x, r2.x); x <= Math.Max(r1.x, r2.x); x++)
                {
                    SetTile(x, r1.y, '.', "Stone Floor", "A simple stone floor.", RLColor.Brown, RLColor.Black, false, true);
                }
                for (int y = Math.Min(r1.y, r2.y); y <= Math.Max(r1.y, r2.y); y++)
                {
                    SetTile(r2.x, y, '.', "Stone Floor", "A simple stone floor.", RLColor.Brown, RLColor.Black, false, true);
                }
            }
            else
            {
                for (int y = Math.Min(r1.y, r2.y); y <= Math.Max(r1.y, r2.y); y++)
                {
                    SetTile(r1.x, y, '.', "Stone Floor", "A simple stone floor.", RLColor.Brown, RLColor.Black, false, true);
                }
                for (int x = Math.Min(r1.x, r2.x); x <= Math.Max(r1.x, r2.x); x++)
                {
                    SetTile(x, r2.y, '.', "Stone Floor", "A simple stone floor.", RLColor.Brown, RLColor.Black, false, true);
                }
            }
        }
        private static bool CheckIfHasSpace(int sX, int sY, int eX, int eY)
        {
            for (int y = sY - 1; y <= eY + 1; y++)
            {
                for (int x = sX - 1; x <= eX + 1; x++)
                {
                    if (x < 1 || y < 1 || x >= mapWidth - 1|| y >= mapHeight - 1) return false;
                    if (Map.map[x, y].character == '.') return false;
                }
            }
            return true;
        }
        private static void SetTile(int x, int y, char character, string name, string description, RLColor fColor, RLColor bColor, bool opaque, bool walkable)
        {
            Map.map[x, y] = new Tile(x, y, character, name, description, fColor, bColor, opaque, walkable);
        }
    }
}
