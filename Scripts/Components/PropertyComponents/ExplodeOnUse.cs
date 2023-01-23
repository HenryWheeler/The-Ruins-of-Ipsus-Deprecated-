using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    class ExplodeOnUse: OnUseProperty
    {
        public override void OnUse(Entity entity, Vector2 target = null)
        {
            if (entity.display)
            {
                if (range == 0)
                {
                    SpecialEffectManager.Explosion(entity, entity.GetComponent<Coordinate>(), strength);
                }
                else if (target == null)
                {
                    TargetReticle.StartTargeting(true, false);
                    Action.targetWeapon = this.entity;
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.Explosion(entity, new Coordinate(target), strength);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else
            {
                if (range == 0 || target == null)
                {
                    SpecialEffectManager.Explosion(entity, entity.GetComponent<Coordinate>(), strength);
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.Explosion(entity, new Coordinate(target), strength);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
        }
        public ExplodeOnUse(int _strength, int _range, bool _singleUse = true) 
        { 
            strength = _strength;
            range = _range;
            singleUse = _singleUse;
            itemType = "Offense";
            rangeModel = "Sphere";
        }
        public ExplodeOnUse() { }
    }
    class ExplodeOnThrow : OnThrowProperty
    {
        public override void OnThrow(Entity user, Coordinate landingSite) 
        { 
            SpecialEffectManager.Explosion(entity, landingSite, strength); 
        }
        public ExplodeOnThrow(int _strength) 
        {
            strength = _strength;
            rangeModel = "Sphere";
            itemType = "Offense";
        }
        public ExplodeOnThrow() { }
    }
}
