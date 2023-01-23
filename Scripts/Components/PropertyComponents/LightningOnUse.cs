using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class LightningOnUse: OnUseProperty
    {
        public override void OnUse(Entity entity, Vector2 target = null)
        {
            if (entity.display)
            {
                if (target == null)
                {
                    TargetReticle.StartTargeting(true, false);
                    Action.targetWeapon = this.entity;
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.Lightning(entity, new Coordinate(target), strength, range);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
        }
        public LightningOnUse(int _strength, int _range, bool _singleUse = true)
        {
            strength = _strength;
            range = _range;
            singleUse = _singleUse;
            rangeModel = "Line";
            itemType = "Offense";
        }
        public LightningOnUse() { }
    }
}
