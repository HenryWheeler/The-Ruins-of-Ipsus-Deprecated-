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
        public static double Distance(Coordinate one, Coordinate two)
        {
            int returnValue = (int)Math.Sqrt(Math.Pow(two.x - one.x, 2) + Math.Pow(two.y - one.y, 2));
            if (returnValue == 0) { return 1; }
            else return (int)Math.Sqrt(Math.Pow(two.x - one.x, 2) + Math.Pow(two.y - one.y, 2));
        }
        public static bool PathBlocked(Coordinate coordinate, Coordinate coordinate2, int range)
        {
            int oX = coordinate.x; int oY = coordinate.y;
            int eX = coordinate2.x; int eY = coordinate2.y;
            int t;
            int x = oX; int y = oY;
            int delta_x = eX - oX; int delta_y = eY - oY;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);

            if (oX == eX && oY == eY) return true;
            if (Math.Abs(delta_x) > range || Math.Abs(delta_y) > range) return false;

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
                while (Map.map[x, y].actor == null && Map.map[x, y].moveType != 0);
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
                while (!Map.map[x, y].GetComponent<Visibility>().opaque);
                return false;
            }
        }
        public static List<Coordinate> ReturnLine(Coordinate origin, Coordinate target)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int t;
            int x = origin.x; int y = origin.y;
            int delta_x = target.x - origin.x; int delta_y = target.y - origin.y;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);
            bool hasConnected = false;

            if (abs_delta_x > abs_delta_y)
            {
                t = abs_delta_y * 2 - abs_delta_x;
                do
                {
                    if (t >= 0) { y += sign_y; t -= abs_delta_x * 2; }
                    x += sign_x;
                    t += abs_delta_y * 2;
                    coordinates.Add(new Coordinate(x, y));
                    if (x == target.x && y == target.y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
            else
            {
                t = abs_delta_x * 2 - abs_delta_y;
                do
                {
                    if (t >= 0) { x += sign_x; t -= abs_delta_y * 2; }
                    y += sign_y;
                    t += abs_delta_x * 2;
                    coordinates.Add(new Coordinate(x, y));
                    if (x == target.x && y == target.y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
            return coordinates;
        }
        public static AI ReturnAI(Entity entityRef)
        {
            foreach (Component component in entityRef.components)
            {
                if (component.GetType().BaseType.Equals(typeof(AI))) { return (AI)component; }
            }
            return null;
        }
        public static void DisplayToConsole(RLConsole console, string logOut, int a, int b, int m = 0, int y = 2)
        {
            string[] outPut = logOut.Split(' ');
            int c = a;
            foreach (string text in outPut)
            {
                string[] split = text.Split('*');
                if (split.Count() == 1)
                {
                    if (split[0].Contains("+")) { y += 2 + m; c = a; }
                    else
                    {
                        if (c + split[0].Length > console.Width - 4) { y += 2 + m; c = a; }
                        console.Print(c + b, y, split[0], RLColor.White);
                        c += split[0].Length + 1;
                    }
                }
                else
                {
                    if (split[1].Contains("+")) { y += 2 + m; c = a; }
                    else
                    {
                        if (c + split[0].Length > console.Width - 4) { y += 2 + m; c = a; }
                        console.Print(c + b, y, split[1], ColorFinder.ColorPicker(split[0]));
                        c += split[1].Length + 1;
                    }
                }
            }
        }
        public static bool CheckBounds(int x, int y)
        {
            if (x <= 0 || x >= Program.gameMapWidth || y <= 0 || y >= Program.gameMapHeight) return false;
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
