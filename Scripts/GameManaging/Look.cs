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
        public static Entity player;
        public static int x { get; set; }
        public static int y { get; set; }
        public static bool looking = false;
        public Look(Entity _player) { player = _player; }
        public static void StartLooking(Coordinate coordinate) 
        { 
            x = coordinate.vector2.x; 
            y = coordinate.vector2.y;
            looking = true; 
            player.GetComponent<TurnFunction>().turnActive = false; 
            Move(0, 0); 
            Action.LookAction();
        }
        public static void StopLooking() 
        { 
            player.GetComponent<TurnFunction>().turnActive = true;
            Renderer.MoveCamera(player.GetComponent<Coordinate>().vector2);
            looking = false;
            World.GetTraversable(new Vector2(x, y)).sfxLayer = null;
            Action.PlayerAction(player);
            Log.DisplayLog();
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(x + _x, y + _y))
            {
                World.GetTraversable(new Vector2(x, y)).sfxLayer = null;
                x += _x; y += _y;
                Traversable traversable = World.GetTraversable(new Vector2(x, y));
                Description description = null;
                if (!World.tiles[x, y].GetComponent<Visibility>().visible) 
                {
                    CMath.DisplayToConsole(Log.console, "You cannot look at what you cannot see.", 1, 1); 
                }
                else if (traversable.actorLayer != null) 
                { 
                    description = traversable.actorLayer.GetComponent<Description>();
                }
                else if (traversable.itemLayer != null)
                { 
                    description = traversable.itemLayer.GetComponent<Description>(); 
                }
                else if (traversable.obstacleLayer != null) 
                { 
                    description = traversable.obstacleLayer.GetComponent<Description>();
                }
                else 
                { 
                    description = World.tiles[x, y].GetComponent<Description>();
                }
                if (description != null)
                {
                    string display = "";

                    if (description.entity != null && description.entity.GetComponent<PronounSet>() != null)
                    {
                        if (description.entity.GetComponent<PronounSet>().present) 
                        { 
                            display += $"{description.Describe()} + + {description.name} is: + "; 
                        }
                        else 
                        {
                            display += $"{description.Describe()} + + {description.name} are: + ";
                        }

                        string compare = display;
                        if (CMath.ReturnAI(description.entity) != null) 
                        { 
                            display += $"{CMath.ReturnAI(description.entity).currentState}, "; 
                        }

                        for (int i = 0; i < description.entity.GetComponent<OnHit>().statusEffects.Count; i++)
                        {
                            if (i == description.entity.GetComponent<OnHit>().statusEffects.Count - 1) 
                            { 
                                display += $"{description.entity.GetComponent<OnHit>().statusEffects[i]}.";
                            }
                            else 
                            {
                                display += $"{description.entity.GetComponent<OnHit>().statusEffects[i]}, ";
                            }
                        }

                        if (display == compare) 
                        { 
                            display = description.Describe();
                        }
                    }
                    else { display += description.Describe(); }
                    CMath.DisplayToConsole(Log.console, display, 1, 1);
                    traversable.sfxLayer = Reticle(x, y, 'X', "Yellow");
                }
                else
                {
                    traversable.sfxLayer = Reticle(x, y, 'X', "Gray");
                }
            }
            Renderer.MoveCamera(new Vector2(x, y));
        }
        public static Entity Reticle(int x, int y, char character, string fColor)
        {
            return new Entity(new List<Component> 
            { 
                new Coordinate(x, y), 
                new Draw(fColor, "Black", character) 
            });
        }
    }
}
