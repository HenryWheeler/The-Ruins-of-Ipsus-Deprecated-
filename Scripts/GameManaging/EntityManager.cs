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
        public static Dictionary<int, Entity> entityReferences = new Dictionary<int, Entity>();
        public EntityManager()
        {
            List<Entity> entities = JsonDataManager.PullAllEntities();
            foreach (Entity entity in entities)
            {
                if (entity != null)
                {
                    entityReferences.Add(entity.uID, entity);
                }
            }
        }
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
                if (entities.ContainsKey(faction)) { DijkstraMaps.CreateMap(entities[faction], faction, 50); }
            }
            else { if (entities.ContainsKey("Items")) { DijkstraMaps.CreateMap(entities["Items"], entity.GetComponent<Description>().name, 50); } }
        }
        public static void FillChunk(string tableName, int amountToSpawn)
        { for (int i = 0; i < amountToSpawn; i++) { CreateEntity(0, 0, SpawnTableManager.RetrieveRandomEntity(tableName, true), true, true); } }
        public static Entity CreateEntity(int x, int y, int uID, bool random, bool seeded = false)
        {
            Entity entity = JsonDataManager.ReturnEntity(uID);
            if (random)
            {
                x = 0; y = 0;
                bool sufficient = false;
                while (!sufficient)
                {
                    if (!CMath.CheckBounds(x, y) || !entity.GetComponent<Movement>().moveTypes.Contains(Map.map[x, y].moveType))
                    {
                        if (seeded)
                        {
                            x = World.seed.Next(0, Program.gameMapWidth);
                            y = World.seed.Next(0, Program.gameMapHeight);
                        }
                        else
                        {
                            x = CMath.random.Next(0, Program.gameMapWidth);
                            y = CMath.random.Next(0, Program.gameMapHeight);
                        }
                    }
                    else { sufficient = true; }
                }
            }
            AddEntity(entity);
            entity.GetComponent<Coordinate>().x = x;
            entity.GetComponent<Coordinate>().y = y;
            if (CMath.ReturnAI(entity) != null) { Map.map[x, y].actor = entity; }
            else { Map.map[x, y].item = entity; }
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
                AddEntity(entityToUse);
                return entityToUse;
            }
            return null;
        }
        public static void CreateNewEntityTest()
        {
            Entity entity = new Entity();
            entity.uID = 4;
            entity.AddComponent(new Stats(5, 14, .50f, 16, 5, 14, 2));
            entity.AddComponent(new Movement(true));
            entity.AddComponent(new Inventory());
            entity.AddComponent(new BodyPlot("Basic_Crustacean"));
            entity.AddComponent(new Faction("Crustacean"));
            entity.AddComponent(PronounReferences.pronounSets["Nueter"]);
            entity.AddComponent(new TurnFunction());
            entity.AddComponent(new OnHit());
            entity.AddComponent(new Draw("Red", "Black", 'c'));
            entity.AddComponent(new Description("Red*Boiler Red*Crab", "This crab has adopted the methods normally used to cook it, and instead has repurposed them to cook everything around it."));
            entity.AddComponent(new Coordinate(0, 0));
            entity.AddComponent(new ScavengerAI());
            CMath.ReturnAI(entity).hatedEntities.Add("Sea_Beast");

            Entity MightyClaw = new Entity();
            MightyClaw.AddComponent(new Equippable("Main_Hand", true, false));
            MightyClaw.AddComponent(new Draw("White", "Black", '/'));
            MightyClaw.AddComponent(new Description("Big Claw", "A large pinching claw."));
            MightyClaw.AddComponent(new AttackFunction(1, 8, 0, 0, "Piercing", "Melee"));
            MightyClaw.AddComponent(new Coordinate());
            MightyClaw.GetComponent<Equippable>().Equip(entity);

            Entity MiniClaw = new Entity();
            MiniClaw.AddComponent(new Equippable("Off_Hand", true, false));
            MiniClaw.AddComponent(new Draw("White", "Black", '/'));
            MiniClaw.AddComponent(new Description("Small Claw", "A small pinching claw."));
            MiniClaw.AddComponent(new AttackFunction(1, 4, 0, 0, "Piercing", "Melee"));
            MiniClaw.AddComponent(new Coordinate());
            MiniClaw.GetComponent<Equippable>().Equip(entity);

            JsonDataManager.SaveEntity(entity);
        }
    }
}
