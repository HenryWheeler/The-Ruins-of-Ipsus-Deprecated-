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
            foreach (Coordinate coordinate in coordinates)
            {
                Vector2 vector3 = coordinate.vector2;
                if (World.GetTraversable(coordinate.vector2).actorLayer != null)
                {
                    AttackManager.Attack(originator, World.tiles[vector3.x, vector3.y].actorLayer,
                    new AttackFunction($"1-{strength}-{strength}-{strength}-{strength}", "Explosive"), "Explosion");
                }
                else
                {
                    Entity particle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            frame1,
                            new ParticleComponent(World.random.Next(11, 14), World.random.Next(3, 5), "None", 1, frames)
                        });
                    Renderer.AddParticle(vector3.x, vector3.y, particle);
                }
            }
        }
        public static void Lightning(Entity originator, Coordinate target, int strength)
        {
            Draw frame1 = new Draw("Yellow", "Black", 'x');
            Draw frame2 = new Draw("Yellow", "Black", '+');
            Draw frame3 = new Draw("Yellow", "Black", (char)176);
            Draw[] frames = new Draw[3] { frame1, frame2, frame3 };
            List<Coordinate> coordinates = RangeModels.BeamRangeModel(originator.GetComponent<Coordinate>(), target, 1000, false);
            foreach (Coordinate coordinate in coordinates)
            {
                Vector2 vector3 = coordinate.vector2;
                if (World.GetTraversable(coordinate.vector2).actorLayer != null)
                {
                    AttackManager.Attack(originator, World.tiles[vector3.x, vector3.y].actorLayer,
                    new AttackFunction($"1-{strength}-{8}-{2}-{strength}", "Lightning"), "Lightning");
                }
                else
                {
                    Entity particle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            frame1,
                            new ParticleComponent(World.random.Next(22, 26), 5, "None", 1, frames)
                        });
                    Renderer.AddParticle(vector3.x, vector3.y, particle);
                    if (World.random.Next(0, 2) == 1)
                    {
                        Entity particle2 = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            frame1,
                            new ParticleComponent(World.random.Next(22, 26), 5, "None", 1, frames)
                        });
                        Renderer.AddParticle(vector3.x + World.random.Next(-1, 2), vector3.y + World.random.Next(-1, 2), particle2);
                    }
                }
            }
        }
        public static void MagicMap(Entity entity)
        {
            Vector2 origin = entity.GetComponent<Coordinate>().vector2;
            Draw frame1 = new Draw("Light_Yellow", "Light_Yellow", (char)0);
            Draw frame2 = new Draw("Yellow", "Yellow", (char)0);
            Draw[] frames = new Draw[2] { frame1, frame2 };
            foreach (Traversable tile in World.tiles)
            {
                if (tile.terrainType != 0)
                {
                    Coordinate coordinate = tile.entity.GetComponent<Coordinate>();
                    tile.entity.GetComponent<Visibility>().explored = true;
                    Entity particle = new Entity(new List<Component>
                        {
                            new Coordinate(0, 0),
                            frame1,
                            new ParticleComponent((int)CMath.Distance(origin.x, origin.y, coordinate.vector2.x, coordinate.vector2.y), 4, "None", 0, frames)
                        });
                    Renderer.AddParticle(coordinate.vector2.x, coordinate.vector2.y, particle);
                }
            }
        }
    }
}
