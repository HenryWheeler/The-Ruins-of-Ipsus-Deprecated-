using System;
using System.Collections.Generic;
using System.Threading;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Program
    {
        private static readonly int screenWidth = 100;
        private static readonly int screenHeight = 50;
        public static Console rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int mapWidth = 65;
        private static readonly int mapHeight = 50;
        public static TitleConsole mapConsole;
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int messageWidth = 35;
        private static readonly int messageHeight = 15;
        public static TitleConsole logConsole;
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int rogueWidth = 35;
        private static readonly int rogueHeight = 35;
        public static TitleConsole playerConsole;

        public static Entity player;
        public static bool gameActive = false;

        public static int gameMapWidth = 100;
        public static int gameMapHeight = 100;
        public static void Main()
        {
            //RLRootConsole Rconsole = new RLRootConsole("ascii_6x6.png", 1, 1, 1, 1);


            // Setup the engine and create the main window.
            Settings.UseHardwareFullScreen = true;
            SadConsole.Game.Create("fonts/ascii_6x6.font.json", screenWidth, screenHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();

            //Rconsole.Close();
        }
        private static void Init()
        {
            //Settings.ToggleFullScreen();

            var console = new ContainerConsole();
            console.IsFocused = true;
            console.Components.Add(new KeyboardComponent());
            Global.CurrentScreen = console;
            mapConsole = new TitleConsole(" Map ", mapWidth, mapHeight) { Position = new Point(0, 0) };
            logConsole = new TitleConsole(" Message Log ", messageWidth, messageHeight) { Position = new Point(mapWidth, rogueHeight) };
            playerConsole = new TitleConsole(" The Rogue @ ", rogueWidth, rogueHeight) { Position = new Point(mapWidth, 0) };

            Global.CurrentScreen.Children.Add(mapConsole);
            Global.CurrentScreen.Children.Add(playerConsole);
            Global.CurrentScreen.Children.Add(logConsole);

            LoadFunctions();
        }
        public static void CloseGame(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gameActive = false;
            Renderer.running = false;
        }
        public static void LoadFunctions()
        {
            Renderer renderer = new Renderer(mapConsole, mapWidth, mapHeight);
            Log log = new Log(logConsole);
            Action action = new Action(playerConsole);
            //Menu update = new Menu(rootConsole);
            StatManager stats = new StatManager(playerConsole);
            SaveDataManager saveDataManager = new SaveDataManager();
            JsonDataManager jsonDataManager = new JsonDataManager();
            PronounReferences pronounReferences = new PronounReferences();
            SpawnTableManager spawnTableManager = new SpawnTableManager();
            DijkstraMaps dijkstraMaps = new DijkstraMaps(gameMapWidth, gameMapHeight);
            EntityManager.LoadAllEntities();

            NewGame();
        }
        public static void LoadPlayerFunctions(Entity player)
        {
            InventoryManager inventory = new InventoryManager(logConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);
        }
        public static void ReloadPlayer(List<Component> components)
        {
            player = EntityManager.ReloadEntity(new Entity(components));
            //Thread thread = new Thread(() => player.GetComponent<PlayerComponent>().Update());
            //thread.Start();
            //rootConsole.Update += player.GetComponent<PlayerComponent>().Update;

            Vector2 vector2 = player.GetComponent<Vector2>();
            World.tiles[vector2.x, vector2.y].actorLayer = player;
            StatManager.UpdateStats(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            //Action.PlayerAction(player);
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
            player.AddComponent(new PlayerComponent());
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

            //Action.PlayerAction(player);
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

            Renderer.DrawToScreen();
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
    public class TitleConsole : Console
    {
        public TitleConsole(string title, int _width, int _height)
            :  base(_width, _height)
        {
            Fill(Color.White, Color.Black, 176);

            TheRuinsOfIpsus.Renderer.CreateConsoleBorder(this, title);
        }
    }
}
