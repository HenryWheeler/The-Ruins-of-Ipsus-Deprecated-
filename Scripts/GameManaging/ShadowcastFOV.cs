using System;
using RLNET;
using System.Collections.Generic;

namespace TheRuinsOfIpsus
{
    public struct Slope
    {
        public Slope(int y, int x) { Y = y; X = x; }
        public readonly int Y, X;
    }
    public static class ShadowcastFOV
    {
        public static List<Coordinate> visibleTiles = new List<Coordinate>();
        public static void Compute(Vector2 vector2, int rangeLimit)
        {
            SetVisible(vector2, true);
            for (uint octant = 0; octant < 8; octant++) 
            {
                Compute(octant, vector2.x, vector2.y, rangeLimit, 1, new Slope(1, 1), new Slope(0, 1)); 
            }
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
                        case 0: tx += x; ty -= y; break; case 1: tx += y; ty -= x; break;
                        case 2: tx -= y; ty -= x; break; case 3: tx -= x; ty -= y; break;
                        case 4: tx -= x; ty += y; break; case 5: tx -= y; ty += x; break;
                        case 6: tx += y; ty += x; break; case 7: tx += x; ty += y; break;
                    }

                    bool inRange = rangeLimit < 0 || CMath.Distance(oX, oY, tx, ty) <= rangeLimit;
                    if (inRange && (y != topY || top.Y*x >= top.X*y) && (y != bottomY || bottom.Y*x <= bottom.X*y))
                    {
                        //if (aiUse) 
                        //{
                        //    Traversable traversable = World.GetTraversable(new Vector2(tx, ty));
                        //    if (CMath.CheckBounds(tx, ty) && traversable.actorLayer != null && traversable.actorLayer != ai.entity)
                        //    {
                        //        if (ai.ReturnHatred(traversable.actorLayer) > 0)
                        //        { ai.referenceTarget = traversable.actorLayer; ai.mood = "Red*Angry"; return; }
                        //        else
                        //        {
                        //            if (ai.entity.GetComponent<Stats>().acuity <= 10)
                        //            {
                        //                foreach (string status in traversable.actorLayer.GetComponent<OnHit>().statusEffects)
                        //                {
                        //                    if (ai.hatedEntities.Contains(status)) 
                        //                    { ai.referenceTarget = traversable.actorLayer; ai.mood = "Red*Angry"; return; }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        SetVisible(new Vector2(tx, ty), true);
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
        public static void ClearSight()
        {
            foreach (Coordinate coordinate in visibleTiles) { SetVisible(coordinate.vector2, false); }
            ClearList();
        }
        public static void ClearList() { visibleTiles.Clear(); }
        public static void SetVisible(Vector2 vector2, bool visible, bool all = false)
        {
            if (CMath.CheckBounds(vector2.x, vector2.y))
            {
                if (visible)
                {
                    World.tiles[vector2.x, vector2.y].GetComponent<Visibility>().SetVisible(true);
                    if (!all) { visibleTiles.Add(new Coordinate(vector2)); }
                }
                else
                {
                    World.tiles[vector2.x, vector2.y].GetComponent<Visibility>().SetVisible(false);
                }
            }
        }
        public static bool BlocksLight(Vector2 vector2)
        {
            if (CMath.CheckBounds(vector2.x, vector2.y))
            {
                if (World.tiles[vector2.x, vector2.y].GetComponent<Visibility>().opaque) { return true; }
                return false;
            }
            return true;
        }
    }
}
