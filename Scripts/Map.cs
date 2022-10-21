using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Map
    {
        public Map(Tile[,] tiles)
        {
            map = tiles;
        }
        public static Tile[,] map;
    }
    [Serializable]
    public class Tile : IDraw, IDescription
    {
        public int x { get; set; }
        public int y { get; set; }
        public char character { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string fColor { get; set; }
        public string bColor { get; set; }
        public bool opaque { get; set; }
        public bool explored { get; set; }
        public bool visible { get; set; }
        public bool walkable { get; set; }
        public string spacer { get; set; }
        public ActorBase actor = null;
        public Item item = null;
        public string Describe() { return name + ": " + description; }
        public void Draw(RLConsole console)
        {
            if (!visible && !explored) console.Set(x, y, RLColor.Black, RLColor.Black, character);
            else if (!visible && explored) console.Set(x, y, RLColor.Gray, RLColor.Black, character);
            else if (actor != null) actor.Draw(console);
            else if (item != null) item.Draw(console);
            else console.Set(x, y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character);
        }
        public Tile(int _x, int _y, char _character, string _name, string _description, string _fColor, string _bColor, bool _opaque, bool _walkable)
        {
            x = _x;
            y = _y;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            bColor = _bColor;
            opaque = _opaque;
            walkable = _walkable;
            spacer = " + ";
        }
    }
    public class Room
    {
        public int x;
        public int y;
    }
}
