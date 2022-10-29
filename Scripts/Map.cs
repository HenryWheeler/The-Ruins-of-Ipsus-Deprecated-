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
        public int moveType { get; set; }
        public string spacer { get; set; }
        public ActorBase actor = null;
        public Item item = null;
        public string Describe() { return name + ": " + description; }
        public void Draw(RLConsole console)
        {
            if (Map.sfx[x, y] != null) { Map.sfx[x, y].Draw(console); }
            else if (!visible && !explored) console.Set(x, y, RLColor.Black, RLColor.Black, character);
            else if (!visible && explored) console.Set(x, y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(bColor), .55f), character);
            else if (actor != null) actor.Draw(console);
            else if (item != null) item.Draw(console);
            else console.Set(x, y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character);
        }
        public Tile(int _x, int _y, char _character, string _name, string _description, string _fColor, string _bColor, bool _opaque, int _moveType)
        {
            x = _x;
            y = _y;
            character = _character;
            name = _name;
            description = _description;
            fColor = _fColor;
            bColor = _bColor;
            opaque = _opaque;
            moveType = _moveType;
            spacer = " + ";
        }
    }
    [Serializable]
    public class SFX : IDraw
    {
        public int x { get; set; }
        public int y { get; set; }
        public char character { get; set; }
        public string fColor { get; set; }
        public string bColor { get; set; }
        public bool opaque { get; set; }
        public void Draw(RLConsole console)
        {
            console.Set(x, y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character);
        }
        public SFX(int _x, int _y, char _character, string _fColor, string _bColor, bool _opaque)
        {
            x = _x;
            y = _y;
            character = _character;
            fColor = _fColor;
            bColor = _bColor;
            opaque = _opaque;
        }
    }
}
