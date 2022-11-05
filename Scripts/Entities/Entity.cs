using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Entity
    {
        public int uID { get; set; }
        public int tempID { get; set; }
        public List<Component> components = new List<Component>();
        public void AddComponent(Component component)
        {
            if (!components.Contains(component) && !component.GetType().Equals(typeof(PropertyFunction)))
            {
                components.Add(component);
                component.entity = this;

                if (component.property && GetComponent<PropertyFunction>() == null) { AddComponent(new PropertyFunction()); GetComponent<PropertyFunction>().AddProperty(component); }
                else if (component.property && GetComponent<PropertyFunction>() != null) { GetComponent<PropertyFunction>().AddProperty(component); }
            }
            else if (GetComponent<PropertyFunction>() == null && component.GetType().Equals(typeof(PropertyFunction))) { components.Add(component); component.entity = this;  }
        }
        public void RemoveComponent(Component component)
        {
            components.Remove(component);
            component.entity = null;
            if (component.property && GetComponent<PropertyFunction>() != null) 
            {
                GetComponent<PropertyFunction>().RemoveProperty(component); 
                PropertyFunction function = GetComponent<PropertyFunction>(); 
                if (function.onTurnStartProperties.Count == 0 && function.onTurnEndProperties.Count == 0 && function.onMoveProperties.Count == 0 && function.onBeingHitProperties.Count == 0 && function.onHittingProperties.Count == 0) 
                { RemoveComponent(GetComponent<PropertyFunction>()); } 
            }
        }
        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component.GetType().Equals(typeof(T)))
                {
                    return (T)component;
                }
            }
            return null;
        }
        public Entity(Entity entity)
        {
            foreach (Component component in entity.components)
            {
                if (component != null)
                {
                    if (component.GetType().Equals(typeof(TurnFunction))) { AddComponent((TurnFunction)component); TurnManager.AddActor(entity.GetComponent<TurnFunction>()); }
                    else { AddComponent(component); }
                }
            }
        }
        public Entity() { }
    }
}
