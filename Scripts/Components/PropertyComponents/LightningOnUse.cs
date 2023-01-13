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
        public int strength { get; set; }
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
                    Log.Add("A bolt of Yellow*lightning crackles and fries the air in front of you!");
                    SpecialEffectManager.Lightning(entity, new Coordinate(target), strength);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
        }
        public LightningOnUse(int _strength)
        {
            strength = _strength;
        }
        public LightningOnUse() { }
    }
}
