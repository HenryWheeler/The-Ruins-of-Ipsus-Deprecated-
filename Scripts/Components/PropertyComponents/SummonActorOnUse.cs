﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class SummonActorOnUse : OnUse
    {
        public int[] summonedCreatures { get; set; }
        public override void Use(Entity entity, Vector2 target = null)
        {
            if (entity.GetComponent<PlayerComponent>() != null)
            {
                if (range == 0)
                {
                    SpecialEffectManager.SummonActor(entity, entity.GetComponent<Vector2>(), summonedCreatures, strength);
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
                    SpecialEffectManager.SummonActor(entity, entity.GetComponent<Vector2>(), summonedCreatures, strength);
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
    public class SummonActorOnThrow : OnThrow
    {
        public int[] summonedCreatures { get; set; }
        public override void Throw(Entity user, Vector2 landingSite)
        {
            SpecialEffectManager.SummonActor(user, landingSite, summonedCreatures, strength);
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
