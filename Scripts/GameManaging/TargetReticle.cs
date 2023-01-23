﻿using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class TargetReticle
    {
        public static int x;
        public static int y;
        public static bool targeting = false;
        public static bool inInventory { get; set; }
        private static Entity player;
        private static List<Vector2> sfxPositions = new List<Vector2>();
        public TargetReticle(Entity _player) { player = _player; }
        public static void StartTargeting(bool _inInventory, bool throwing)
        {
            inInventory = _inInventory;
            if (inInventory) { InventoryManager.CloseInventory(); }
            Vector2 vector3 = player.GetComponent<Coordinate>().vector2;
            x = vector3.x; y = vector3.y;
            targeting = true; player.GetComponent<TurnFunction>().turnActive = false;
            Move(0, 0);
            if (throwing) { Action.throwing = true; }
            else { Action.throwing = false; }
            RLKey key = RLKey.Unknown; Action.TargetAction(player, key);
        }
        private static void ClearSFXPositions()
        {
            foreach (Vector2 position in sfxPositions) { World.tiles[position.x, position.y].sfxLayer = null; }
            sfxPositions.Clear();
        }
        public static void StopTargeting()
        {
            targeting = false; player.GetComponent<TurnFunction>().turnActive = true;
            ClearSFXPositions();
            Renderer.MoveCamera(player.GetComponent<Coordinate>().vector2);
            if (!inInventory) { Action.PlayerAction(player); Log.DisplayLog(); }
            else { InventoryManager.OpenInventory(); }
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(x + _x, y + _y))
            {
                ClearSFXPositions();
                x += _x; y += _y;
                if (Action.throwing)
                {
                    bool check = CreateLine(player.GetComponent<Stats>().strength + 10, true);

                    List<OnThrowProperty> properties = new List<OnThrowProperty>();
                    foreach (Component property in Action.targetWeapon.components)
                    { 
                        if (property.GetType().BaseType.Equals(typeof(OnThrowProperty))) 
                        { 
                            properties.Add((OnThrowProperty)property); 
                        } 
                    }
                    if (properties.Count != 0)
                    {
                        Vector2 vector2 = player.GetComponent<Coordinate>().vector2;

                        foreach (OnThrowProperty property in properties)
                        {
                            foreach (Coordinate coordinate in RangeModels.FindModel(new Coordinate(vector2.x, vector2.y), new Coordinate(x, y), property.strength, player.GetComponent<Stats>().strength + 10, true, property.rangeModel))
                            {
                                sfxPositions.Add(new Vector2(coordinate.vector2.x, coordinate.vector2.y));
                                if (CMath.Distance(x, y, vector2.x, vector2.y) < player.GetComponent<Stats>().strength + 10 && World.tiles[x, y].terrainType != 0 && check)
                                {
                                    if (World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer != null && World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer.GetComponent<Draw>().fColor == "Gray")
                                    {
                                        if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                        }
                                        else
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Gray");
                                        }
                                    }
                                    else
                                    {
                                        if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Yellow");
                                        }
                                        else
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Yellow");
                                        }
                                    }
                                }
                                else
                                {
                                    if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                    {
                                        World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                    }
                                    else
                                    {
                                        World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Gray");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int maxRange = 1000;

                    List<OnUseProperty> properties = new List<OnUseProperty>();
                    foreach (Component property in Action.targetWeapon.components)
                    {
                        if (property.GetType().BaseType.Equals(typeof(OnUseProperty)))
                        {
                            properties.Add((OnUseProperty)property);
                        }
                    }

                    if (properties.Count != 0)
                    {
                        foreach (OnUseProperty property in properties)
                        {
                            if (property.range < maxRange)
                            {
                                maxRange = property.range;
                            }
                        }
                    }

                    CreateLine(maxRange, true);

                    if (properties.Count != 0)
                    {
                        Vector2 vector2 = player.GetComponent<Coordinate>().vector2;
                        foreach (OnUseProperty property in properties)
                        {
                            List<Coordinate> coordinates = RangeModels.FindModel(new Coordinate(vector2.x, vector2.y), new Coordinate(x, y), property.strength, property.range, true, property.rangeModel);
                            if (coordinates != null && coordinates.Count != 0)
                            {
                                foreach (Coordinate coordinate in coordinates)
                                {
                                    sfxPositions.Add(new Vector2(coordinate.vector2.x, coordinate.vector2.y));
                                    if (World.tiles[coordinate.vector2.x, coordinate.vector2.y].terrainType != 0)
                                    {
                                        if (World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer != null && World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer.GetComponent<Draw>().fColor == "Gray")
                                        {
                                            if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                            {
                                                World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                            }
                                            else
                                            {
                                                World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Gray");
                                            }
                                        }
                                        else
                                        {
                                            if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                            {
                                                World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Yellow");
                                            }
                                            else
                                            {
                                                World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Yellow");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (coordinate.vector2.x == x && coordinate.vector2.y == y)
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                        }
                                        else
                                        {
                                            World.tiles[coordinate.vector2.x, coordinate.vector2.y].sfxLayer = Reticle(x, y, '*', "Gray");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Renderer.MoveCamera(new Vector2(x, y));
        }
        public static bool CreateLine(int range, bool visual)
        {
            bool check = true;
            Vector2 vector3 = player.GetComponent<Coordinate>().vector2;
            int t;
            int x = vector3.x; int y = vector3.y;
            int delta_x = TargetReticle.x - vector3.x; int delta_y = TargetReticle.y - vector3.y;
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
                    if (x == TargetReticle.x && y == TargetReticle.y) 
                    {
                        hasConnected = true;
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range) 
                        { 
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Yellow");
                                }
                                CMath.DisplayToConsole(Log.console, "", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            }
                            else
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                }
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                                check = false;
                            }
                        }
                        else 
                        {
                            if (visual)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Gray");
                            }
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                            Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            check = false;
                        } 
                    }
                    else 
                    {
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range)
                        {
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Yellow");
                                }
                                CMath.DisplayToConsole(Log.console, "", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            }
                            else
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Gray");
                                }
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                                check = false;
                            }
                        }
                        else
                        {
                            if (visual)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Gray");
                            }
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                            Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            check = false;
                        }
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
                    if (x == TargetReticle.x && y == TargetReticle.y)
                    {
                        hasConnected = true;
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range)
                        {
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Yellow");
                                }
                                CMath.DisplayToConsole(Log.console, "", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            }
                            else
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Gray");
                                }
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                                check = false;
                            }
                        }
                        else
                        {
                            if (visual)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, 'X', "Gray");
                            }
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                            Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            check = false;
                        }
                    }
                    else
                    {
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range)
                        {
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Yellow");
                                }
                                CMath.DisplayToConsole(Log.console, "", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            }
                            else
                            {
                                if (visual)
                                {
                                    sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Gray");
                                }
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                                Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                                check = false;
                            }
                        }
                        else
                        {
                            if (visual)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].sfxLayer = Reticle(x, y, '.', "Gray");
                            }
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                            Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White);
                            check = false;
                        }
                    }
                }
                while (!hasConnected);
            }
            return check;
        }
        public static Vector2 ReturnCoords(bool magic)
        {
            int range;
            if (magic) 
            {
                range = 1000;

                List<OnUseProperty> properties = new List<OnUseProperty>();
                foreach (Component property in Action.targetWeapon.components)
                {
                    if (property.GetType().BaseType.Equals(typeof(OnUseProperty)))
                    {
                        properties.Add((OnUseProperty)property);
                    }
                }

                if (properties.Count != 0)
                {
                    foreach (OnUseProperty property in properties)
                    {
                        if (property.range < range)
                        {
                            range = property.range;
                        }
                    }
                }
            }
            else
            {
                range = player.GetComponent<Stats>().strength + 10;
            }
            Vector2 refVector3 = player.GetComponent<Coordinate>().vector2;
            if (CMath.Distance(refVector3.x, refVector3.y, x, y) < range)
            {
                if (CMath.PathBlocked(player.GetComponent<Coordinate>().vector2, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                {
                    ClearSFXPositions();
                    StopTargeting();
                    return new Vector2(x, y);
                }
                else { CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1); Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White); return null; }
            }
            else { CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1); Program.messageConsole.Print(11, 0, " Message Log ", RLColor.White); return null; }
        }
        public static void ThrowWeapon(Entity weaponUsed)
        {
            Vector2 vector2 = ReturnCoords(false);
            if (vector2 != null)
            {
                AttackManager.ThrowWeapon(player, new Coordinate(vector2.x, vector2.y), weaponUsed);
            }
        }
        public static Entity Reticle(int x, int y, char character, string fColor)
        {
            return new Entity(new List<Component> { new Coordinate(x, y), new Draw(fColor, "Black", character) });
        }
    }
}
