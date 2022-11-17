using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class ColorFinder
    {
        public static RLColor ColorPicker(string color)
        {
            switch (color)
            {
                case "Red": return RLColor.Red;
                case "Blue": return RLColor.Blue;
                case "Light_Blue": return RLColor.LightBlue;
                case "Dark_Blue": return RLColor.Blend(RLColor.Black, RLColor.Blue, 50);
                case "Yellow": return RLColor.Yellow;
                case "Green": return RLColor.Green;
                case "Light_Green": return RLColor.LightGreen;
                case "Dark_Green": return RLColor.Blend(RLColor.Black, RLColor.Green, 50);
                case "Gray": return RLColor.Gray;
                case "Light_Gray": return RLColor.LightGray;
                case "Dark_Gray": return RLColor.Blend(RLColor.Black, RLColor.Gray, 50);
                case "Gray_Blue": return RLColor.Blend(RLColor.Blue, RLColor.Gray, 50);
                case "Light_Gray_Blue": return RLColor.Blend(RLColor.LightBlue, RLColor.Gray, 50);
                case "Purple_Gray": return RLColor.Blend(RLColor.Magenta, RLColor.Gray, 50);
                case "Light_Purple_Gray": return RLColor.Blend(RLColor.LightMagenta, RLColor.Gray, 50);
                case "Dark_Purple": return RLColor.Blend(RLColor.Magenta, RLColor.Gray, 50);
                case "White": return RLColor.White;
                case "Black": return RLColor.Black;
                case "Brown": return RLColor.Brown;
            }
            return RLColor.Black;
        }
    }
}
