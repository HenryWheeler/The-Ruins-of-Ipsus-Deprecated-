using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class PropertyFunction : Component
    {
        public List<OnTurnStartProperty> onTurnStartProperties = new List<OnTurnStartProperty>();
        public List<OnTurnEndProperty> onTurnEndProperties = new List<OnTurnEndProperty>();
        public List<OnMoveProperty> onMoveProperties = new List<OnMoveProperty>();
        public List<OnBeingHitProperty> onBeingHitProperties = new List<OnBeingHitProperty>();
        public List<OnHittingProperty> onHittingProperties = new List<OnHittingProperty>();
        public void TriggerTurnStart()
        {
            foreach (OnTurnStartProperty property in onTurnStartProperties)
            {
                property.OnTurnStart();
            }
        }
        public void TriggerTurnEnd()
        {
            foreach (OnTurnEndProperty property in onTurnEndProperties)
            {
                property.OnTurnEnd();
            }
        }
        public void TriggerOnMove(int x, int y)
        {
            foreach (OnMoveProperty property in onMoveProperties)
            {
                property.OnMove(x, y);
            }
        }
        public void TriggerOnBeingHit(Entity attacker, int dmg, string type)
        {
            foreach (OnBeingHitProperty property in onBeingHitProperties)
            {
                property.OnBeingHit(attacker, dmg, type);
            }
        }
        public void TriggerOnHitting(Entity target, int dmg, string type)
        {
            foreach (OnHittingProperty property in onHittingProperties)
            {
                property.OnHitting(target, dmg, type);
            }
        }
        public void AddProperty(Component property)
        {
            if (property.GetType().BaseType.Equals(typeof(OnTurnStartProperty))) { onTurnStartProperties.Add((OnTurnStartProperty)property); }
            else if (property.GetType().BaseType.Equals(typeof(OnTurnEndProperty))) { onTurnEndProperties.Add((OnTurnEndProperty)property); }
            else if(property.GetType().BaseType.Equals(typeof(OnMoveProperty))) { onMoveProperties.Add((OnMoveProperty)property); }
            else if(property.GetType().BaseType.Equals(typeof(OnBeingHitProperty))) { onBeingHitProperties.Add((OnBeingHitProperty)property); }
            else if(property.GetType().BaseType.Equals(typeof(OnHittingProperty))) { onHittingProperties.Add((OnHittingProperty)property); }
        }
        public void RemoveProperty(Component property)
        {
            if (property.GetType().BaseType.Equals(typeof(OnTurnStartProperty))) { onTurnStartProperties.Remove((OnTurnStartProperty)property); }
            else if (property.GetType().BaseType.Equals(typeof(OnTurnEndProperty))) { onTurnEndProperties.Remove((OnTurnEndProperty)property); }
            else if (property.GetType().BaseType.Equals(typeof(OnMoveProperty))) { onMoveProperties.Remove((OnMoveProperty)property); }
            else if (property.GetType().BaseType.Equals(typeof(OnBeingHitProperty))) { onBeingHitProperties.Remove((OnBeingHitProperty)property); }
            else if (property.GetType().BaseType.Equals(typeof(OnHittingProperty))) { onHittingProperties.Remove((OnHittingProperty)property); }
        }
        public void AddAllToEntity(Entity entityRef)
        {
            foreach (OnTurnStartProperty property in onTurnStartProperties) { entityRef.AddComponent(property); }
            foreach (OnTurnEndProperty property in onTurnEndProperties) { entityRef.AddComponent(property); }
            foreach (OnMoveProperty property in onMoveProperties) { entityRef.AddComponent(property); }
            foreach (OnBeingHitProperty property in onBeingHitProperties) { entityRef.AddComponent(property); }
            foreach (OnHittingProperty property in onHittingProperties) { entityRef.AddComponent(property); }
        }
        public void RemoveAllFromEntity(Entity entityRef)
        {
            foreach (OnTurnStartProperty property in onTurnStartProperties) { entityRef.RemoveComponent(property); }
            foreach (OnTurnEndProperty property in onTurnEndProperties) { entityRef.RemoveComponent(property); }
            foreach (OnMoveProperty property in onMoveProperties) { entityRef.RemoveComponent(property); }
            foreach (OnBeingHitProperty property in onBeingHitProperties) { entityRef.RemoveComponent(property); }
            foreach (OnHittingProperty property in onHittingProperties) { entityRef.RemoveComponent(property); }
        }
        public PropertyFunction() { }
    }
}
