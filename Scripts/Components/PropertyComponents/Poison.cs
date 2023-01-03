using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Poison: OnTurnProperty
    {
        public int timeLeft { get; set; }
        public int strength { get; set; }
        public override void OnTurn()
        {
            timeLeft--; if (timeLeft == 0) { entity.collectionsToRemove.Add(this); entity.GetComponent<OnHit>().statusEffects.Remove("Green*Poisoned");
                if (entity.display)
                { Log.AddToStoredLog("The Green*poison ailing " + entity.GetComponent<PronounSet>().subjective + " has subsided."); }
            } 
            else
            {
                int dmg = World.random.Next(strength - 2, strength + 2);
                entity.GetComponent<OnHit>().LowerHealth(dmg);
                if (entity.GetComponent<Stats>() != null && entity.display) 
                { Log.AddToStoredLog("The Green*poison drains " + dmg + " points of " + entity.GetComponent<PronounSet>().possesive + " health away."); }
            }
        }
        public Poison(int _timeLeft, int _strength) { timeLeft = _timeLeft; strength = _strength; start = true; }
        public Poison() { start = true; }
    }
}
