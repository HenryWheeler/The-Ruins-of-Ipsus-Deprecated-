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
                case "Yellow": return RLColor.Yellow;
                case "Green": return RLColor.Green;
                case "Gray": return RLColor.Gray;
                case "White": return RLColor.White;
                case "Black": return RLColor.Black;
                case "Brown": return RLColor.Brown;
            }
            return RLColor.Black;
        }
    }
}
