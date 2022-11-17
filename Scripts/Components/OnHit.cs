using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class OnHit: Component
    {
        public void Hit(int dmg, string type, string weaponName, Entity attacker) 
        {
            SpecialComponentManager.TriggerOnHit(entity, attacker, entity, dmg, type, false);
            if (CMath.ReturnAI(entity) != null) { CMath.ReturnAI(entity).OnHit(attacker); }
            Log.AddToStoredLog(attacker.GetComponent<Description>().name + " hit " + entity.GetComponent<Description>().name + " for " + dmg + " damage with the " + weaponName + ".");
            Stats stats = entity.GetComponent<Stats>();
            stats.hp -= dmg;
            if (stats.display) { StatManager.UpdateStats(entity); }
            if (stats.hp <= 0) { Death(); }
        }
        public void LowerHealth(int dmg)
        {
            Stats stats = entity.GetComponent<Stats>();
            stats.hp -= dmg;
            if (stats.display) { StatManager.UpdateStats(entity); }
            if (stats.hp <= 0) { Death(); }
        }
        public void Death() 
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            Map.map[coordinate.x, coordinate.y].actor = null; 
            Log.AddToStoredLog(entity.GetComponent<Description>().name + " has died."); 
            TurnManager.RemoveActor(entity.GetComponent<TurnFunction>()); 
        }
        public OnHit() { }
    }
}
