using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class SpellManager
    {
        public static void BeamOfSteam(Entity caster, Coordinate target)
        {
            if (ReduceMP(caster, 5))
            {
                SpecialEffectManager.Beam(caster, caster.GetComponent<Coordinate>(), target, 2, 10, "Steam");
            }
        }
        public static bool ReduceMP(Entity entity, int reduction)
        {
            Stats stats = entity.GetComponent<Stats>();
            if (stats.mp < reduction) { return false; }
            else { stats.mp -= reduction; return true; }
        }
    }
}
