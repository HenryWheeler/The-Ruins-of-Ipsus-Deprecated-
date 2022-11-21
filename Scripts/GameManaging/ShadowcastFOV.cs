using System;
using RLNET;
using System.Collections.Generic;

namespace TheRuinsOfIpsus
{
    public static class ShadowcastFOV
    {
        public static List<Coordinate> visibleTiles = new List<Coordinate>();
        public static void Compute(int x, int y, int rangeLimit, AI ai = null, bool aiUse = false)
        {
            //ClearList();
            if (!aiUse) { SetVisible(x, y, true); }
            if (Map.outside && !aiUse) { for (uint octant = 0; octant < 8; octant++) Compute(octant, x, y, 75, 1, new Slope(1, 1), new Slope(0, 1), aiUse, ai); }
            else for (uint octant = 0; octant < 8; octant++) Compute(octant, x, y, rangeLimit, 1, new Slope(1, 1), new Slope(0, 1), aiUse, ai);
        }
        struct Slope 
        {
            public Slope(int y, int x) { Y = y; X = x; }
            public readonly int Y, X;
        }
        static void Compute(uint octant, int oX, int oY, int rangeLimit, int x, Slope top, Slope bottom, bool aiUse, AI ai = null)
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
                        case 0: tx += x; ty -= y; break; case 1: tx += y; ty -= x; break;
                        case 2: tx -= y; ty -= x; break; case 3: tx -= x; ty -= y; break;
                        case 4: tx -= x; ty += y; break; case 5: tx -= y; ty += x; break;
                        case 6: tx += y; ty += x; break; case 7: tx += x; ty += y; break;
                    }

                    bool inRange = rangeLimit < 0 || CMath.Distance(oX, oY, tx, ty) <= rangeLimit;
                    if(inRange && (y != topY || top.Y*x >= top.X*y) && (y != bottomY || bottom.Y*x <= bottom.X*y))
                    {
                        if (aiUse) 
                        {
                            if (CMath.CheckBounds(tx, ty) && Map.map[tx, ty].actor != null && Map.map[tx, ty].actor != ai.entity)
                            {
                                if (ai.ReturnHatred(Map.map[tx, ty].actor) > Math.Abs(ai.ReturnConviction(Map.map[tx, ty].actor, 1)))
                                { ai.referenceTarget =  Map.map[tx, ty].actor; ai.mood = "Angry"; return; }
                                else
                                {
                                    if (Map.map[tx, ty].actor.GetComponent<Stats>() != null)
                                    {
                                        foreach (string status in Map.map[tx, ty].actor.GetComponent<Stats>().status)
                                        {
                                            if (ai.hatedEntities.Contains(status)) 
                                            { ai.referenceTarget = Map.map[tx, ty].actor; ai.mood = "Angry"; return; }
                                        }
                                    }
                                }
                            }
                        }
                        else if (!aiUse) { SetVisible(tx, ty, true); }
                    }

                    bool isOpaque = !inRange || BlocksLight(tx, ty);
                    if (x != rangeLimit)
                    {
                        if (isOpaque)
                        {
                            if (wasOpaque == 0)
                            {
                                Slope newBottom = new Slope(y * 2 + 1, x * 2 - 1);
                                if (!inRange || y == bottomY) { bottom = newBottom; break; }
                                else { Compute(octant, oX, oY, rangeLimit, x + 1, top, newBottom, aiUse, ai); }
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
        public static void ClearSight()
        {
            foreach (Coordinate coordinate in visibleTiles) { SetVisible(coordinate.x, coordinate.y, false); }
            ClearList();
        }
        public static void ClearList() { visibleTiles.Clear(); }
        public static void SetVisible(int x, int y, bool visible, bool all = false)
        {
            if (CMath.CheckBounds(x, y))
            {
                if (visible)
                {
                    Map.map[x, y].GetComponent<Visibility>().SetVisible(true);
                    if (!all) { visibleTiles.Add(new Coordinate(x, y)); }
                    if (Map.map[x, y].item != null) { Map.map[x, y].item.GetComponent<Visibility>().SetVisible(true); }
                    if (Map.map[x, y].actor != null) { Map.map[x, y].actor.GetComponent<Visibility>().SetVisible(true); }
                    if (Map.map[x, y].terrain != null) { Map.map[x, y].terrain.GetComponent<Visibility>().SetVisible(true); }
                }
                else
                {
                    Map.map[x, y].GetComponent<Visibility>().SetVisible(false);
                    if (Map.map[x, y].item != null) { Map.map[x, y].item.GetComponent<Visibility>().SetVisible(false); }
                    if (Map.map[x, y].actor != null) { Map.map[x, y].actor.GetComponent<Visibility>().SetVisible(false); }
                    if (Map.map[x, y].terrain != null) { Map.map[x, y].terrain.GetComponent<Visibility>().SetVisible(false); }
                }
            }
        }
        public static bool BlocksLight(int x, int y)
        {
            if (CMath.CheckBounds(x, y))
            {
                if (Map.map[x, y].GetComponent<Visibility>().opaque) { return true; }
                else if (Map.map[x, y].actor != null && Map.map[x, y].actor.GetComponent<Visibility>().opaque) { return true; }
                else if (Map.map[x, y].item != null && Map.map[x, y].item.GetComponent<Visibility>().opaque) { return true; }
                return false;
            }
            return true;
        }
    }
}
