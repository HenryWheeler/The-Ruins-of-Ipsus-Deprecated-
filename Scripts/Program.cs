using System;
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
            CMath cMath = new CMath(0);
            Action action = new Action(actionConsole);
            BodyPlots bodyPlots = new BodyPlots();
            Menu update = new Menu(rootConsole);

            rootConsole.Run();
        }
        public static void ReloadPlayer(Player _player)
        {
            Stats stats = new Stats(_player);
            Inventory inventory = new Inventory(_player);
            Look look = new Look(_player);

            Stats.DisplayStats();
            player = _player;
        }
        public static void NewGame()
        {
            MapGenerator.CreateMap(mapWidth, mapHeight, 5, 12, 15);
            Room room = MapGenerator.rooms[MapGenerator.random.Next(0, MapGenerator.rooms.Count - 1)];
            player = new Player(rootConsole)
            {
                x = room.x,
                y = room.y,
            };
            Map.map[room.x, room.y].actor = player;
            player.FOV();
            TurnManager.AddActor(player);
            Stats stats = new Stats(rogueConsole, player);
            Stats.UpdateStats();
            Inventory inventory = new Inventory(rogueConsole, player);

            foreach (Tile tile in Map.map)
            {
                if (tile.walkable && CMath.seed.Next(25) == 5)
                {
                    Item armor = new Item(tile.x, tile.y, 0, '[', "Unspecific Armor", "Who knows what it is?!", RLColor.Blue, 3, "Torso", 0, null);
                    Map.map[tile.x, tile.y].item = armor;
                }
                else if (tile.walkable && CMath.seed.Next(25) == 6)
                {
                    Item weapon = new Item(tile.x, tile.y, 1, ')', "Banana", "A curved yellow fruit", RLColor.Yellow, 0, "Main_Hand", 0, new AtkData("Banana", 0, "1-4-0-0"));
                    Map.map[tile.x, tile.y].item = weapon;
                }
                else if (tile.walkable && CMath.seed.Next(15) == 10)
                {
                    Monster monster = new Monster(tile.x, tile.y, 10, 10, 5, 'e', RLColor.Red, RLColor.Black, false, "Test Enemy", "A rowdy Test Enemy Looking for action!", .8f, new ChaseAI(), 20, "Basic_Humanoid");
                    Map.map[tile.x, tile.y].actor = monster;
                    TurnManager.AddActor(monster);
                }
            }
            Look look = new Look(player);
            Log.AddToStoredLog("Welcome to the Ruins of Ipsus", true);

            gameActive = true;
        }
    }
}
