using System;
using RLNET;

namespace RoguelikeTest
{
    interface IDraw
    {
        int x { get; set; }
        int y { get; set; }
        RLColor fColor { get; set; }
        RLColor bColor { get; set; }
        char character { get; set; }
        bool opaque { get; set; }
        void Draw(RLConsole console);
    }
}
