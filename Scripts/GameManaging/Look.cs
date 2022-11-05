using System;
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
        public Look(Player _player) { player = _player; }
        public static void StartLooking(Coordinate coordinate) 
        { 
            x = coordinate.x; y = coordinate.y; 
            looking = true; player.GetComponent<TurnFunction>().turnActive = false; 
            Move(0, 0); RLKey key = RLKey.Unknown; Action.LookAction(key);
        }
        public static void StopLooking() 
        { 
            player.GetComponent<TurnFunction>().turnActive = true; 
            looking = false;
            Map.sfx[x, y] = null;
            RLKey key = RLKey.Unknown; Action.PlayerAction(player, key); 
            Log.ClearLogDisplay(); 
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(x + _x, y + _y))
            {
                Map.sfx[x, y] = null;
                x += _x; y += _y;
                if (!Map.map[x, y].GetComponent<Visibility>().visible) { Log.AddToStoredLog("You cannot look at what you cannot see.", true); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else if (Map.map[x, y].actor != null) { Log.AddToStoredLog(Map.map[x, y].actor.GetComponent<Description>().Describe(), true); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else if (Map.map[x, y].item != null) { Log.AddToStoredLog(Map.map[x, y].item.GetComponent<Description>().Describe(), true); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else { Log.AddToStoredLog(Map.map[x, y].GetComponent<Description>().Describe(), true); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
            }
        }
    }
}
