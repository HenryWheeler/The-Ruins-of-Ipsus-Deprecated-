using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Program
    {
        private static readonly int screenWidth = 100;
        private static readonly int screenHeight = 50;
        public static RLRootConsole rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int mapWidth = 50;
        private static readonly int mapHeight = 50;
        private static RLConsole mapConsole;
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int messageWidth = 25;
        private static readonly int messageHeight = 35;
        private static RLConsole messageConsole;
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int rogueWidth = 25;
        private static readonly int rogueHeight = 50;
        private static RLConsole rogueConsole;

        private static readonly int actionWidth = 25;
        private static readonly int actionHeight = 15;
        private static RLConsole actionConsole;

        public static Entity player;
        public static bool gameActive = false;

        public static int gameMapWidth = 100;
        public static int gameMapHeight = 100;
        public static void Main()
        {
            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_6x6.png";
            settings.CharWidth = 6;
            settings.CharHeight = 6;
            settings.Width = screenWidth;
            settings.Height = screenHeight;
            settings.Scale = 3f;
            settings.Title = "The Ruins of Ipsus";
            settings.WindowBorder = RLWindowBorder.Hidden;
            settings.ResizeType = RLResizeType.None;
            settings.StartWindowState = RLWindowState.Maximized;
            
            rootConsole = new RLRootConsole(settings);
            mapConsole = new RLConsole(mapWidth, mapHeight);
            messageConsole = new RLConsole(messageWidth, messageHeight);
            rogueConsole = new RLConsole(rogueWidth, rogueHeight);
            actionConsole = new RLConsole(actionWidth, actionHeight);

            LoadFunctions();

            rootConsole.Run(60);
        }
        public static void LoadFunctions()
        {
            Renderer renderer = new Renderer(rootConsole, mapConsole, mapWidth, mapHeight, messageConsole, messageWidth, messageHeight, rogueConsole, rogueWidth, rogueHeight, actionConsole, actionWidth, actionHeight);
            Log log = new Log(messageConsole);
            Action action = new Action(actionConsole);
            Menu update = new Menu(rootConsole);
            StatManager stats = new StatManager(rogueConsole);
            SaveDataManager saveDataManager = new SaveDataManager();
            JsonDataManager jsonDataManager = new JsonDataManager();
            PronounReferences pronounReferences = new PronounReferences();
            SpawnTableManager spawnTableManager = new SpawnTableManager();
            DijkstraMaps dijkstraMaps = new DijkstraMaps(gameMapWidth, gameMapHeight);
            EntityManager.LoadAllEntities();
        }
        public static void LoadPlayerFunctions(Entity player)
        {
            InventoryManager inventory = new InventoryManager(rogueConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);
        }
        public static void ReloadPlayer(List<Component> components)
        {
            player = new Entity(components, true);
            rootConsole.Update += player.GetComponent<PlayerComponent>().Update;
            player.display = true;

            List<Entity> entities = new List<Entity>();
            foreach (Entity entity in player.GetComponent<Inventory>().inventory) { if (entity != null) { entities.Add(entity); } }
            player.GetComponent<Inventory>().inventory.Clear();
            foreach (Entity id in entities) { player.GetComponent<Inventory>().inventory.Add(new Entity(id)); }
            entities.Clear();
            foreach (EquipmentSlot entity in player.GetComponent<BodyPlot>().bodyPlot) { if (entity != null && entity.item != null) { entities.Add(entity.item); } }
            foreach (Entity id in entities) { new Entity(id).GetComponent<Equippable>().Equip(player); } 
            Vector2 vector2 = player.GetComponent<Coordinate>().vector2;
            World.tiles[vector2.x, vector2.y].actorLayer = player;
            StatManager.UpdateStats(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            Action.PlayerAction(player);
            ShadowcastFOV.Compute(vector2, player.GetComponent<Stats>().sight);
            player.GetComponent<UpdateCameraOnMove>().OnMove(vector2, vector2);
            player.GetComponent<TurnFunction>().StartTurn();

            EntityManager.AddEntity(player);
        }
        public static void CreateNewPlayer()
        {
            player = new Entity();
            player.display = true;
            player.AddComponent(new ID(0));
            player.AddComponent(new Coordinate(0, 0));
            player.AddComponent(new Draw("White", "Black", '@'));
            player.AddComponent(new Description("You", "It's you."));
            player.AddComponent(PronounReferences.pronounSets["Player"]);
            player.AddComponent(new Stats(7, 10, 1f, 50, 1, 1));
            player.AddComponent(new TurnFunction());
            player.AddComponent(new Movement(new List<int> { 1, 2 }));
            player.AddComponent(new Inventory());
            player.AddComponent(new BodyPlot());
            player.AddComponent(new OnHit());
            player.AddComponent(new Faction("Player"));
            player.AddComponent(new DijkstraProperty());
            player.AddComponent(new UpdateCameraOnMove());
            player.AddComponent(new PlayerComponent(rootConsole));
            Entity startingWeapon = new Entity(new List<Component>() 
            {
                new Coordinate(0, 0),
                new ID(1100),
                new Draw("Orange", "Black", '!'),
                new Description("Potion of Orange*Explosion", "The label reads: 'Do Not Drink'."),
                new Usable(),
                new Throwable(),
                new ExplodeOnUse(3),
                new ExplodeOnThrow(3)
            });
            InventoryManager.AddToInventory(player, startingWeapon);

            Entity testScrollOfLightning = new Entity(new List<Component>()
            {
                new Coordinate(0, 0),
                new ID(1300),
                new Draw("Yellow", "Black", '?'),
                new Description("Scroll of Yellow*Lightning", "This scroll is carved with Yellow*yellow Yellow*runes onto a vellum of human skin"),
                new Usable(),
                new LightningOnUse(5),
            });
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));

            Entity testMagicMappingScroll = new Entity(new List<Component>()
            {
                new Coordinate(0, 0),
                new ID(1301),
                new Draw("Cyan", "Black", '?'),
                new Description("Scroll of Cyan*Mapping", "This scroll seems as if lighter than air and feels charged with unearthly knowledge."),
                new Usable(),
                new MagicMapOnUse(),
            });
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));

            Action.PlayerAction(player);
            EntityManager.AddEntity(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            StatManager.UpdateStats(player);
            player.GetComponent<TurnFunction>().StartTurn();
        }
        public static void NewGame()
        {
            World world = new World(gameMapWidth, gameMapHeight);
            CreateNewPlayer();
            World.GenerateNewFloor(true);
            LoadPlayerFunctions(player);
            Log.Add("Welcome to the Ruins of Ipsus");
            Log.DisplayLog();
            //EntityManager.CreateNewEntityTest();

            gameActive = true;
        }
        public static void LoadSave(SaveData saveData)
        {
            EntityManager.LoadAllEntities();
            World world = new World(gameMapWidth, gameMapHeight, saveData.depth, saveData.seed);

            foreach (Entity actor in saveData.actors) { if (actor != null) { Entity entity = EntityManager.ReloadEntity(actor); World.GetTraversable(entity.GetComponent<Coordinate>().vector2).actorLayer = entity; } }
            foreach (Entity item in saveData.items) { if (item != null) { Entity entity = EntityManager.ReloadEntity(item); World.GetTraversable(entity.GetComponent<Coordinate>().vector2).itemLayer = entity; } }
            foreach (Entity terrain in saveData.terrain) { if (terrain != null) { Entity entity = EntityManager.ReloadEntity(terrain); World.GetTraversable(entity.GetComponent<Coordinate>().vector2).obstacleLayer = entity; } }
            foreach (Traversable tile in World.tiles) { if (tile != null && tile.entity != null) { Vector2 vector2 = tile.entity.GetComponent<Coordinate>().vector2; if (saveData.visibility[vector2.x, vector2.y] != null) { tile.entity.RemoveComponent(tile.entity.GetComponent<Visibility>()); tile.entity.AddComponent(saveData.visibility[vector2.x, vector2.y]); } } }

            ReloadPlayer(saveData.player.components);
            LoadPlayerFunctions(player);
            RecordKeeper.record = saveData.records;
            Renderer.MoveCamera(player.GetComponent<Coordinate>().vector2);
            Log.Add("Welcome to the Ruins of Ipsus");
            Log.DisplayLog();
            gameActive = true;
        }
    }
}
