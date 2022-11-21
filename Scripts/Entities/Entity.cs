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
        public List<Component> collectionsToRemove = new List<Component>();
        public List<Component> components = new List<Component>();
        public void AddComponent(Component component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
                component.entity = this;
                if (component.componentName != null && GetComponent<Stats>() != null) { GetComponent<Stats>().status.Add(component.componentName); }
            }
        }
        public void RemoveComponent(Component component)
        {
            components.Remove(component);
            component.entity = null;
            if (component.componentName != null && GetComponent<Stats>() != null) { GetComponent<Stats>().status.Remove(component.componentName); }
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
        public void ClearCollections() { if (collectionsToRemove.Count != 0) 
            { foreach (Component component in collectionsToRemove) { RemoveComponent(component); if (component.componentName != "") 
                    { GetComponent<Stats>().status.Remove(component.componentName); } } collectionsToRemove.Clear(); } }
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
