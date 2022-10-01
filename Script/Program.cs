using System;
using RLNET;

namespace RoguelikeTest
{
    public class Program
    {
        private static readonly int screenWidth = 155;
        private static readonly int screenHeight = 82;
        private static RLRootConsole rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int mapWidth = 80;
        private static readonly int mapHeight = 70;
        private static RLConsole mapConsole;
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int messageWidth = 155;
        private static readonly int messageHeight = 12;
        private static RLConsole messageConsole;
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int statWidth = 45;
        private static readonly int statHeight = 70;
        private static RLConsole statConsole;
        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int inventoryWidth = 30;
        private static readonly int inventoryHeight = 70;
        private static RLConsole inventoryConsole;

        public static Player player;
        public static void Main()
        {
            rootConsole = new RLRootConsole("ascii_8x8.png", screenWidth, screenHeight, 8, 8, 1.5f, "RogueLike Test");

            mapConsole = new RLConsole(mapWidth, mapHeight);
            messageConsole = new RLConsole(messageWidth, messageHeight);
            statConsole = new RLConsole(statWidth, statHeight);
            inventoryConsole = new RLConsole(inventoryWidth, inventoryHeight);

            Renderer renderer = new Renderer(rootConsole, mapConsole, messageConsole, statConsole, inventoryConsole);
            rootConsole.SetWindowState(RLWindowState.Maximized);

            MapGenerator.CreateMap(mapWidth, mapHeight, 5, 12, 15, new Random());
            Room room = MapGenerator.rooms[MapGenerator.random.Next(0, MapGenerator.rooms.Count - 1)];
            player = new Player(rootConsole)
            {
                x = room.x,
                y = room.y,
            };
            Map.map[room.x, room.y].actor = player;
            player.FOV();
            TurnManager.AddActor(player);

            MonsterData monsterData = new MonsterData()
            {
                x = room.x + 1,
                y = room.y + 1,
                character = 'E',
                bColor = RLColor.Black,
                fColor = RLColor.Red,
                sight = 5,
                speedCap = 1,
                hpCap = 10,
                name = "Enemy",
                opaque = false,
                ai = new ChaseAI(),
            };
            Monster monster = new Monster(monsterData);
            Map.map[room.x + 1, room.y + 1].actor = monster;
            TurnManager.AddActor(monster);
            rootConsole.Run();
        }
    }
}
