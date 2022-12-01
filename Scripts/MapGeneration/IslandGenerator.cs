﻿using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheRuinsOfIpsus
{
    public class IslandGenerator: AGenerator
    {
        private int wallsNeeded = 4;
        private int randomFill;
        private int smooth = 5;
        public void CreateMap(int _mapWidth, int _mapHeight)
        {
            mapWidth = _mapWidth; mapHeight = _mapHeight;
            Map.outside = true;
            randomFill = CMath.seed.Next(46, 52);
            SetAllWalls();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        if (CMath.seed.Next(0, 100) < randomFill)
                        {
                            int probability = CMath.seed.Next(0, 100);
                            if (probability > 60) { SetTile(x, y, '"', "Grass", "Soft Green Grass.", "Dark_Green", "Black", false, 1); }
                            else if (probability > 20) { SetTile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
                            else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }

                        }
                    }
                }
            }
            for (int z = 0; z < smooth; z++) { SmoothMap(); }
            SetSandyShore();
        }
        public void SetSandyShore()
        {
            foreach (Tile tile in Map.map)
            {
                if (tile != null && tile.moveType == 1)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    if (WaterCount(coordinate.x, coordinate.y) != 0) 
                    {
                        if (CMath.seed.Next(0, 100) < 50) { SetTile(coordinate.x, coordinate.y, (char)176, "Sandy Shore", "A grainy shore of white sand.", "Light_Gray", "Dark_Yellow", false, 1); }
                        else { SetTile(coordinate.x, coordinate.y, (char)176, "Sandy Shore", "A grainy shore of white sand.", "Light_Gray", "Dark_Yellow_Gray", false, 1); } }
                }
            }
        }
        public override void SetAllWalls()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (CMath.seed.Next(0, 100) < 50) { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Dark_Blue", false, 2); }
                    else { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
                }
            }
        }
        public void SmoothMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (x > 1 && x < mapWidth - 2 && y > 1 && y < mapHeight - 2)
                    {
                        int walls = WaterCount(x, y);

                        if (walls > wallsNeeded)
                        {
                            if (CMath.seed.Next(0, 100) < 50) { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Dark_Blue", false, 2); }
                            else { SetTile(x, y, (char)247, "Water", "A murky pool.", "Light_Blue", "Blue", false, 2); }
                        }
                        else if (walls < wallsNeeded)
                        {
                            int probability = CMath.seed.Next(0, 100);
                            if (probability > 60) { SetTile(x, y, '"', "Grass", "Soft Green Grass.", "Dark_Green", "Black", false, 1); }
                            else if (probability > 20) { SetTile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
                            else { SetTile(x, y, '.', "Bare Ground", "The bare dirt ground.", "Brown", "Black", false, 1); }
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
        { }
        public override void CreateBezierCurve(int r0x, int r0y, int r2x, int r2y)
        { }
        public override void CreateStraightPassage(int r1x, int r1y, int r2x, int r2y)
        { }
    }
}