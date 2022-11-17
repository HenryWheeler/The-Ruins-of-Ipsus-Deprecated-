using System;
using RLNET;

namespace TheRuinsOfIpsus
{
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
