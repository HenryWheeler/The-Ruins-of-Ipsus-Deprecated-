using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Draw: Component
    {
        public string fColor { get; set; }
        public string bColor { get; set; }
        public char character { get; set; }
        public Draw(string _fColor, string _bColor, char _character) { fColor = _fColor; bColor = _bColor; character = _character; }
        public Draw(Draw draw) { fColor = draw.fColor; bColor = draw.bColor; character = draw.character; }
        public Draw() { }
        public void DrawToScreen(Console console, int x, int y) { console.SetCellAppearance(x, y, new Cell(ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character)); }
    }
}
