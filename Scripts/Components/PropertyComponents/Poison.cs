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
        public bool display { get; set; }
        public override void OnTurn()
        {
            timeLeft--; if (timeLeft == 0) { entity.collectionsToRemove.Add(this); 
                if (entity.GetComponent<Stats>() != null && entity.GetComponent<Stats>().display)
                { Log.AddToStoredLog("The poison ailing " + entity.GetComponent<PronounSet>().subjective + " has subsided."); }
            } 
            else
            {
                int dmg = CMath.random.Next(strength - 2, strength + 2);
                entity.GetComponent<OnHit>().LowerHealth(dmg);
                if (entity.GetComponent<Stats>() != null && entity.GetComponent<Stats>().display) 
                { Log.AddToStoredLog("The poison drains " + dmg + " points of " + entity.GetComponent<PronounSet>().possesive + " health away."); }
            }
        }
        public Poison(int _timeLeft, int _strength, bool _display = false) { timeLeft = _timeLeft; strength = _strength; display = _display; special = true; componentName = "Green*Poisoned"; start = true; }
        public Poison() { special = true; componentName = "Green*Poisoned"; start = true; }
    }
}
