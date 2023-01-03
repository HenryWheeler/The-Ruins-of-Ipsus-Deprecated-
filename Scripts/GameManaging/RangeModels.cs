using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheRuinsOfIpsus.ShadowcastFOV;

namespace TheRuinsOfIpsus
{
    public class RangeModels
    {
        private static List<Coordinate> returnCoordinates = new List<Coordinate>();
        public static List<Coordinate> SphereRangeModel(Coordinate startingPoint, int size, bool includeStart)
        {
            Vector2 vector = startingPoint.vector2;
            ClearCoordinates();
            for (uint octant = 0; octant < 8; octant++)
            {
                ComputeSphere(octant, vector.x, vector.y, size, 1, new Slope(1, 1), new Slope(0, 1));
            }
            if (includeStart) { returnCoordinates.Add(startingPoint); }
            return returnCoordinates;
        }
        public static List<Coordinate> BeamRangeModel(Coordinate startingPoint, Coordinate target, int range, bool includeStart)
        {
            ClearCoordinates();
            if (CMath.Distance(startingPoint, target) > range) { return null; }
            returnCoordinates = ReturnLine(startingPoint, target);
            if (!includeStart) { returnCoordinates.Remove(startingPoint); }
            return returnCoordinates;
        }
        public static void ClearCoordinates() { returnCoordinates.Clear(); }
        public static List<Coordinate> ReturnLine(Coordinate origin, Coordinate target)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            int t;
            int x = origin.vector2.x; int y = origin.vector2.y;
            int delta_x = target.vector2.x - origin.vector2.x; int delta_y = target.vector2.y - origin.vector2.y;
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
                    if (x == target.vector2.x && y == target.vector2.y) { hasConnected = true; }
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
                    if (x == target.vector2.x && y == target.vector2.y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
            return coordinates;
        }
        static void ComputeSphere(uint octant, int oX, int oY, int rangeLimit, int x, Slope top, Slope bottom)
        {
            for (; (uint)x <= (uint)rangeLimit; x++)
            {
                int topY = top.X == 1 ? x : ((x * 2 + 1) * top.Y + top.X - 1) / (top.X * 2);
                int bottomY = bottom.Y == 0 ? 0 : ((x * 2 - 1) * bottom.Y + bottom.X) / (bottom.X * 2);

                int wasOpaque = -1;
                for (int y = topY; y >= bottomY; y--)
                {
                    int tx = oX, ty = oY;
                    switch (octant)
                    {
                        case 0: tx += x; ty -= y; break;
                        case 1: tx += y; ty -= x; break;
                        case 2: tx -= y; ty -= x; break;
                        case 3: tx -= x; ty -= y; break;
                        case 4: tx -= x; ty += y; break;
                        case 5: tx -= y; ty += x; break;
                        case 6: tx += y; ty += x; break;
                        case 7: tx += x; ty += y; break;
                    }

                    bool inRange = rangeLimit < 0 || CMath.Distance(oX, oY, tx, ty) <= rangeLimit;
                    if (CMath.CheckBounds(tx, ty) && inRange && (y != topY || top.Y * x >= top.X * y) && (y != bottomY || bottom.Y * x <= bottom.X * y))
                    {
                        returnCoordinates.Add(new Coordinate(tx, ty));
                    }

                    bool isOpaque = !inRange || BlocksLight(new Vector2(tx, ty));
                    if (x != rangeLimit)
                    {
                        if (isOpaque)
                        {
                            if (wasOpaque == 0)
                            {
                                Slope newBottom = new Slope(y * 2 + 1, x * 2 - 1);
                                if (!inRange || y == bottomY) { bottom = newBottom; break; }
                                else { ComputeSphere(octant, oX, oY, rangeLimit, x + 1, top, newBottom); }
                            }
                            wasOpaque = 1;
                        }
                        else
                        {
                            if (wasOpaque > 0) top = new Slope(y * 2 + 1, x * 2 + 1);
                            wasOpaque = 0;
                        }
                    }
                }
                if (wasOpaque != 0) break;
            }
        }
    }
}
