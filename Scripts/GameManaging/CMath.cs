﻿using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class CMath
    {
        public static double Distance(int oX, int oY, int eX, int eY) { return Math.Sqrt(Math.Pow(eX - oX, 2) + Math.Pow(eY - oY, 2)); }
        public static int Distance(Coordinate one, Coordinate two)
        {
            Vector2 origin = one.vector2; Vector2 destination = two.vector2;
            int returnValue = (int)Math.Sqrt(Math.Pow(destination.x - origin.x, 2) + Math.Pow(destination.y - origin.y, 2));
            if (returnValue == 0) { return 1; }
            else { return returnValue; }
        }
        public static bool PathBlocked(Vector2 coordinate, Vector2 coordinate2, int range)
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
                while (World.GetTraversable(new Vector2(x, y)).actorLayer == null && World.GetTraversable(new Vector2(x, y)).terrainType != 0);
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
                while (World.GetTraversable(new Vector2(x, y)).actorLayer == null && World.GetTraversable(new Vector2(x, y)).terrainType != 0);
                return false;
            }
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
            console.Clear();
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
            Renderer.CreateConsoleBorder(console);
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
