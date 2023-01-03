using System;
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
        public static void StartTargeting(bool _inInventory)
        {
            inInventory = _inInventory;
            if (inInventory) { InventoryManager.CloseInventory(); }
            Vector2 vector3 = player.GetComponent<Coordinate>().vector2;
            x = vector3.x; y = vector3.y;
            targeting = true; player.GetComponent<TurnFunction>().turnActive = false;
            Move(0, 0);
            RLKey key = RLKey.Unknown; Action.TargetAction(player, key);
        }
        private static void ClearSFXPositions()
        {
            foreach (Vector2 position in sfxPositions) { World.tiles[position.x, position.y].GetComponent<Traversable>().sfxLayer = null; }
            sfxPositions.Clear();
        }
        public static void StopTargeting()
        {
            targeting = false; player.GetComponent<TurnFunction>().turnActive = true;
            ClearSFXPositions();
            Renderer.MoveCamera(player.GetComponent<Coordinate>().vector2);
            if (!inInventory) { Action.PlayerAction(player); }
            else { InventoryManager.OpenInventory(); }
            Log.DisplayLog();
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(x + _x, y + _y))
            {
                ClearSFXPositions();
                x += _x; y += _y;
                CreateLine(player.GetComponent<Stats>().strength + 10);
            }
            Renderer.MoveCamera(new Vector2(x, y));
        }
        public static void CreateLine(int range)
        {
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
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Yellow");
                            }
                            else
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Gray");
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                            }
                        }
                        else 
                        { 
                            sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Gray");
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1); 
                        } 
                    }
                    else 
                    {
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range)
                        {
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Yellow");
                            }
                            else
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Gray");
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                            }
                        }
                        else
                        {
                            sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Gray");
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
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
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Yellow");
                            }
                            else
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Gray");
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                            }
                        }
                        else
                        {
                            sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, 'X', "Gray");
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                        }
                    }
                    else
                    {
                        if (CMath.Distance(vector3.x, vector3.y, x, y) < range)
                        {
                            if (CMath.PathBlocked(vector3, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Yellow");
                            }
                            else
                            {
                                sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Gray");
                                CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1);
                            }
                        }
                        else
                        {
                            sfxPositions.Add(new Vector2(x, y)); World.tiles[x, y].GetComponent<Traversable>().sfxLayer = Reticle(x, y, '.', "Gray");
                            CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1);
                        }
                    }
                }
                while (!hasConnected);
            }
        }
        public static void ThrowWeapon(Entity weaponUsed)
        {
            int range = player.GetComponent<Stats>().strength + 10;
            Vector2 refVector3 = player.GetComponent<Coordinate>().vector2;
            if (CMath.Distance(refVector3.x, refVector3.y, x, y) < range)
            {
                if (CMath.PathBlocked(player.GetComponent<Coordinate>().vector2, new Vector2(x, y), range) && World.GetTraversable(new Vector2(x, y)).terrainType != 0)
                {
                    ClearSFXPositions();
                    StopTargeting();
                    AttackManager.ThrowWeapon(player, new Coordinate(x, y), weaponUsed);
                }
                else { CMath.DisplayToConsole(Log.console, "Your target is blocked.", 1, 1); return; }
            }
            else { CMath.DisplayToConsole(Log.console, "Your target is out of range.", 1, 1); return; }
        }
        public static Entity Reticle(int x, int y, char character, string fColor)
        {
            return new Entity(new List<Component> { new Coordinate(x, y), new Draw(fColor, "Black", character) });
        }
    }
}
