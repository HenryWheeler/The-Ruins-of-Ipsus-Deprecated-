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
        public static void CreateMap(Entity coordinate, string name)
        {
            List<Entity> entity = new List<Entity>(); entity.Add(coordinate);
            CreateMap(entity, name, 250);
        }
        public static void CreateMap(List<Entity> coordinates, string name, int strength = 50)
        {
            int current = 0;
            Node[,] map = new Node[Program.gameMapWidth, Program.gameMapHeight];
            foreach (Tile tile in Map.map)
            {
                Coordinate tileCoordinate = tile.GetComponent<Coordinate>();
                if (CMath.CheckBounds(tileCoordinate.x, tileCoordinate.y)) { map[tileCoordinate.x, tileCoordinate.y] = new Node(tileCoordinate.x, tileCoordinate.y, 1000); }
            }
            foreach (Entity entity in coordinates) 
            { Coordinate coordinate = entity.GetComponent<Coordinate>(); if (CMath.CheckBounds(coordinate.x, coordinate.y)) { map[coordinate.x, coordinate.y].v = 0; } }

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
                                if (CMath.CheckBounds(x, y) && Map.map[x, y].moveType != 0)
                                {
                                    if (map[x, y].v > current)
                                    {
                                        if (Map.map[x, y].actor == null) { map[x, y].v = current + 1; finalNode = node; }
                                        else { map[x, y].v = current + 70; finalNode = node; }
                                        //Map.map[x, y].GetComponent<Draw>().character = (char)current;
                                    }
                                    else { continue; }
                                }
                                else continue;
                            }
                        }
                    }
                }
                current++;
            } while (current <= strength);

            AddMap(map, name);
        }
        public static void CombineThenCreate(Node[,] map1, Node[,] map2, string name)
        {
            Node[,] map = new Node[Program.gameMapWidth, Program.gameMapHeight];
            foreach (Tile tile in Map.map)
            {
                Coordinate tileCoordinate = tile.GetComponent<Coordinate>();
                if (CMath.CheckBounds(tileCoordinate.x, tileCoordinate.y) && map1[tileCoordinate.x, tileCoordinate.y] != null && map2[tileCoordinate.x, tileCoordinate.y] != null) 
                { map[tileCoordinate.x, tileCoordinate.y] = new Node(tileCoordinate.x, tileCoordinate.y, map1[tileCoordinate.x, tileCoordinate.y].v + map2[tileCoordinate.x, tileCoordinate.y].v); }
            }
            AddMap(map, name);
        }
        public static void ReverseMap(Node[,] map, string name) { foreach (Node node in map) { if (node != null) { node.v *= -1; } } AddMap(map, name); }
        private static void AddMap(Node[,] map, string name)
        {
            if (maps.ContainsKey(name)) { maps[name] = map; }
            else maps.Add(name, map);
        }
        public static void DiscardAll() { maps.Clear(); }
        public static Node PathFromMap(Entity entity, string mapName)
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            Node[,] map;
            if (maps.ContainsKey(mapName)) { map = maps[mapName]; }
            else { return null; }

            Node start = map[coordinate.x, coordinate.y];
            Node target = start;

            for (int y = start.y - 1; y <= start.y + 1; y++)
            {
                for (int x = start.x - 1; x <= start.x + 1; x++)
                {
                    if (CMath.CheckBounds(x, y))
                    {
                        if (map[x, y].v == 0) { target = map[x, y]; return new Node(target.x - coordinate.x, target.y - coordinate.y); }
                        else if ((y == start.y - 1 && x == start.x - 1) || (y == start.y - 1 && x == start.x + 1) || (y == start.y + 1 && x == start.x - 1) || (y == start.y + 1 && x == start.x + 1))
                        {
                            if (entity.GetComponent<Movement>().moveTypes.Contains(Map.map[x, y].moveType) && map[x, y].v + .5f < target.v) 
                            { target = map[x, y]; target.v += .5f; }
                            else continue;
                        }
                        else
                        {
                            if (entity.GetComponent<Movement>().moveTypes.Contains(Map.map[x, y].moveType) && map[x, y].v < target.v) 
                            { target = map[x, y]; }
                            else continue;
                        }
                    }
                }
            }
            return new Node(target.x - coordinate.x, target.y - coordinate.y, map[target.x, target.y].v);
        }
    }
    public class Node
    {
        public Node(int _x, int _y, float _v = 0)
        {
            x = _x;
            y = _y;
            v = _v;
        }
        public int x { get; set; }
        public int y { get; set; }
        public float v { get; set; }
    }
}
