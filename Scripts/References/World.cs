using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class World
    {
        public World(int width, int height, int depth, int _seed = 0, bool randomSeed = true)
        {
            if (randomSeed) { seed = new Random(); }
            else { seed = new Random(_seed); }
            chunkWidth = width;
            chunkHeight = height;
            tiles = new Entity[width * chunkWidth, height * chunkHeight, depth];
            chunks = new OverworldGeneration().CreateOverworld(chunkWidth, chunkHeight);
            foreach (Entity entity in chunks) 
            {
                if (entity != null)
                {
                    Chunk chunk = entity.GetComponent<Chunk>();
                    Coordinate coordinate = entity.GetComponent<Coordinate>();
                    ChunkGenerator.CreateChunk(chunk, coordinate.x, coordinate.y, 0);
                }
            }
        }
        public static Random seed { get; set; }
        public static Entity[,] chunks;
        public static Entity[,,] tiles; 
        public static int chunkWidth { get; set; }
        public static int chunkHeight { get; set; }
    }
    [Serializable]
    public class Chunk: Component
    {
        public int chunkWidth { get; set; }
        public int chunkHeight { get; set; }
        public string environment { get; set; }
        public int strength { get; set; }
        public Chunk(int _chunkWidth, int _chunkHeight, string _environment, int _strength)
        {
            chunkWidth = _chunkWidth;
            chunkHeight = _chunkHeight;
            environment = _environment;
            strength = _strength;
        }
        public Chunk() { }
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
