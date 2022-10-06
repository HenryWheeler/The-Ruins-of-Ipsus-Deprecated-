using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public static class CMath
    {
        public static int Distance(int oX, int oY, int eX, int eY) { return ((oX - eX) * (oX - eX)) + ((oY - eY) * (oY - eY)); }
        public static bool Sight(int oX, int oY, int eX, int eY, int sight)
        {
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
                while (Map.map[x, y].walkable == true);
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
                while (Map.map[x, y].walkable == true);
                return false;
            }
        }
    }
}
