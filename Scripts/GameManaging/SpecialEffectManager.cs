using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class SpecialEffectManager
    {
        public static void Explosion(Entity originator, Coordinate origin, int strength)
        {
            Draw frame1 = new Draw("Orange", "Orange", '+');
            Draw frame2 = new Draw("Red", "Red", 'x');
            Draw frame3 = new Draw("Red_Orange", "Red_Orange", (char)176);
            Draw[] frames = new Draw[3] { frame1, frame2, frame3 };
            List<Coordinate> coordinates = RangeModels.SphereRangeModel(origin, strength, true);
            List<Entity> sfx = new List<Entity>();
            foreach (Coordinate coordinate in coordinates)
            {
                Vector2 vector3 = coordinate.vector2;
                if (World.GetTraversable(coordinate.vector2).actorLayer != null)
                { 
                    AttackManager.Attack(originator, World.tiles[vector3.x, vector3.y].GetComponent<Traversable>().actorLayer, 
                    new AttackFunction($"1-{strength}-{strength}-{strength}-{strength}", "Explosive"), "Explosion"); 
                }

                Entity sFX = new Entity(new List<Component>() { new Coordinate(vector3), new Draw(frame1) });
                if (World.random.Next(1, 100) > 50) 
                {
                    sFX.AddComponent(new AnimationFunction(frames)); 
                }
                else
                {
                    sFX.AddComponent(new AnimationFunction(new Draw[3]
                    {
                    frame3, frame2, frame1
                    }));
                }
                sfx.Add(sFX);
            }
            Renderer.StartAnimationThread(sfx, 25, 25);
        }
        public static void Beam(Entity originator, Coordinate origin, Coordinate target, int strength, int range, string type)
        {
            List<Coordinate> coordinates = RangeModels.BeamRangeModel(origin, target, range, true);
            List<Entity> sfx = new List<Entity>();
            int current = 0;
            Draw frame1 = new Draw("White", "Black", (char)176);
            Draw baseFrame = new Draw("Gray", "Black", '.');
            Draw finalFrame = new Draw("Yellow", "Black", 'X');
            foreach (Coordinate coordinate in coordinates)
            {
                Draw[] frames = new Draw[coordinates.Count()];
                if (current != coordinates.Count - 1)
                {
                    for (int i = 0; i < coordinates.Count(); i++)
                    {
                        if (i == current)
                        {
                            frames[i] = frame1; 
                        }
                        else 
                        {
                            frames[i] = baseFrame;
                        }
                    }
                }
                else { for (int i = 0; i < coordinates.Count(); i++) { frames[i] = finalFrame; } }
                Entity sFX;
                if (current == 0)
                { 
                    sFX = new Entity(new List<Component>() 
                    { 
                        new Coordinate(coordinate.vector2), new Draw(frame1) 
                    }); 
                }
                else
                {
                    sFX = new Entity(new List<Component>() 
                    { 
                        new Coordinate(coordinate.vector2), new Draw("Gray", "Black", '.')
                    });
                }
                sFX.AddComponent(new AnimationFunction(frames));
                sfx.Add(sFX);
                current++;
            }
            Renderer.StartAnimationThread(sfx, coordinates.Count(), 250 / coordinates.Count());
            switch (type)
            {
                case "Steam":
                    {
                        foreach (Coordinate coordinate in coordinates)
                        {
                            if (World.GetTraversable(coordinate.vector2).actorLayer != null)
                            {
                                AttackManager.Attack(originator, World.GetTraversable(coordinate.vector2).actorLayer,
                                  new AttackFunction($"1-{strength}-8-0-4", "Fire"), "Steam");
                            }
                        }
                        break;
                    }
            }
        }
    }
}
