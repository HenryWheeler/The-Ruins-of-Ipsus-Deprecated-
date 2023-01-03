using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Entity
    {
        public bool display { get; set; }
        public List<Component> collectionsToRemove = new List<Component>();
        public List<Component> components = new List<Component>();
        public void AddComponent(Component component)
        {
            if (!components.Contains(component) && component != null)
            {
                components.Add(component);
                component.entity = this;
            }
        }
        public void RemoveComponent(Component component)
        {
            components.Remove(component);
            component.entity = null;
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
        public bool CheckComponent<T>() where T : Component 
        { 
            if (GetComponent<T>() != null) 
            { 
                return true; 
            } 
            else 
            { 
                return false; 
            }  
        }
        public void ClearCollections() 
        { 
            if (collectionsToRemove.Count != 0) 
            { 
                foreach (Component component in collectionsToRemove) 
                { 
                    RemoveComponent(component); 
                } 
                collectionsToRemove.Clear(); 
            } 
        }
        public void ClearEntity()
        {
            foreach (Component component in components)
            { 
                if (component != null) 
                { 
                    component.entity = null; 
                } 
            }
            components = null;
        }
        public Entity(Entity entity)
        {
            display = entity.display;
            foreach (Component component in entity.components)
            {
                if (component != null)
                {
                    if (component.GetType().Equals(typeof(TurnFunction))) 
                    { 
                        AddComponent((TurnFunction)component); 
                        TurnManager.AddActor(entity.GetComponent<TurnFunction>()); 
                    }
                    else 
                    { 
                        AddComponent(component); 
                    }
                }
            }
        }
        public Entity(List<Component> _components, int id = -1, bool _display = false) 
        { 
            display = _display;
            foreach (Component component in _components)
            {
                if (component != null)
                {
                    if (component.GetType().Equals(typeof(TurnFunction))) 
                    { 
                        AddComponent((TurnFunction)component); 
                        TurnManager.AddActor(GetComponent<TurnFunction>()); 
                    }
                    else 
                    { 
                        AddComponent(component); 
                    }
                }
            }
            if (id != -1) 
            { 
                AddComponent(new ID(id)); 
            }
        }
        public Entity() { }
    }
}
