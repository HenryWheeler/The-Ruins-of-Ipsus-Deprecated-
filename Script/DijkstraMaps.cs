using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeTest
{
    public class DijkstraMaps
    {
        public static Dictionary<string, Node[,]> maps = new Dictionary<string, Node[,]>();
        public static void CreateMap(Node point, string name)
        {
            int timesToRun = 50;
            int current = 0;
            Node[,] map = new Node[80, 70];
            foreach (Tile tile in Map.map)
            {
                if (CheckBounds(tile.x, tile.y))
                {
                    map[tile.x, tile.y] = new Node(tile.x, tile.y, 1000);
                }
            }
            map[point.x, point.y] = point;

            while (current < timesToRun)
            {
                foreach (Node node in map)
                {
                    if (node != null && node.v == current)
                    {
                        for (int y = node.y - 1; y <= node.y + 1; y++)
                        {
                            for (int x = node.x - 1; x <= node.x + 1; x++)
                            {
                                if (CheckBounds(x, y) && Map.map[x, y].walkable)
                                {
                                    if (Map.map[x, y].actor == null)
                                    {
                                        if (map[x, y].v > current) map[x, y].v = current + 1;
                                        else continue;
                                    } else if (map[x, y].v > current) map[x, y].v = current + 25;
                                }
                                else continue;
                            }
                        }
                    }
                }
                current++;
            }
            AddMap(map, name);
        }
        private static void AddMap(Node[,] map, string name)
        {
            if (maps.ContainsKey(name)) maps[name] = map;
            else maps.Add(name, map);
        }
        private static bool CheckBounds(int x, int y)
        {
            if (x <= 0 || x >= 80 || y <= 0 || y >= 70) return false;
            else return true;
        }
        public static Node PathFromMap(int _x, int _y, string mapName)
        {
            Node[,] map;
            if (maps.ContainsKey(mapName)) map = maps[mapName];
            else return null;

            Node start = map[_x, _y];
            Node target = start;

            for (int y = start.y - 1; y <= start.y + 1; y++)
            {
                for (int x = start.x - 1; x <= start.x + 1; x++)
                {
                    if ((y == start.y - 1 && x == start.x - 1) || (y == start.y - 1 && x == start.x + 1) || (y == start.y + 1 && x == start.x - 1) || (y == start.y + 1 && x == start.x + 1))
                    {
                        if (map[x, y].v + .5f < target.v) { target = map[x, y]; target.v += .5f; }
                        else continue;
                    }
                    else
                    {
                        if (map[x, y].v < target.v) target = map[x, y];
                        else continue;
                    }
                }
            }

            return target;
        }
    }
    public class Node
    {
        public Node(int _x, int _y, int _v = 0)
        {
            x = _x;
            y = _y;
            v = _v;
        }
        public int x { get; set; }
        public int y { get; set; }
        public float v { get; set; }
        public Node previous { get; set; }
    }
}
