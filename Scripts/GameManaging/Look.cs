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
                Description description = null;
                if (!Map.map[x, y].GetComponent<Visibility>().visible) { Log.AddToStoredLog("You cannot look at what you cannot see.", true); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else if (Map.map[x, y].actor != null) { description = Map.map[x, y].actor.GetComponent<Description>(); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else if (Map.map[x, y].item != null) { description = Map.map[x, y].item.GetComponent<Description>(); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else if (Map.map[x, y].terrain != null) { description = Map.map[x, y].terrain.GetComponent<Description>(); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                else { description = Map.map[x, y].GetComponent<Description>(); Map.sfx[x, y] = new SFX(x, y, 'X', "Yellow", "Black", false, true); }
                if (description != null)
                {
                    string display = "";

                    if (description.entity != null && description.entity.GetComponent<PronounSet>() != null)
                    {
                        if (description.entity.GetComponent<PronounSet>().present) { display += description.Describe() + " + + " + description.name + " is: + "; }
                        else { display += description.Describe() + " + + " + description.name + " are: + "; }
                        string compare = display;
                        if (CMath.ReturnAI(description.entity) != null) { display += CMath.ReturnAI(description.entity).mood + ", "; }
                        foreach (Component component in description.entity.components)
                        {
                            if (component.special && component.componentName != "") { display += component.componentName + ", "; }
                        }
                        if (display == compare) { display = description.Describe();}
                    }
                    else { display += description.Describe(); }
                    Log.AddToStoredLog(display, true);
                }
            }
        }
    }
}
