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
            CreateMap(entity, name);
        }
        public static void CreateMap(List<Entity> coordinates, string name)
        {
            //Log.Add($"Running map {name} with {coordinates.Count} entry coordinates");
            int current = 0;
            Node[,] map = new Node[Program.gameMapWidth, Program.gameMapHeight];
            for (int x = 0; x < Program.gameMapWidth; x++)
            {
                for (int y = 0; y < Program.gameMapHeight; y++)
                {
                    map[x, y] = new Node(x, y, 1000);
                }
            }
            //foreach (Node node in map) { map[node.x, node.y] = new Node(node.x, node.y, 1000); }
            foreach (Entity entity in coordinates) 
            { 
                Vector2 vector3 = entity.GetComponent<Coordinate>().vector2;
                map[vector3.x, vector3.y] = new Node(vector3.x, vector3.y, 0);
            }

            Node finalNode;
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
                                if (CMath.CheckBounds(x, y))
                                {
                                    Traversable traversable = World.tiles[x, y].GetComponent<Traversable>();
                                    if (traversable.terrainType != 0 && map[x, y].v > current)
                                    {
                                        if (traversable.actorLayer == null) 
                                        {
                                            map[x, y].v = current + 1;
                                            finalNode = node; 
                                        }
                                        else 
                                        {
                                            map[x, y].v = current + 70;
                                            finalNode = node; 
                                        }
                                        Draw draw = World.tiles[x, y].GetComponent<Draw>();
                                        //draw.character = (char)current;
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
        public static void CombineThenCreate(Node[,] map1, Node[,] map2, string name)
        {
            Node[,] map = new Node[Program.gameMapWidth, Program.gameMapHeight];
            foreach (Entity tile in World.tiles)
            {
                if (tile != null)
                {
                    Vector2 vector3 = tile.GetComponent<Coordinate>().vector2;
                    if (CMath.CheckBounds(vector3.x, vector3.y) && map1[vector3.x, vector3.y] != null && map2[vector3.x, vector3.y] != null)
                    { map[vector3.x, vector3.y] = new Node(vector3.x, vector3.y, map1[vector3.x, vector3.y].v + map2[vector3.x, vector3.y].v); }
                }
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
        public static Vector2 PathFromMap(Entity entity, string mapName)
        {
            Vector2 vector3 = entity.GetComponent<Coordinate>().vector2;
            Node[,] map;
            if (maps.ContainsKey(mapName)) { map = maps[mapName]; }
            else { return null; }

            Node start = map[vector3.x, vector3.y];
            Node target = start;

            for (int y = start.y - 1; y <= start.y + 1; y++)
            {
                for (int x = start.x - 1; x <= start.x + 1; x++)
                {
                    if (CMath.CheckBounds(x, y))
                    {
                        if (map[x, y].v == 0) 
                        {
                            target = map[x, y]; 
                            return new Vector2(target.x, target.y); 
                        }
                        else if ((y == start.y - 1 && x == start.x - 1) || (y == start.y - 1 && x == start.x + 1) || (y == start.y + 1 && x == start.x - 1) || (y == start.y + 1 && x == start.x + 1))
                        {
                            if (entity.GetComponent<Movement>().moveTypes.Contains(World.GetTraversable(new Vector2(x, y)).terrainType) && map[x, y].v + .5f < target.v) 
                            {
                                target = map[x, y]; target.v += .5f;
                            }
                            else continue;
                        }
                        else
                        {
                            if (entity.GetComponent<Movement>().moveTypes.Contains(World.GetTraversable(new Vector2(x, y)).terrainType) && map[x, y].v < target.v) 
                            { 
                                target = map[x, y]; 
                            }
                            else continue;
                        }
                    }
                }
            }
            return new Vector2(target.x, target.y);
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
