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

        public static Player player;
        public static bool gameActive = false;

        public static int gameMapWidth = 50;
        public static int gameMapHeight = 50;
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

            Renderer renderer = new Renderer(rootConsole, mapConsole, mapWidth, mapHeight, messageConsole, messageWidth, messageHeight, rogueConsole, rogueWidth, rogueHeight, actionConsole, actionWidth, actionHeight);
            Log log = new Log(messageConsole);
            Action action = new Action(actionConsole);
            BodyPlots bodyPlots = new BodyPlots();
            Menu update = new Menu(rootConsole);
            StatManager stats = new StatManager(rogueConsole);
            SaveDataManager saveDataManager = new SaveDataManager();
            JsonDataManager jsonDataManager = new JsonDataManager();
            PronounReferences pronounReferences = new PronounReferences();
            SpawnTableManager spawnTableManager = new SpawnTableManager();

            rootConsole.Run(60);
        }
        public static void ReloadPlayer(List<Component> components)
        {
            player = new Player(components);
            rootConsole.Update += player.GetComponent<PlayerComponent>().Update;
            player.display = true;

            InventoryManager inventory = new InventoryManager(rogueConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);

            List<Entity> entities = new List<Entity>();
            foreach (Entity entity in player.GetComponent<Inventory>().inventory) { if (entity != null) { entities.Add(entity); } }
            player.GetComponent<Inventory>().inventory.Clear();
            foreach (Entity id in entities) { player.GetComponent<Inventory>().inventory.Add(new Entity(id)); }
            entities.Clear();
            foreach (EquipmentSlot entity in player.GetComponent<BodyPlot>().bodyPlot) { if (entity != null && entity.item != null) { entities.Add(entity.item); } }
            foreach (Entity id in entities) { new Entity(id).GetComponent<Equippable>().Equip(player); } 
            Coordinate coordinate = player.GetComponent<Coordinate>();
            Map.map[coordinate.x, coordinate.y].actor = player;
            StatManager.UpdateStats(player);
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            Action.PlayerAction(player);
            ShadowcastFOV.Compute(coordinate.x, coordinate.y, player.GetComponent<Stats>().sight);
            player.GetComponent<UpdateCameraOnMove>().OnMove(coordinate.x, coordinate.y, coordinate.x, coordinate.y);
            player.GetComponent<TurnFunction>().StartTurn();

            EntityManager.AddEntity(player);
        }
        public static void NewGame()
        {
            EntityManager entityManager = new EntityManager();

            World world = new World(mapWidth, mapHeight, 1);

            List<Tile> tiles = new List<Tile>();
            foreach (Tile tile in Map.map) { if (tile != null && tile.moveType != 0) { tiles.Add(tile); } }
            Coordinate useTile = tiles[CMath.seed.Next(0, tiles.Count - 1)].GetComponent<Coordinate>();
            player = new Player(null);
            player.display = true;
            Map.map[useTile.x, useTile.y].actor = player;
            Coordinate playerCoordinate = player.GetComponent<Coordinate>();
            playerCoordinate.x = useTile.x; playerCoordinate.y = useTile.y;
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            StatManager.UpdateStats(player);
            InventoryManager inventory = new InventoryManager(rogueConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);

            //EntitySpawner.CreateNewEntityTest();

            EntityManager.AddEntity(player);

            ShadowcastFOV.Compute(playerCoordinate.x, playerCoordinate.y, player.GetComponent<Stats>().sight);
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);
            player.GetComponent<TurnFunction>().StartTurn();
            EntityManager.UpdateAll();
            Renderer.MoveCamera(playerCoordinate);
            gameActive = true;
        }
        public static void LoadSave(SaveData saveData)
        {
            EntityManager entityManager = new EntityManager();
            World world = new World(mapWidth, mapHeight, 1, saveData.seed, false);

            foreach (Tile tile in saveData.tiles)
            {
                if (tile != null)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Map.map[coordinate.x, coordinate.y] = tile;
                    if (tile.actor != null && tile.actor != player) { tile.actor = EntityManager.ReloadEntity(tile.actor); }
                    else { tile.actor = null; }
                    if (tile.item != null) { tile.item = EntityManager.ReloadEntity(tile.actor); }
                    else { tile.item = null; }
                    if (tile.terrain != null) { tile.terrain = EntityManager.ReloadEntity(tile.terrain); }
                    else { tile.terrain = null; }
                }
            }
            ReloadPlayer(saveData.player.components);
            EntityManager.UpdateAll();
            Renderer.MoveCamera(player.GetComponent<Coordinate>());
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);
            gameActive = true;
        }
    }
}
