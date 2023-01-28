using System;
using System.Collections.Generic;
using RLNET;
using System.Threading;

namespace TheRuinsOfIpsus
{
    public class Program
    {
        private static readonly int screenWidth = 100;
        private static readonly int screenHeight = 50;
        public static RLRootConsole rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int mapWidth = 65;
        private static readonly int mapHeight = 50;
        private static RLConsole mapConsole;
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int messageWidth = 35;
        private static readonly int messageHeight = 15;
        public static RLConsole messageConsole;
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int rogueWidth = 35;
        private static readonly int rogueHeight = 35;
        public static RLConsole rogueConsole;

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
            settings.Scale = 1f;
            settings.Title = "The Ruins of Ipsus";
            settings.WindowBorder = RLWindowBorder.Resizable;
            settings.ResizeType = RLResizeType.ResizeScale;
            settings.StartWindowState = RLWindowState.Maximized;

            rootConsole = new RLRootConsole(settings);

            rootConsole.OnClosing += CloseGame;
            rootConsole.OnResize += OnResize;

            mapConsole = new RLConsole(mapWidth, mapHeight);
            messageConsole = new RLConsole(messageWidth, messageHeight);
            rogueConsole = new RLConsole(rogueWidth, rogueHeight);

            Thread thread = new Thread(() => LoadFunctions());
            thread.Start();

            rootConsole.Run(60);

            //thread.Join();
        }

        private static void OnResize(object sender, ResizeEventArgs e)
        {
            if (rootConsole.Height > 50 && rootConsole.Width > 100) { rootConsole.LoadBitmap("ascii_12x12.png", 12, 12); }
            else { rootConsole.LoadBitmap("ascii_6x6.png", 6, 6); }
        }

        public static void CloseGame(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gameActive = false;
            Renderer.running = false;
        }
        
        public static void LoadFunctions()
        {
            Renderer renderer = new Renderer(rootConsole, mapConsole, mapWidth, mapHeight, messageConsole, messageWidth, messageHeight, rogueConsole, rogueWidth, rogueHeight);
            Log log = new Log(messageConsole);
            Action action = new Action(rogueConsole);
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
            InventoryManager inventory = new InventoryManager(messageConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);
        }
        public static void ReloadPlayer(List<Component> components)
        {
            player = EntityManager.ReloadEntity(new Entity(components));
            Thread thread = new Thread(() => player.GetComponent<PlayerComponent>().Update());
            thread.Start();
            //rootConsole.Update += player.GetComponent<PlayerComponent>().Update;

            Vector2 vector2 = player.GetComponent<Vector2>();
            World.tiles[vector2.x, vector2.y].actorLayer = player;
            StatManager.UpdateStats(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            Action.PlayerAction(player);
            ShadowcastFOV.Compute(vector2, player.GetComponent<Stats>().sight);
            player.GetComponent<UpdateCameraOnMove>().Move(vector2, vector2);
            player.GetComponent<TurnFunction>().StartTurn();

            EntityManager.AddEntity(player);
        }
        public static void CreateNewPlayer()
        {
            player = new Entity();
            player.AddComponent(new ID(0));
            player.AddComponent(new Vector2(0, 0));
            player.AddComponent(new Draw("White", "Black", '@'));
            player.AddComponent(new Description("You", "It's you."));
            player.AddComponent(PronounReferences.pronounSets["Player"]);
            player.AddComponent(new Stats(10, 10, 1f, 50, 1, 1));
            player.AddComponent(new TurnFunction());
            player.AddComponent(new Movement(new List<int> { 1, 2 }));
            player.AddComponent(new Inventory());
            player.AddComponent(new Harmable());
            player.AddComponent(new Faction("Player"));
            player.AddComponent(new UpdateCameraOnMove());
            player.AddComponent(new PlayerComponent(rootConsole));
            Entity startingWeapon = new Entity(new List<Component>() 
            {
                new Vector2(0, 0),
                new ID(1100),
                new Draw("Orange", "Black", '!'),
                new Description("Potion of Orange*Explosion", "The label reads: 'Do Not Drink'."),
                new Usable("The potion explodes in a Red*fiery burst!"),
                new Throwable("The potion explodes in a Red*fiery burst!"),
                new ExplodeOnUse(6, 0),
                new ExplodeOnThrow(6),
            });
            InventoryManager.AddToInventory(player, new Entity(startingWeapon));
            InventoryManager.AddToInventory(player, new Entity(startingWeapon));
            InventoryManager.AddToInventory(player, new Entity(startingWeapon));
            InventoryManager.AddToInventory(player, new Entity(startingWeapon));

            Entity testScrollOfLightning = new Entity(new List<Component>()
            {
                new Vector2(0, 0),
                new ID(1300),
                new Draw("Yellow", "Black", '?'),
                new Description("Scroll of Yellow*Lightning", "This scroll is carved with Yellow*yellow Yellow*runes onto a vellum of human skin."),
                new Usable("A bolt of Yellow*lightning crackles and fries the air in front of the scroll!", false),
                new LightningOnUse(5, 15),
            });
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));
            InventoryManager.AddToInventory(player, new Entity(testScrollOfLightning));

