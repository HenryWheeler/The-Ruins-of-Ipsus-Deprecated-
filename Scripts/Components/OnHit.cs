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
        public void Hit(int dmg, string type, string weaponName, Entity entityRef) 
        {
            if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().TriggerOnBeingHit(entityRef, dmg, type); }
            Log.AddToStoredLog(entityRef.GetComponent<Description>().name + " hit " + entity.GetComponent<Description>().name + " for " + dmg + " damage with the " + weaponName + ".");
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
