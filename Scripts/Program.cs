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
            SaveDataManager saveDataManager = new SaveDataManager();
            AIManager aiManager = new AIManager();

            JsonDataManager jsonDataManager = new JsonDataManager();

            rootConsole.Run();
        }
        public static void ReloadPlayer(Player _player)
        {
            player = _player;
            Stats stats = new Stats(rogueConsole, player);
            Inventory inventory = new Inventory(rogueConsole, player);
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);

            Map.map[player.x, player.y].actor = player;
            Stats.UpdateStats();
            TurnManager.AddActor(player);
            player.Clear();
            player.FOV();
            RLKey key = RLKey.Unknown;
            Action.PlayerAction(player, key);
            player.StartTurn();
        }
        public static void NewGame()
        {
            CMath cMath = new CMath(new Random().Next());
            MapGenerator.CreateMap(mapWidth, mapHeight);

            List<Tile> tiles = new List<Tile>();

            foreach (Tile tile in Map.map) { if (tile.moveType != 0) { tiles.Add(tile); } }

            Tile useTile = tiles[CMath.seed.Next(0, tiles.Count - 1)];

            player = new Player(rootConsole)
            {
                x = useTile.x,
                y = useTile.y,
            };
            Map.map[useTile.x, useTile.y].actor = player;
            player.FOV();
            TurnManager.AddActor(player);
            Stats stats = new Stats(rogueConsole, player);
            Stats.UpdateStats();
            Inventory inventory = new Inventory(rogueConsole, player);

            //List<int> move = new List<int>(); move.Add(1);
            //JsonDataManager.SaveActor(new Monster(1, 0, 0, 5, 5, 5, 'e', "Red", "Black", false, "Test Enemy", "A rowdy Test Enemy looking to pummel!", .8f, "Chase_AI", 30, "Basic_Humanoid", move, true));
            //JsonDataManager.SaveItem(new Item(3, 0, 0, 1, ')', "Wooden Bow", "A sturdy bow made of yew.", "Brown", 0, "Missile", 0, new AtkData("Wooden_Bow", 1, "1-4-0-0")));

            foreach (Tile tile in Map.map)
            {
                if (tile.moveType == 1 && CMath.seed.Next(450) == 5)
                {
                    Item armor = new Item(JsonDataManager.items[1], tile.x, tile.y);
                    Map.map[tile.x, tile.y].item = armor;
                }
                else if (tile.moveType == 1 && CMath.seed.Next(300) == 6)
                {
                    Item weapon = new Item(JsonDataManager.items[2], tile.x, tile.y);
                    Map.map[tile.x, tile.y].item = weapon;
                }
                else if (tile.moveType == 1 && CMath.seed.Next(300) == 7)
                {
                    Item weapon = new Item(JsonDataManager.items[3], tile.x, tile.y);
                    Map.map[tile.x, tile.y].item = weapon;
                }
                else if (tile.moveType == 1 && CMath.seed.Next(700) == 10)
                {
                    Monster monster = new Monster(JsonDataManager.actors[1], tile.x, tile.y, BodyPlots.bodyPlots[JsonDataManager.actors[1].bodyPlotName], new List<Item>(), new List<AtkData>());
                    Map.map[tile.x, tile.y].actor = monster;
                    TurnManager.AddActor(monster);
                }
                else if (tile.moveType == 1 && CMath.seed.Next(500) == 70)
                {
                    Monster frog = new Monster(JsonDataManager.actors[2], tile.x, tile.y, BodyPlots.bodyPlots[JsonDataManager.actors[2].bodyPlotName], new List<Item>(), new List<AtkData>());
                    Map.map[tile.x, tile.y].actor = frog;
                    TurnManager.AddActor(frog);
                }
            }
            Look look = new Look(player);
            TargetReticle reticle = new TargetReticle(player);
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);

            gameActive = true;
        }
        public static void LoadSave(SaveData saveData)
        {
            CMath cMath = new CMath(saveData.seed);
            MapGenerator.CreateMap(mapWidth, mapHeight);
            ReloadPlayer(saveData.player);

            foreach (VisibilitySaveData visData in saveData.visibility) { if (Map.map[visData.x, visData.y] != null && visData != null) { Map.map[visData.x, visData.y].explored = true; } }
            foreach (ActorSaveData actor in saveData.actors) 
            { 
                if (Map.map[actor.x, actor.y] != null && actor != null) 
                {
                    Monster actorX = new Monster(JsonDataManager.actors[actor.id], actor.x, actor.y, actor.bodyPlot, actor.inventory, actor.attacks);
                    Map.map[actor.x, actor.y].actor = actorX;
                    TurnManager.AddActor(actorX);
                } 
            }
            foreach (ItemSaveData item in saveData.items) 
            { 
                if (Map.map[item.x, item.y] != null && item != null) 
                {
                    Item itemX = new Item(JsonDataManager.items[item.id], item.x, item.y);
                    Map.map[item.x, item.y].item = itemX; 
                } 
            }

            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);

            gameActive = true;
        }
    }
}
