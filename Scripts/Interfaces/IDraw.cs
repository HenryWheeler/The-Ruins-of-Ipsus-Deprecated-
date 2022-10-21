using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    interface IDraw
    {
        int x { get; set; }
        int y { get; set; }
        string fColor { get; set; }
        string bColor { get; set; }
        char character { get; set; }
        bool opaque { get; set; }
        void Draw(RLConsole console);
    }
}
