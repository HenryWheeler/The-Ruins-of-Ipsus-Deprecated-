using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class EntityManager
    {
        public static Dictionary<string, List<Entity>> entities = new Dictionary<string, List<Entity>>();
        public static void UpdateAll()
        {
            foreach (List<Entity> entities in entities.Values)
            { if (entities != null) { UpdateMap(entities[0]); } }
        }
        public static void AddEntity(Entity entity) 
        {
            if (entity.GetComponent<Faction>() != null)
            {
                if (!entities.ContainsKey(entity.GetComponent<Faction>().faction)) 
                { entities.Add(entity.GetComponent<Faction>().faction, new List<Entity>()); }
                entities[entity.GetComponent<Faction>().faction].Add(entity);
            }
            else
            {
                if (!entities.ContainsKey("Items"))
                { entities.Add("Items", new List<Entity>()); }
                entities["Items"].Add(entity);
            }
            UpdateMap(entity);
        }
        public static void RemoveEntity(Entity entity)
        {
            UpdateMap(entity);
            if (entity.GetComponent<Faction>() != null)
            {
                entities[entity.GetComponent<Faction>().faction].Remove(entity);
                if (entities[entity.GetComponent<Faction>().faction].Count == 0) 
                { entities.Remove(entity.GetComponent<Faction>().faction); }
            }
            else
            {
                entities["Items"].Remove(entity);
                if (entities["Items"].Count == 0)
                { entities.Remove("Items"); }
            }
        }
        public static void UpdateMap(Entity entity) 
        {
            if (entity.GetComponent<Faction>() != null)
            {
                string faction = entity.GetComponent<Faction>().faction;
                if (entities.ContainsKey(faction)) { DijkstraMaps.CreateMap(entities[faction], faction); }
            }
            else { if (entities.ContainsKey("Items")) { DijkstraMaps.CreateMap(entities["Items"], entity.GetComponent<Description>().name); } }
        }
    }
}
