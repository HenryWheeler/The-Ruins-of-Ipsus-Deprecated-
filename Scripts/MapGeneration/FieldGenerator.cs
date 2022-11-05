using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheRuinsOfIpsus
{
    public class FieldGenerator: AGenerator
    {
        public void CreateMap(int _mapWidth, int _mapHeight)
        {
            mapWidth = _mapWidth; mapHeight = _mapHeight;
            Map.outside = true;

            SetAllWalls();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
                    {
                        int probability = CMath.seed.Next(0, 100);
                        if (probability > 80) { SetTile(x, y, '"', "Grass", "Soft Green Grass.", "Dark_Green", "Black", false, 1); }
                        else if (probability > 60) { SetTile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
                        else if (probability == 1) { CreateTreePatch(x, y); }
                        else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }

                        probability = CMath.seed.Next(0, 3000);
                        if (probability == 1500) { CreatePond(x, y); }
                        else if (probability == 2000) { CreateBezierCurve(x, 1, 1, y); }
                        else if (probability < 500 && probability > 495) { SetTile(x, y, '*', "Rock", "A solid hunk of granite.", "Light_Gray", "Dark_Gray", false, 0); }

                        if (Map.map[x, y].GetComponent<Description>().name == "Stone Wall") { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
                    }
                }
            }

            CreateSurroundingWalls();
        }
        public override void SetAllWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SetTile(x, y, '#', "Stone Wall", "A cold stone wall.", "White", "Gray", true, 0);
                }
            }
        }
        public void CreateTreePatch(int _x, int _y)
        {
            int size = CMath.seed.Next(1, 4);

            for (int x = _x - size; x < _x + size; x++)
            {
                for (int y = _y - size; y < _y + size; y++)
                {
                    if (CMath.CheckBounds(x, y))
                    {
                        if (CMath.seed.Next(1, 100) > 50) { SetTile(x, y, (char)20, "Oak Tree", "A solid sturdy oak.", "Dark_Green", "Black", true, 0); }
                    }
                }
            }
        }
        public void CreatePond(int _x, int _y)
        {
            int size = CMath.seed.Next(3, 12);
            for (int x = _x - size; x < _x + size; x++)
            {
                for (int y = _y - size; y < _y + size; y++)
                {
                    if (CMath.CheckBounds(x, y) && CMath.seed.Next(0, 100) > 50)
                    {
                        if (CMath.seed.Next(0, 100) < 50) { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Dark_Blue", false, 2); }
                        else { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
                    }
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                for (int x = _x - size; x < _x + size; x++)
                {
                    for (int y = _y - size; y < _y + size; y++)
                    {
                        if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                        {
                            if (CMath.CheckBounds(x, y))
                            {
                                int water = WaterCount(x, y);

                                if (water > 4)
                                {
                                    if (CMath.seed.Next(0, 100) < 50) { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Dark_Blue", false, 2); }
                                    else { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
                                }
                                else if (water < 4)
                                {
                                    int probability = CMath.seed.Next(0, 100);
                                    if (probability > 80) { SetTile(x, y, '"', "Grass", "Soft Green Grass.", "Dark_Green", "Black", false, 1); }
                                    else if (probability > 60) { SetTile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
                                    else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static int WaterCount(int sX, int sY)
        {
            int walls = 0;

            for (int x = sX - 1; x <= sX + 1; x++)
            {
                for (int y = sY - 1; y <= sY + 1; y++)
                {
                    if (x != sX || y != sY) { if (CMath.CheckBounds(x, y) && Map.map[x, y].moveType == 2) { walls++; } }
                }
            }

            return walls;
        }
        public override void CreateDiagonalPassage(int r1x, int r1y, int r2x, int r2y)
        {
            int t;
            int x = r1x; int y = r1y;
            int delta_x = r2x - r1x; int delta_y = r2y - r1y;
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
                    if (Map.map[x, y].moveType == 0)
                    {
                        SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                        SetTile(x + 1, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                    }
                    if (x == r2x && y == r2y) { hasConnected = true; }
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
                    if (Map.map[x, y].moveType == 0)
                    {
                        SetTile(x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                        SetTile(x, y + 1, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                    }
                    if (x == r2x && y == r2y) { hasConnected = true; }
                }
                while (!hasConnected);
            }
        }
        public override void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y)
        {
            int r1x; int r1y;

            r1x = CMath.seed.Next(1, 80);
            r1y = CMath.seed.Next(1, 70);

            for (float t = 0; t < 1; t += .001f)
            {
                int x = (int)((1 - t) * ((1 - t) * r0x + t * r1x) + t * ((1 - t) * r0x + t * r2x));
                int y = (int)((1 - t) * ((1 - t) * r0y + t * r1y) + t * ((1 - t) * r0y + t * r2y));
                if (CMath.CheckBounds(x, y))
                {
                    if (CMath.seed.Next(0, 100) < 50) { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Dark_Blue", false, 2); }
                    else { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
                }
            }
        }
        public override void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y)
        {
            if (CMath.seed.Next(0, 1) == 0)
            {
                for (int x = Math.Min(r1x, r2x); x <= Math.Max(r1x, r2x); x++)
                {
                    SetTile(x, r1y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                }
                for (int y = Math.Min(r1y, r2y); y <= Math.Max(r1y, r2y); y++)
                {
                    SetTile(r2x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                }
            }
            else
            {
                for (int y = Math.Min(r1y, r2y); y <= Math.Max(r1y, r2y); y++)
                {
                    SetTile(r1x, y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                }
                for (int x = Math.Min(r1x, r2x); x <= Math.Max(r1x, r2x); x++)
                {
                    SetTile(x, r2y, '.', "Stone Floor", "A simple stone floor.", "Brown", "Black", false, 1);
                }
            }
        }
    }
}
