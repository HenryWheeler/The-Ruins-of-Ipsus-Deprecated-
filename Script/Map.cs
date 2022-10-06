using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public static class Map
    {
        public static Tile[,] map;
    }
    [Serializable]
    public class Tile : IDraw
    {
        public int x { get; set; }
        public int y { get; set; }
        public char character { get; set; }
        public ActorBase actor = null;
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public bool explored { get; set; }
        public bool visible { get; set; }
        public bool walkable { get; set; }
        public void Draw(RLConsole console)
        {
            if (!visible && !explored) console.Set(x, y, RLColor.Black, RLColor.Black, character);
            else if (!visible && explored) console.Set(x, y, RLColor.Gray, RLColor.Black, character);
            else if (actor != null) actor.Draw(console);
            else console.Set(x, y, fColor, bColor, character);
        }
    }
    public class Room
    {
        public int x;
        public int y;
    }
}
