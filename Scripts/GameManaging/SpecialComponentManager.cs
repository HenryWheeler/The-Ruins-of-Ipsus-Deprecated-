using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class SpecialComponentManager
    {
        public static void TriggerTurn(Entity entity, bool start)
        {
            List<OnTurnProperty> properties = new List<OnTurnProperty>();
            foreach (Component property in entity.components) 
            { if (property.GetType().BaseType.Equals(typeof(OnTurnProperty))) { properties.Add((OnTurnProperty)property); } }
            foreach (OnTurnProperty property in properties)
            {
                if (start && property.start) { property.OnTurn(); }
                else if (!start && !property.start) { property.OnTurn(); }
            }
            entity.ClearCollections();
        }
        public static void TriggerOnMove(Entity entity, int x1, int y1, int x2, int y2)
        {
            List<OnMoveProperty> properties = new List<OnMoveProperty>();
            foreach (Component property in entity.components)
            { if (property.GetType().BaseType.Equals(typeof(OnMoveProperty))) { properties.Add((OnMoveProperty)property); } }
            foreach (OnMoveProperty property in properties) { property.OnMove(x1, y1, x2, y2); }
            entity.ClearCollections();
        }
        public static void TriggerOnHit(Entity entity, Entity attacker, Entity target, int dmg, string type, bool attack)
        {
            List<OnHitProperty> properties = new List<OnHitProperty>();
            foreach (Component property in entity.components)
            { if (property.GetType().BaseType.Equals(typeof(OnHitProperty))) { properties.Add((OnHitProperty)property); } }
            foreach (OnHitProperty property in properties) 
            {
                if (attack && property.attack) { property.OnHit(attacker, target, dmg, type); }
                else if (!attack && !property.attack) { property.OnHit(attacker, target, dmg, type); }
            }
            entity.ClearCollections();
        }
        public static void TriggerOnUse(Entity entity, Entity itemUsed)
        {
            List<OnUseProperty> properties = new List<OnUseProperty>();
            foreach (Component property in itemUsed.components)
            { if (property.GetType().BaseType.Equals(typeof(OnUseProperty))) { properties.Add((OnUseProperty)property); } }
            foreach (OnUseProperty property in properties) { property.OnUse(entity); }
            itemUsed.ClearCollections();
        }
        public static void TriggerOnThrow(Entity entity, Entity itemThrown, Coordinate landingSite)
        {
            List<OnThrowProperty> properties = new List<OnThrowProperty>();
            foreach (Component property in itemThrown.components)
            { if (property.GetType().BaseType.Equals(typeof(OnThrowProperty))) { properties.Add((OnThrowProperty)property); } }
            foreach (OnThrowProperty property in properties) { property.OnThrow(entity, landingSite); }
            itemThrown.ClearCollections();
        }
        public static void AddAllToEntity(Entity entityToAddTo, Entity entityToAddWith)
        {
            foreach (Component component in entityToAddWith.components) { if (component.special) { entityToAddTo.AddComponent(component); } }
        }
        public static void RemoveAllFromEntity(Entity entityToSubtractFrom, Entity entityToSubtractdWith)
        {
            foreach (Component component in entityToSubtractdWith.components) { if (component.special) { entityToSubtractFrom.RemoveComponent(component); } }
        }
    }
}
