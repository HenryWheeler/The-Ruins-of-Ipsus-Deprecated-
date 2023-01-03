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
                case "Red_Orange": return RLColor.Blend(RLColor.Red, ColorPicker("Orange"), 50);
                case "Blue": return RLColor.Blue;
                case "Orange": return RLColor.Blend(RLColor.Red, RLColor.Yellow, 50);
                case "Light_Blue": return RLColor.LightBlue;
                case "Dark_Blue": return RLColor.Blend(RLColor.Black, RLColor.Blue, 50);
                case "Yellow": return RLColor.Yellow;
                case "Yellow_Gray": return RLColor.Blend(RLColor.Yellow, RLColor.Gray, 50);
                case "Light_Yellow": return RLColor.Blend(RLColor.Yellow, RLColor.White, 50);
                case "Light_Yellow_Gray": return RLColor.Blend(RLColor.Blend(RLColor.Yellow, RLColor.White, 50), RLColor.Gray, 50);
                case "Dark_Yellow_Gray": return RLColor.Blend(RLColor.Blend(RLColor.Yellow, RLColor.Black, 50), RLColor.Gray, 50);
                case "Dark_Yellow": return RLColor.Blend(RLColor.Yellow, RLColor.Black, 50);
                case "Green": return RLColor.Green;
                case "Light_Green": return RLColor.LightGreen;
                case "Dark_Green": return RLColor.Blend(RLColor.Black, RLColor.Green, 50);
                case "Gray": return RLColor.Gray;
                case "Light_Gray": return RLColor.LightGray;
                case "Dark_Gray": return RLColor.Blend(RLColor.Black, RLColor.Gray, 50);
                case "Gray_Blue": return RLColor.Blend(RLColor.Blue, RLColor.Gray, 50);
                case "Light_Gray_Blue": return RLColor.Blend(RLColor.LightBlue, RLColor.Gray, 50);
                case "Purple": return RLColor.Magenta;
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
