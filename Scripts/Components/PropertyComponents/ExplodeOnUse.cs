using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    class ExplodeOnUse: OnUseProperty
    {
        public int strength { get; set; }
        public override void OnUse(Entity entity) { SpecialEffectManager.Explosion(entity, entity.GetComponent<Coordinate>(), strength); }
        public ExplodeOnUse(int _strength) { strength = _strength; }
        public ExplodeOnUse() { }
    }
    class ExplodeOnThrow : OnThrowProperty
    {
        public int strength { get; set; }
        public override void OnThrow(Entity user, Coordinate landingSite) 
        { SpecialEffectManager.Explosion(entity, landingSite, strength); entity.GetComponent<Throwable>().ConsumeItem(); }
        public ExplodeOnThrow(int _strength) { strength = _strength; }
        public ExplodeOnThrow() { }
    }
}
