using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class World
    {
        public World(int width, int height, int depth)
        {
            chunks = new Chunk[width, height, depth];
            chunkWidth = Renderer.mapWidth;
            chunkHeight = Renderer.mapHeight;
            chunkSeeds = new double[width, height, depth];
            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    for (int z = 0; z < depth - 1; y++)
                    {
                        chunkSeeds[x, y, z] = CMath.seed.NextDouble();
                    }
                }
            }
        }
        public static void CreateChunk(int x, int y, int z, string environment)
        {
            chunks[x, y, z] = new Chunk(chunkWidth, chunkHeight, environment, chunkSeeds[x, y, z]);
        }
        public static Chunk[,,] chunks;
        public static double[,,] chunkSeeds; 
        public static int chunkWidth { get; set; }
        public static int chunkHeight { get; set; }
    }
    [Serializable]
    public class Chunk
    {
        public Chunk(int width, int height, string _environment, double seedStart)
        {
            tiles = new Tile[width, height];
            environment = _environment;
            chunkSeed = new Random((int)Math.Floor(seedStart) + (int)Math.Ceiling(seedStart));
        }
        public static Tile[,] tiles { get; set; }
        public static string environment { get; set; }
        public static Random chunkSeed { get; set; }
    }
    [Serializable]
    public class Map
    {
        public Map(int width, int height)
        {
            map = new Tile[width, height];
            sfx = new SFX[width, height];
        }
        public static Tile[,] map;
        public static SFX[,] sfx;
        public static bool outside;
        public Map() { }
    }
    [Serializable]
    public class Tile: Entity
    {
        public int moveType { get; set; }
        public Entity actor = null;
        public Entity item = null;
        public Entity terrain = null;
        public Tile(int _x, int _y, char _character, string _name, string _description, string _fColor, string _bColor, bool _opaque, int _moveType)
        {
            AddComponent(new Coordinate(_x, _y));
            AddComponent(new Draw(_fColor, _bColor, _character));
            AddComponent(new Description(_name, _description));
            AddComponent(new Visibility(_opaque, false, false));
            moveType = _moveType;
        }
        public Tile() { }
    }
    [Serializable]
    public class SFX: Entity
    {
        public SFX(int _x, int _y, char _character, string _fColor, string _bColor, bool _opaque, bool _displayOverAll)
        {
            AddComponent(new Coordinate(_x, _y));
            AddComponent(new Draw(_fColor, _bColor, _character, _displayOverAll));
            Visibility vis = Map.map[_x, _y].GetComponent<Visibility>();
            AddComponent(new Visibility(_opaque, vis.visible, vis.explored));
        }
        public SFX() { }
    }
}
