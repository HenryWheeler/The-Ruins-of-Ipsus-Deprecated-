using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class BreathWeaponOnUse: OnUseProperty
    {
        public string type { get; set; }
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
                    SpecialEffectManager.BreathWeapon(entity, new Coordinate(target), strength, range, type);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else
            {
                if (range == 0 || target == null)
                {
                    SpecialEffectManager.BreathWeapon(entity, entity.GetComponent<Coordinate>(), strength, range, type);
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.BreathWeapon(entity, new Coordinate(target), strength, range, type);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
        }
        public BreathWeaponOnUse(int _strength, string _type, int _range, bool _singleUse = true)
        {
            strength = _strength;
            range = _range;
            type = _type;
            itemType = "Offense";
            singleUse = _singleUse;
            rangeModel = "Cone";
        }
        public BreathWeaponOnUse() { }
    }
}
