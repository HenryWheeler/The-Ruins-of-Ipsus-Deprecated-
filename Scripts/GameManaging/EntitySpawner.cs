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
            EntityManager.AddEntity(entity);
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
                EntityManager.AddEntity(entityToUse);
                return entityToUse;
            }
            return null;
        }
        public static void CurrrentTestThing(int x, int y)
        {
            Entity cucumber = new Entity();
            cucumber.AddComponent(new Coordinate(x, y));
            cucumber.AddComponent(new Draw("Dark_Blue", "Black", 'e'));
            cucumber.AddComponent(new Description("Dark_Blue*Deepwater Dark_Blue*Eel", "A product of the deep, one with a nasty temper and empty stomach."));
            cucumber.AddComponent(PronounReferences.pronounSets["Nueter"]);
            cucumber.AddComponent(new Stats(15, 8, .5f, 5, 5, 5, false));
            cucumber.AddComponent(new TurnFunction(cucumber.GetComponent<Stats>().maxAction, false));
            cucumber.AddComponent(new Movement(false, true));
            cucumber.AddComponent(new Inventory(false));
            cucumber.AddComponent(new BodyPlot("Basic_Worm"));
            cucumber.AddComponent(new Visibility(false, false, false));
            cucumber.AddComponent(new OnHit());
            cucumber.AddComponent(new ChaseAI(20));
            cucumber.AddComponent(new Faction("Sea_Beast"));
            cucumber.GetComponent<ChaseAI>().hatedEntities.Add("Human");

            Entity teeth = new Entity();
            teeth.AddComponent(new Coordinate(0, 0));
            teeth.AddComponent(new Draw("White", "Black", '/'));
            teeth.AddComponent(new Description("Sharp Teeth", "A pair of sharp teeth."));
            teeth.AddComponent(new Visibility(false, false, false));
            teeth.AddComponent(new Equippable("Face", false, false));
            teeth.AddComponent(new AttackFunction(1, 3, 0, 2, "Piercing", "Melee"));

            InventoryManager.AddToInventory(cucumber, teeth);
            InventoryManager.EquipItem(cucumber, teeth);

            TurnManager.AddActor(cucumber.GetComponent<TurnFunction>());
            Map.map[x, y].actor = cucumber;

            EntityManager.AddEntity(cucumber);
        }
    }
}
