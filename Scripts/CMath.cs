using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class CMath
    {
        public static Random seed { get; set; }
        public static Random random { get; set; }
        public static int seedInt { get; set; }
        public CMath(int _seed) { seed = new Random(_seed); random = new Random(); seedInt = _seed; }
        public static double Distance(int oX, int oY, int eX, int eY) { return Math.Sqrt(Math.Pow(eX - oX, 2) + Math.Pow(eY - oY, 2)); }
        public static bool Sight(int oX, int oY, int eX, int eY, int sight)
        {
            if (Map.outside) { sight = sight * 2; }
            int t;
            int x = oX; int y = oY;
            int delta_x = eX - oX; int delta_y = eY - oY;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);

            if (oX == eX && oY == eY) return true;
            if (Math.Abs(delta_x) > sight || Math.Abs(delta_y) > sight) return false;

            if (abs_delta_x > abs_delta_y)
            {
                t = abs_delta_y * 2 - abs_delta_x;
                do
                {
                    if (t >= 0) { y += sign_y; t -= abs_delta_x * 2; }
                    x += sign_x;
                    t += abs_delta_y * 2;
                    if (x == eX && y == eY) { return true; }
                }
                while (!Map.map[x, y].opaque);
                return false;
            }
            else
            {
                t = abs_delta_x * 2 - abs_delta_y;
                do
                {
                    if (t >= 0) { x += sign_x; t -= abs_delta_y * 2; }
                    y += sign_y;
                    t += abs_delta_x * 2;
                    if (x == eX && y == eY) { return true; }
                }
                while (!Map.map[x, y].opaque);
                return false;
            }
        }
        public static void DisplayToConsole(RLConsole console, string logOut, int a, int b, int m = 0, int y = 2)
        {
            string[] outPut = logOut.Split(' ');
            int c = 0;
            foreach (string text in outPut)
            {
                if (text.Contains("+")) { y += 2 + m; c = a; }
                else
                {
                    if (c + text.Length > console.Width - 4) { y += 2 + m; c = a; }
                    console.Print(c + b, y, text, RLColor.White);
                    c += text.Length + 1;
                }
            }
        }
        public static bool CheckBounds(int x, int y)
        {
            if (x <= 0 || x >= 79 || y <= 0 || y >= 69) return false;
            else return true;
        }
        public static void ClearConsole(RLConsole console)
        {
            int h = console.Height - 2;
            int w = console.Width - 2;
            for (int y = (h); y >= 2; y--)
            {
                for (int x = 1; x < w + 1; x++)
                {
                    console.SetColor(x, y, RLColor.Black);
                }
            }
        }
    }
}
