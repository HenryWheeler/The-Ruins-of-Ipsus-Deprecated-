using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Program
    {
        private static readonly int screenWidth = 155;
        private static readonly int screenHeight = 82;
        public static RLRootConsole rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int mapWidth = 80;
        private static readonly int mapHeight = 70;
        private static RLConsole mapConsole;
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int messageWidth = 34;
        private static readonly int messageHeight = 82;
        private static RLConsole messageConsole;
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int rogueWidth = 41;
        private static readonly int rogueHeight = 82;
        private static RLConsole rogueConsole;

        private static readonly int actionWidth = 80;
        private static readonly int actionHeight = 12;
        private static RLConsole actionConsole;

        public static Player player;
        public static bool gameActive = false;
        public static void Main()
        {

            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = screenWidth;
            settings.Height = screenHeight;
            settings.Scale = 1.5f;
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

            rootConsole.Run(60);
        }
        public static void ReloadPlayer(List<Component> components)
        {
            player = new Player(components);
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
            player.GetComponent<TurnFunction>().StartTurn();

            EntityManager.AddEntity(player);
        }
        public static void NewGame()
        {
            CMath cMath = new CMath(new Random().Next());
            MapGenerator.CreateMap(mapWidth, mapHeight);

            List<Tile> tiles = new List<Tile>();
            foreach (Tile tile in Map.map) { if (tile.moveType == 1) { tiles.Add(tile); } }
            Coordinate useTile = tiles[CMath.seed.Next(0, tiles.Count - 1)].GetComponent<Coordinate>();
            player = new Player(null);
            Map.map[useTile.x, useTile.y].actor = player;
            Coordinate playerCoordinate = player.GetComponent<Coordinate>();
            playerCoordinate.x = useTile.x; playerCoordinate.y = useTile.y;
            TurnManager.AddActor(player.GetComponent<TurnFunction>());
            StatManager.UpdateStats(player);
            InventoryManager inventory = new InventoryManager(rogueConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);

            EntityManager.AddEntity(player);

            foreach (Tile tile in Map.map)
            {
                if (tile.moveType == 1 && CMath.seed.Next(450) == 5)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Entity test = EntitySpawner.CreateEntity(coordinate.x, coordinate.y, 3, 1);
                }
                else if (tile.moveType == 1 && CMath.seed.Next(450) == 6)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Entity test = EntitySpawner.CreateEntity(coordinate.x, coordinate.y, 3, 0);
                    test.RemoveComponent(test.GetComponent<Movement>()); test.AddComponent(new Movement(true));
                    test.GetComponent<SpiderAI>().hatedEntities.Add("Restrained");
                }
                else if (tile.moveType == 1 && CMath.seed.Next(450) == 7)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    EntitySpawner.CreateEntity(coordinate.x, coordinate.y, 1, 1);
                }
                else if (tile.moveType == 1 && CMath.seed.Next(450) == -10)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Entity frog = EntitySpawner.CreateEntity(coordinate.x, coordinate.y, 2, 0);
                }
                else if (tile.moveType == 1 && CMath.seed.Next(450) == 800)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Entity chaosFrog = new Entity();
                    chaosFrog.AddComponent(new Coordinate(coordinate.x, coordinate.y));
                    chaosFrog.AddComponent(new Draw("Red", "Black", 'f'));
                    chaosFrog.AddComponent(new Description("Red*Chaos Red*Frog", "It is very Red*angry."));
                    chaosFrog.AddComponent(PronounReferences.pronounSets["Nueter"]);
                    chaosFrog.AddComponent(new Stats(7, 10, .5f, 500, 10, 10, false));
                    chaosFrog.AddComponent(new TurnFunction(chaosFrog.GetComponent<Stats>().maxAction, false));
                    chaosFrog.AddComponent(new Movement(true));
                    chaosFrog.AddComponent(new Inventory(false));
                    chaosFrog.AddComponent(new BodyPlot("Basic_Creature"));
                    chaosFrog.AddComponent(new Visibility(false, false, false));
                    chaosFrog.AddComponent(new OnHit());
                    chaosFrog.AddComponent(new ChaseAI(10));
                    //chaosFrog.GetComponent<ChaseAI>().hatedEntities.Add("You");
                    //chaosFrog.GetComponent<ChaseAI>().hatedEntities.Add("Dark_Purple*Blacklog Dark_Purple*Spider");

                    TurnManager.AddActor(chaosFrog.GetComponent<TurnFunction>());
                    Map.map[coordinate.x, coordinate.y].actor = chaosFrog;
                }
                else if (tile.moveType == 2 && CMath.seed.Next(450) == 9)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    EntitySpawner.CurrrentTestThing(coordinate.x, coordinate.y);
                }
            }

            ShadowcastFOV.Compute(playerCoordinate.x, playerCoordinate.y, player.GetComponent<Stats>().sight);
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);
            player.GetComponent<TurnFunction>().StartTurn();

            gameActive = true;
        }
        public static void LoadSave(SaveData saveData)
        {
            CMath cMath = new CMath(saveData.seed);
            MapGenerator.CreateMap(mapWidth, mapHeight);
            foreach (Tile tile in saveData.tiles)
            {
                if (tile != null)
                {
                    Coordinate coordinate = tile.GetComponent<Coordinate>();
                    Map.map[coordinate.x, coordinate.y] = tile;
                    if (tile.actor != null && tile.actor != player) { tile.actor = EntitySpawner.ReloadEntity(tile.actor); }
                    else { tile.actor = null; }
                    if (tile.item != null) { tile.item = EntitySpawner.ReloadEntity(tile.actor); }
                    else { tile.item = null; }
                    if (tile.terrain != null) { tile.terrain = EntitySpawner.ReloadEntity(tile.terrain); }
                    else { tile.terrain = null; }
                }
            }
            ReloadPlayer(saveData.player.components);
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);
            gameActive = true;
        }
    }
}
