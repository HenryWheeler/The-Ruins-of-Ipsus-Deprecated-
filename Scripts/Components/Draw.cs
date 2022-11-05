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
        public int renderOrder { get; set; }
        public string fColor { get; set; }
        public string bColor { get; set; }
        public char character { get; set; }
        public bool displayOverAll { get; set; }
        public Draw(string _fColor, string _bColor, char _character, bool _displayOverAll = false) { fColor = _fColor; bColor = _bColor; character = _character; displayOverAll = _displayOverAll; }
        public Draw() { }
        public void DrawToScreen(RLConsole console)
        {
            Coordinate pos = entity.GetComponent<Coordinate>();
            Visibility vis = entity.GetComponent<Visibility>();

            if (!vis.visible && displayOverAll) { console.Set(pos.x, pos.y, ColorFinder.ColorPicker("Gray"), ColorFinder.ColorPicker("Black"), character); }
            else if (!vis.visible && !vis.explored) { console.Set(pos.x, pos.y, RLColor.Black, RLColor.Black, character); }
            else if (!vis.visible && vis.explored) { console.Set(pos.x, pos.y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(bColor), .55f), character); }
            else { console.Set(pos.x, pos.y, ColorFinder.ColorPicker(fColor), ColorFinder.ColorPicker(bColor), character); }
        }
    }
}
