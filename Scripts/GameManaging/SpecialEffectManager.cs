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
            List<SFX> sfx = new List<SFX>();
            foreach (Coordinate coordinate in coordinates)
            {
                if (Map.map[coordinate.x, coordinate.y].actor != null)
                { AttackManager.EffectAttack(originator, Map.map[coordinate.x, coordinate.y].actor, 
                    new AttackFunction(strength, strength, strength, strength, "Explosive", "Magic"), "Explosion"); }

                SFX sFX = new SFX(coordinate.x, coordinate.y, frame1.character, frame1.fColor, frame1.bColor, false, false);
                if (CMath.random.Next(1, 100) > 50) { sFX.AddComponent(new AnimationFunction(frames)); }
                else { sFX.AddComponent(new AnimationFunction(new Draw[3] { frame3, frame2, frame1 })); }
                sfx.Add(sFX);
            }
            Renderer.StartAnimationThread(sfx, 25, 25);
        }
        public static void Beam(Entity originator, Coordinate origin, Coordinate target, int strength, int range, string type)
        {
            List<Coordinate> coordinates = RangeModels.BeamRangeModel(origin, target, range, true);
            List<SFX> sfx = new List<SFX>();
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
                        if (i == current) { frames[i] = frame1; }
                        else { frames[i] = baseFrame; }
                    }
                }
                else { for (int i = 0; i < coordinates.Count(); i++) { frames[i] = finalFrame; } }
                SFX sFX;
                if (current == 0) { sFX = new SFX(coordinate.x, coordinate.y, frame1.character, frame1.fColor, frame1.bColor, false, true); }
                else { sFX = new SFX(coordinate.x, coordinate.y, '.', "Gray", "Black", false, true); }
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
                            if (Map.map[coordinate.x, coordinate.y].actor != null)
                            {
                                AttackManager.EffectAttack(originator, Map.map[coordinate.x, coordinate.y].actor,
                                  new AttackFunction(strength, 8, 0, 4, "Fire", "Magic"), "Steam");
                            }
                        }
                        break;
                    }
            }
        }
    }
}
