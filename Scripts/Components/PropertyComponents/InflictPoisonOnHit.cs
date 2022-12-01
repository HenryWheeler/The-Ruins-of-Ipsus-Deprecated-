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
            if (target.GetComponent<Poison>() == null) 
            { 
                target.AddComponent(new Poison(strength * 3, strength));
                if (attacker.GetComponent<PronounSet>().present) { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " has inflicted " + target.GetComponent<Description>().name + " with a malignant Green*poison"); }
                else { Log.AddToStoredLog(attacker.GetComponent<Description>().name + " have inflicted " + target.GetComponent<Description>().name + " with a malignant Green*poison"); }
            }
        }
        public InflictPoisonOnHit(int _strength) { strength = _strength; special = true; attack = true; }
        public InflictPoisonOnHit() { special = true; attack = true; }
    }
}