            Entity testMagicMappingScroll = new Entity(new List<Component>()
            {
                new Vector2(0, 0),
                new ID(1301),
                new Draw("Cyan", "Black", '?'),
                new Description("Scroll of Cyan*Mapping", "This scroll seems as if lighter than air and feels charged with unearthly knowledge."),
                new Usable("The world around you becomes Cyan*clearer."),
                new MagicMapOnUse(),
            });
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));
            InventoryManager.AddToInventory(player, new Entity(testMagicMappingScroll));

            Entity djinnInABottle = new Entity(new List<Component>()
            {
                new Vector2(0, 0),
                new ID(1101),
                new Draw("Cyan", "Black", '!'),
                new Description("Cyan*Djinn in a Bottle", "This glass bottle is filled with a furious Cyan*Djinn."),
                new Usable("The bottle cracks open and a furious Cyan*Djinn emerges!"),
                new Throwable("The bottle cracks open and a furious Cyan*Djinn emerges!"),
                new SummonActorOnUse(new int[] { 75 }, 1, 0),
                new SummonActorOnThrow(new int[] { 75 }, 1),
            });
            InventoryManager.AddToInventory(player, new Entity(djinnInABottle));
            InventoryManager.AddToInventory(player, new Entity(djinnInABottle));
            InventoryManager.AddToInventory(player, new Entity(djinnInABottle));
            InventoryManager.AddToInventory(player, new Entity(djinnInABottle));

            Entity testPotionOfDragonsFire = new Entity(new List<Component>()
            {
                new Vector2(0, 0),
                new ID(1102),
                new Draw("Red", "Black", '!'),
                new Description("Potion of Red*Dragon's Red*Fire", "The label reads: 'Breathe with the fire of Red*Dragons!'"),
                new Usable($"A cone of Red*flame emits from your mouth!", false),
                new BreathWeaponOnUse(5, "Fire", 10),
            });
            InventoryManager.AddToInventory(player, new Entity(testPotionOfDragonsFire));
            InventoryManager.AddToInventory(player, new Entity(testPotionOfDragonsFire));
            InventoryManager.AddToInventory(player, new Entity(testPotionOfDragonsFire));
            InventoryManager.AddToInventory(player, new Entity(testPotionOfDragonsFire));
            InventoryManager.AddToInventory(player, new Entity(testPotionOfDragonsFire));

            Entity testTongueLash = new Entity(new List<Component>()
            {
                new Vector2(0, 0),
                new ID(1932),
                new Draw("Pink", "Black", '/'),
                new Description("Pink*Tongue Test", "Test"),
                new Usable($"A giant Pink*Tongue emits from your mouth!", false),
                new TongueLashOnUse(5, 100),
            });
            InventoryManager.AddToInventory(player, new Entity(testTongueLash));
            InventoryManager.AddToInventory(player, new Entity(testTongueLash));
            InventoryManager.AddToInventory(player, new Entity(testTongueLash));
            InventoryManager.AddToInventory(player, new Entity(testTongueLash));
            InventoryManager.AddToInventory(player, new Entity(testTongueLash));

            Action.PlayerAction(player);
            EntityManager.AddEntity(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            StatManager.UpdateStats(player);
            player.GetComponent<TurnFunction>().StartTurn();
        }
        public static void NewGame()
        {
            gameActive = true;
            World world = new World(gameMapWidth, gameMapHeight);
            CreateNewPlayer();
            World.GenerateNewFloor(true);
            LoadPlayerFunctions(player);
            Log.Add("Welcome to the Ruins of Ipsus");
            Log.DisplayLog();
            //EntityManager.CreateNewEntityTest();
        }
        public static void LoadSave(SaveData saveData)
        {
            EntityManager.LoadAllEntities();
            World world = new World(gameMapWidth, gameMapHeight, saveData.depth, saveData.seed);

            foreach (Entity actor in saveData.actors) { if (actor != null) { Entity entity = EntityManager.ReloadEntity(actor); World.tiles[entity.GetComponent<Vector2>().x, entity.GetComponent<Vector2>().y].actorLayer = entity; } }
            foreach (Entity item in saveData.items) { if (item != null) { Entity entity = EntityManager.ReloadEntity(item); World.tiles[entity.GetComponent<Vector2>().x, entity.GetComponent<Vector2>().y].itemLayer = entity; } }
            foreach (Entity terrain in saveData.terrain) { if (terrain != null) { Entity entity = EntityManager.ReloadEntity(terrain); World.tiles[entity.GetComponent<Vector2>().x, entity.GetComponent<Vector2>().y].obstacleLayer = entity; } }
            foreach (Traversable tile in World.tiles) { if (tile != null && tile.entity != null) { Vector2 vector2 = tile.entity.GetComponent<Vector2>(); if (saveData.visibility[vector2.x, vector2.y] != null) { tile.entity.RemoveComponent(tile.entity.GetComponent<Visibility>()); tile.entity.AddComponent(saveData.visibility[vector2.x, vector2.y]); } } }

            ReloadPlayer(saveData.player.components);
            LoadPlayerFunctions(player);
            RecordKeeper.record = saveData.records;
            Renderer.MoveCamera(player.GetComponent<Vector2>());
            Log.Add("Welcome to the Ruins of Ipsus");
            Log.DisplayLog();
            gameActive = true;
        }
    }
}
