using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class InflictPoisonOnHit : OnHitProperty
    {
        public int strength { get; set; }
        public override void OnHit(Entity attacker, Entity target, int dmg, string type)
        {
            if (target.GetComponent<Poison>() == null && !target.GetComponent<Stats>().immunities.Contains("Poison")) 
            { 
                if (World.random.Next(1, 21) + strength > 10 + target.GetComponent<Stats>().strength)
                {
                    target.AddComponent(new Poison(strength * 3, strength));
                    if (attacker.GetComponent<PronounSet>().present) { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " has inflicted " + target.GetComponent<Description>().name + " with a malignant Green*poison"); }
                    else { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " have inflicted " + target.GetComponent<Description>().name + " with a malignant Green*poison"); }
                    target.GetComponent<Harmable>().statusEffects.Add("Green*Poisoned");
                }
            }
        }
        public InflictPoisonOnHit(int _strength) { strength = _strength; attack = true; }
        public InflictPoisonOnHit() { attack = true; }
    }
}
