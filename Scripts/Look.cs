﻿using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Look
    {
        public static Player player;
        public static int x { get; set; }
        public static int y { get; set; }
        public static bool looking = false;
        public static RLColor savedColor = RLColor.Black;
        public Look(RLRootConsole _rootConsole) { player = Program.player; }
        public static void StartLooking(int _x, int _y) { x = _x; y = _y; looking = true; player.turnActive = false; savedColor = RLColor.Black; Move(0, 0); RLKey key = RLKey.Unknown; Action.LookAction(key); }
        public static void StopLooking() { player.turnActive = true; looking = false; ChangeBackColor(x, y, savedColor); RLKey key = RLKey.Unknown; Action.PlayerAction(player, key); Log.ClearLogDisplay(); }
        public static void Move(int _x, int _y)
        {
            if (Map.map[x + _x, y + _y].visible)
            {
                ChangeBackColor(x, y, savedColor);
                x += _x; y += _y;
                if (Map.map[x, y].actor != null) { Log.AddToStoredLog(Map.map[x, y].actor.Describe(), true); savedColor = Map.map[x, y].actor.bColor; }
                else if (Map.map[x, y].item != null) { Log.AddToStoredLog(Map.map[x, y].item.Describe(), true); savedColor = Map.map[x, y].item.bColor; }
                else { Log.AddToStoredLog(Map.map[x, y].Describe(), true); savedColor = Map.map[x, y].bColor; }
                ChangeBackColor(x, y, RLColor.White);
            }
            else { Log.AddToStoredLog("You cannot see there."); Log.DisplayLog(); }
        }
        public static void ChangeBackColor(int _x, int _y, RLColor color)
        {
            if (Map.map[_x, _y].actor != null) { Map.map[_x, _y].actor.bColor = color; }
            else if (Map.map[_x, _y].item != null) { Map.map[_x, _y].item.bColor = color; }
            else { Map.map[_x, _y].bColor = color; }
        }
    }
}