using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void DrawToScreen(RLConsole console, int x, int y) { console.Set(x, y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character); }
    }
}
