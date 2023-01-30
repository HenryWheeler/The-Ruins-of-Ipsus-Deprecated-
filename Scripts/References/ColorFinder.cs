using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TheRuinsOfIpsus
{
    public class ColorFinder
    {
        public static Color ColorPicker(string color)
        {
            switch (color)
            {
                case "Red": return Color.Red;
                case "Red_Orange": return Color.OrangeRed;
                case "Blue": return Color.Blue;
                case "Orange": return Color.Orange;
                case "Light_Blue": return Color.LightBlue;
                case "Dark_Blue": return Color.DarkBlue;
                case "Yellow": return Color.Yellow;
                case "Light_Yellow": return Color.LightYellow;
                case "Green": return Color.Green;
                case "Light_Green": return Color.LightGreen;
                case "Dark_Green": return Color.DarkGreen;
                case "Cyan": return Color.Cyan;
                case "Gray": return Color.Gray;
                case "Light_Gray": return Color.LightGray;
                case "Dark_Gray": return Color.DarkGray;
                case "Purple": return Color.Violet;
                case "Dark_Purple": return Color.DarkViolet;
                case "White": return Color.White;
                case "Black": return Color.Black;
                case "Dark_Brown": return Color.DarkKhaki;
                case "Brown": return Color.Brown;
                case "Pale": return Color.LightPink;
                case "Pink": return Color.Pink;
            }
            return Color.Black;
        }
    }
}
