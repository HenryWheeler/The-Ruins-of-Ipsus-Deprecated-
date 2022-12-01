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
    }
}
