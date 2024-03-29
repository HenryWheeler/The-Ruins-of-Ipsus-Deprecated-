﻿using System;
using SadConsole;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Look
    {
        public static Entity player;
        public static Vector2 position { get; set; }
        public static bool looking = false;
        public Look(Entity _player) { player = _player; }
        public static void StartLooking(Vector2 coordinate) 
        {
            position = coordinate;
            looking = true; 
            player.GetComponent<TurnFunction>().turnActive = false; 
            Move(0, 0); 
            Action.LookAction();
        }
        public static void StopLooking() 
        { 
            player.GetComponent<TurnFunction>().turnActive = true;
            Renderer.MoveCamera(player.GetComponent<Vector2>());
            looking = false;
            World.tiles[position.x, position.y].sfxLayer = null;
            //Action.PlayerAction(player);
            StatManager.UpdateStats(player);
        }
        public static void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(position.x + _x, position.y + _y))
            {
                World.tiles[position.x, position.y].sfxLayer = null;
                position.x += _x; position.y += _y;
                Traversable traversable = World.tiles[position.x, position.y];
                Description description = null;
                if (!World.tiles[position.x, position.y].entity.GetComponent<Visibility>().visible) 
                {
                    CMath.DisplayToConsole(Program.playerConsole, "You cannot look at what you cannot see.", 1, 1); 
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
                    description = World.tiles[position.x, position.y].entity.GetComponent<Description>();
                }
                if (description != null)
                {
                    string display = "";

                    if (description.entity != null && description.entity.GetComponent<PronounSet>() != null)
                    {
                        if (description.entity.GetComponent<PronounSet>().present) 
                        { 
                            display += $"{description.description} + + {description.name} is: + "; 
                        }
                        else 
                        {
                            display += $"{description.description} + + {description.name} are: + ";
                        }

                        string compare = display;
                        if (CMath.ReturnAI(description.entity) != null) 
                        { 
                            display += $"{CMath.ReturnAI(description.entity).currentState}, ";

                            if (description.entity.GetComponent<Harmable>().statusEffects.Count == 0)
                            {
                                display += "and ";
                            }
                        }

                        Stats stats = description.entity.GetComponent<Stats>();

                        if (stats.hp == stats.hpCap) { display += "Green*Uninjured"; }
                        else if (stats.hp <= stats.hpCap && stats.hp >= stats.hpCap / 2) { display += "Yellow*Hurt"; }
                        else { display += "Red*Badly Red*Hurt"; }

                        if (description.entity.GetComponent<Harmable>().statusEffects.Count == 0)
                        {
                            display += ".";
                        }
                        else
                        {
                            display += ", + ";
                        }

                        for (int i = 0; i < description.entity.GetComponent<Harmable>().statusEffects.Count; i++)
                        {
                            if (i == description.entity.GetComponent<Harmable>().statusEffects.Count - 1) 
                            { 
                                display += $"and {description.entity.GetComponent<Harmable>().statusEffects[i]}. + ";
                            }
                            else 
                            {
                                display += $"{description.entity.GetComponent<Harmable>().statusEffects[i]}, ";
                            }
                        }

                        if (display == compare) 
                        { 
                            display = description.description;
                        }
                    }
                    else { display += description.description; }
                    CMath.DisplayToConsole(Program.playerConsole, display, 1, 1);
                    string[] nameParts = description.name.Split(' ');
                    string name = "";
                    foreach (string part in nameParts)
                    {
                        string[] temp = part.Split('*');
                        if (temp.Length == 1)
                        {
                            name += temp[0] + " ";
                        }
                        else
                        {
                            name += temp[1] + " ";
                        }
                    }
                    int start = 17 - (int)Math.Ceiling((double)name.Length / 2);

                    Program.playerConsole.Print(start, 0, " ", Color.White);

                    start++;

                    foreach (string part in nameParts)
                    {
                        string[] temp = part.Split('*');
                        if (temp.Length == 1)
                        {
                            Program.playerConsole.Print(start, 0, temp[0] + " ", Color.White);
                            start += temp[0].Length + 1;
                        }
                        else
                        {
                            Program.playerConsole.Print(start, 0, temp[1] + " ", ColorFinder.ColorPicker(temp[0]), Color.Black);
                            start += temp[1].Length + 1;
                        }
                    }
                    traversable.sfxLayer = Reticle(position.x, position.y, 'X', "Yellow");
                }
                else
                {
                    traversable.sfxLayer = Reticle(position.x, position.y, 'X', "Gray");
                }
            }

            Renderer.MoveCamera(new Vector2(position.x, position.y));

            CMath.DisplayToConsole(Program.playerConsole, $"Move Reticle Yellow*[Arrow Yellow*Keys]", 0, 2, 1, 29, false);
            CMath.DisplayToConsole(Program.playerConsole, $"Cancel Look Yellow*[L/Escape]", 0, 2, 1, 32, false);
        }
        public static Entity Reticle(int x, int y, char character, string fColor)
        {
            return new Entity(new List<Component> 
            { 
                new Vector2(x, y), 
                new Draw(fColor, "Black", character) 
            });
        }
    }
}
