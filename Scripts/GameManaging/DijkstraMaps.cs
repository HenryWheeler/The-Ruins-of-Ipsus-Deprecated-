using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class DijkstraMaps
    {
        public static Dictionary<string, Node[,]> maps = new Dictionary<string, Node[,]>();
        public static Coordinate CreateToPath(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            int current = 0;
            Node[,] map = new Node[80, 70];
            foreach (Tile tile in Map.map)
            {
                Coordinate tileCoordinate = tile.GetComponent<Coordinate>();
                if (CheckBounds(tileCoordinate.x, tileCoordinate.y)) { map[tileCoordinate.x, tileCoordinate.y] = new Node(tileCoordinate.x, tileCoordinate.y, 1000); }
            }
            map[endCoordinate.x, endCoordinate.y] = new Node(endCoordinate.x, endCoordinate.y, 0);

            Node finalNode = null;
            do
            {
                finalNode = null;
                foreach (Node node in map)
                {
                    if (node != null && node.v == current)
                    {
                        for (int y = node.y - 1; y <= node.y + 1; y++)
                        {
                            for (int x = node.x - 1; x <= node.x + 1; x++)
                            {
                                if (CheckBounds(x, y) && Map.map[x, y].moveType != 0)
                                {
                                    if (map[x, y].v > current)
                                    {
                                        if (Map.map[x, y].actor == null && Map.map[x, y].moveType != 2) { map[x, y].v = current + 1; finalNode = node; }
                                        else if (Map.map[x, y].actor != null && Map.map[x, y].moveType == 2) { map[x, y].v = current + 70; finalNode = node; }
                                        else { map[x, y].v = current + 35; finalNode = node; }
                                    }
                                    else { continue; }
                                }
                                else continue;
                            }
                        }
                    }
                }
                current++;
            } while (finalNode != map[startCoordinate.x, startCoordinate.y]);

            Node start = map[startCoordinate.x, startCoordinate.y];
            Node target = start;

            for (int y = start.y - 1; y <= start.y + 1; y++)
            {
                for (int x = start.x - 1; x <= start.x + 1; x++)
                {
                    if (CMath.CheckBounds(x, y))
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
            }
            return new Coordinate(target.x - start.x, target.y - start.y);
        }
        public static void CreateMap(Coordinate coordinate, string name)
        {
            int current = 0;
            Node[,] map = new Node[80, 70];
            foreach (Tile tile in Map.map)
            {
                Coordinate tileCoordinate = tile.GetComponent<Coordinate>();
                if (CheckBounds(tileCoordinate.x, tileCoordinate.y)) { map[tileCoordinate.x, tileCoordinate.y] = new Node(tileCoordinate.x, tileCoordinate.y, 1000); }
            }
            map[coordinate.x, coordinate.y] = new Node(coordinate.x, coordinate.y, 0);

            Node finalNode = null;
            do
            {
                finalNode = null;
                foreach (Node node in map)
                {
                    if (node != null && node.v == current)
                    {
                        for (int y = node.y - 1; y <= node.y + 1; y++)
                        {
                            for (int x = node.x - 1; x <= node.x + 1; x++)
                            {
                                if (CheckBounds(x, y) && Map.map[x, y].moveType != 0)
                                {
                                    if (map[x, y].v > current)
                                    {
                                        if (Map.map[x, y].actor == null && Map.map[x, y].moveType != 2) { map[x, y].v = current + 1; finalNode = node; }
                                        else if (Map.map[x, y].actor != null && Map.map[x, y].moveType == 2) { map[x, y].v = current + 70; finalNode = node; }
                                        else { map[x, y].v = current + 35; finalNode = node; }
                                    }
                                    else { continue; }
                                }
                                else continue;
                            }
                        }
                    }
                }
                current++;
            } while (finalNode != null);

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
        public static void DiscardAll() { maps.Clear(); }
        public static Coordinate PathFromMap(Coordinate coordinate, string mapName)
        {
            int _x = coordinate.x;
            int _y = coordinate.y;
            Node[,] map;
            if (maps.ContainsKey(mapName)) map = maps[mapName];
            else return null;

            Node start = map[_x, _y];
            Node target = start;

            for (int y = start.y - 1; y <= start.y + 1; y++)
            {
                for (int x = start.x - 1; x <= start.x + 1; x++)
                {
                    if (CMath.CheckBounds(x, y))
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
            }
            return new Coordinate(target.x - _x, target.y - _y);
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
