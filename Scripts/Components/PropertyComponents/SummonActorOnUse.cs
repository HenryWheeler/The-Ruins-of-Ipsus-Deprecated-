using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class SummonActorOnUse : OnUseProperty
    {
        public int[] summonedCreatures { get; set; }
        public override void OnUse(Entity entity, Vector2 target = null)
        {
            if (entity.display)
            {
                if (range == 0)
                {
                    SpecialEffectManager.SummonActor(entity, entity.GetComponent<Coordinate>().vector2, summonedCreatures, strength);
                }
                else if (target == null)
                {
                    TargetReticle.StartTargeting(true, false);
                    Action.targetWeapon = this.entity;
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.SummonActor(entity, target, summonedCreatures, strength);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else
            {
                if (range == 0 || target == null)
                {
                    SpecialEffectManager.SummonActor(entity, entity.GetComponent<Coordinate>().vector2, summonedCreatures, strength);
                }
                else
                {
                    this.entity.GetComponent<Usable>().DisplayMessage(entity);
                    SpecialEffectManager.SummonActor(entity, target, summonedCreatures, strength);
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
        }
        public SummonActorOnUse(int[] _summonedCreatures, int amount, int _range, bool _singleUse = true)
        {
            summonedCreatures = _summonedCreatures;
            strength = amount;
            range = _range;
            singleUse = _singleUse;
            rangeModel = "SingleTarget";
            itemType = "Support";
        }
        public SummonActorOnUse() { }
    }
    [Serializable]
    public class SummonActorOnThrow : OnThrowProperty
    {
        public int[] summonedCreatures { get; set; }
        public override void OnThrow(Entity user, Coordinate landingSite)
        {
            SpecialEffectManager.SummonActor(user, landingSite.vector2, summonedCreatures, strength);
        }
        public SummonActorOnThrow(int[] _summonedCreatures, int amount)
        {
            summonedCreatures = _summonedCreatures;
            strength = amount;
            rangeModel = "SingleTarget";
        }
        public SummonActorOnThrow() { }
    }
}
