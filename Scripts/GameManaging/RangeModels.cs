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
        public static List<Coordinate> FindModel(Coordinate startingPoint, Coordinate target, int size, int range, bool includeStart, string type)
        {
            switch (type)
            {
                case "SingleTarget":
                    {
                        return new List<Coordinate>() { target };
                    }
                case "Line":
                    {
                        return BeamRangeModel(startingPoint, target, range, includeStart);
                    }
                case "Cone":
                    {
                        return ConeRangeModel(startingPoint, target, size, range);
                    }
                case "Sphere":
                    {
                        return SphereRangeModel(target, size, includeStart);
                    }
                case "Cube":
                    {
                        break;
                    }
            }
            return null;
        }
        public static List<Coordinate> SphereRangeModel(Coordinate startingPoint, int size, bool includeStart)
        {
            Vector2 vector = startingPoint.vector2;
            ClearCoordinates();
            for (uint octant = 0; octant < 8; octant++)
            {
                Compute(octant, vector.x, vector.y, size, 1, new Slope(1, 1), new Slope(0, 1));
            }
            if (includeStart) { returnCoordinates.Add(startingPoint); }
            return returnCoordinates;
        }
        public static List<Coordinate> ConeRangeModel(Coordinate startingPoint, Coordinate target, int size, int range)
        {
            Vector2 t = startingPoint.vector2;
            Vector2 o = target.vector2;
            List<Coordinate> coordinates = new List<Coordinate>();

            if (CMath.Distance(startingPoint, target) >= range) { return new List<Coordinate>(); }

            double distanceChange = CMath.Distance(t.x, t.y, o.x, o.y) / 3;

            double sideA = CMath.Distance(t.x, t.y, o.x, o.y);
            double sideB = size;
            double sideC = Math.Sqrt(sideA * sideA + sideB * sideB);

            double s;
            double area;
            double y;
            double x;
            double y2;
            double x2;

            if (t.x < o.x + distanceChange && t.x > o.x - distanceChange && o.y > t.y)
            {
                //South

                sideC /= 2;

                //s = (sideA + sideB + sideC) / 2;
                //area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = 0;
                x = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                //s = (-sideA + sideB + sideC) / 2;
                //area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = 0;
                x2 = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                y += o.y; x += o.x + 1;
                y2 += o.y; x2 += o.x;
            }
            else if (t.x < o.x + distanceChange && t.x > o.x - distanceChange)
            {
                //North

                sideC /= 2;

                //s = (sideA + sideB + sideC) / 2;
                //area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = 0;
                x = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                //s = (-sideA + sideB + sideC) / 2;
                //area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = 0;
                x2 = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                y += o.y; x += o.x + 1;
                y2 += o.y; x2 += o.x;
            }
            else if (t.y < o.y + distanceChange && t.y > o.y - distanceChange && o.x > t.x)
            {
                //East

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = 0;

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = 0;

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }
            else if (t.y < o.y + distanceChange && t.y > o.y - distanceChange)
            {
                //West;

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = 0;

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = 0;

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }
            else if (o.x > t.x && o.y < t.y)
            {
                sideC /= 2;

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y2, 2), 0.5f);

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }
            else if (o.x < t.x && o.y > t.y)
            {
                sideC /= 2;

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y2, 2), 0.5f);

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }
            else if (o.x > t.x && o.y > t.y)
            {
                sideC /= 2;

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y2, 2), 0.5f);

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }
            else
            {
                sideC /= 2;

                s = (sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s - sideA) * (s - sideB) * (s - sideC), 0.5f);

                y = (2 * area / sideA);
                x = -Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y, 2), 0.5f);

                s = (-sideA + sideB + sideC) / 2;
                area = Math.Pow(s * (s + sideA) * (s - sideB) * (s - sideC), 0.5f);

                y2 = (2 * area / -sideA);
                x2 = Math.Pow(Math.Pow(sideC, 2) - Math.Pow(y2, 2), 0.5f);

                y += o.y; x += o.x;
                y2 += o.y; x2 += o.x;
            }


            foreach (Coordinate coordinate in ReturnLine(new Coordinate((int)x, (int)y), new Coordinate((int)x2, (int)y2), false, true))
            {
                if (coordinate != null && CMath.CheckBounds(coordinate.vector2.x, coordinate.vector2.y))
                {
                    //coordinates.Add(coordinate);

                    foreach (Coordinate coordinate2 in ReturnLine(startingPoint, coordinate, true))
                    {
                        if (coordinate2 != null && CMath.CheckBounds(coordinate2.vector2.x, coordinate2.vector2.y))
                        {
                            coordinates.Add(coordinate2);

                            foreach (Coordinate coordinate3 in ReturnLine(coordinate2, coordinate))
                            {
                                if (coordinate3 != null && CMath.CheckBounds(coordinate3.vector2.x, coordinate3.vector2.y))
                                {
                                    coordinates.Add(coordinate3);
                                }
                            }
                        }
                    }
                }
            }

            return coordinates;
        }
        public static List<Coordinate> BeamRangeModel(Coordinate startingPoint, Coordinate target, int range, bool includeStart)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            if (CMath.Distance(startingPoint, target) >= range) { return new List<Coordinate>(); }
            foreach (Coordinate coordinate3 in ReturnLine(startingPoint, target))
            {
                if (coordinate3 != null && CMath.CheckBounds(coordinate3.vector2.x, coordinate3.vector2.y))
                {
                    coordinates.Add(coordinate3);
                }
            }
            if (!includeStart) { coordinates.Remove(startingPoint); }
            else { coordinates.Add(startingPoint); }
            return coordinates;
        }
        public static void ClearCoordinates() { returnCoordinates.Clear(); }
        public static List<Coordinate> ReturnLine(Coordinate origin, Coordinate target, bool returnOnWall = false, bool includeStart = false)
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            if (includeStart)
            {
                coordinates.Add(origin);
            }

            int t;
            int x = origin.vector2.x; int y = origin.vector2.y;
            int delta_x = target.vector2.x - origin.vector2.x; int delta_y = target.vector2.y - origin.vector2.y;
            int abs_delta_x = Math.Abs(delta_x); int abs_delta_y = Math.Abs(delta_y);
            int sign_x = Math.Sign(delta_x); int sign_y = Math.Sign(delta_y);
            bool hasConnected = false;

            if (returnOnWall)
            {
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

                        else if (World.tiles[x, y].obstacleLayer != null && World.tiles[x, y].obstacleLayer.GetComponent<Visibility>() != null && World.tiles[x, y].obstacleLayer.GetComponent<Visibility>().opaque)
                        {
                            return coordinates;
                        }
                        if (World.tiles[x, y].terrainType == 0)
                        {
                            return coordinates;
                        }
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

                        else if (World.tiles[x, y].obstacleLayer != null && World.tiles[x, y].obstacleLayer.GetComponent<Visibility>() != null && World.tiles[x, y].obstacleLayer.GetComponent<Visibility>().opaque)
                        {
                            return coordinates;
                        }
                        if (World.tiles[x, y].terrainType == 0)
                        {
                            return coordinates;
                        }
                    }
                    while (!hasConnected);
                }
            }
            else
            {
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
            }

            return coordinates;
        }
        static void Compute(uint octant, int oX, int oY, int rangeLimit, int x, Slope top, Slope bottom)
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
                                else { Compute(octant, oX, oY, rangeLimit, x + 1, top, newBottom); }
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
