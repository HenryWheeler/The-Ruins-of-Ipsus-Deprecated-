using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class EntitySpawner
    {
        public static Entity CreateEntity(int x, int y, int uID, int type)
        {
            Entity entity = JsonDataManager.ReturnEntity(uID, type);
            entity.GetComponent<Coordinate>().x = x;
            entity.GetComponent<Coordinate>().y = y;
            switch (type)
            {
                case 0: Map.map[x, y].actor = entity; break;
                case 1: Map.map[x, y].item = entity; break;
            }
            return entity;
        }
        public static Entity ReloadEntity(Entity entityRef)
        {
            if (entityRef != null)
            {
                Entity entityToUse = new Entity(entityRef);
                if (entityToUse.GetComponent<Inventory>() != null)
                {
                    List<Entity> entities = new List<Entity>();
                    if (entityToUse.GetComponent<Inventory>().inventory != null)
                    {
                        foreach (Entity entity in entityToUse.GetComponent<Inventory>().inventory) { if (entity != null) { entities.Add(entity); } }
                        entityToUse.GetComponent<Inventory>().inventory.Clear();
                        foreach (Entity id in entities) { entityToUse.GetComponent<Inventory>().inventory.Add(new Entity(id)); }
                    }
                    if (entityToUse.GetComponent<BodyPlot>() != null)
                    {
                        entities.Clear();
                        foreach (EquipmentSlot entity in entityToUse.GetComponent<BodyPlot>().bodyPlot) { if (entity != null && entity.item != null) { entities.Add(entity.item); } }
                        foreach (Entity id in entities) { new Entity(id).GetComponent<Equippable>().Equip(entityToUse); }
                    }
                    if (CMath.ReturnAI(entityToUse) != null) { CMath.ReturnAI(entityRef).target = null; }
                }
                return entityToUse;
            }
            return null;
        }
    }
}
