using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Renderer
    {
        private RLRootConsole rootConsole;
        private RLConsole mapConsole;
        private int mapWidth;
        private int mapHeight;
        private RLConsole messageConsole;
        private int messageWidth;
        private int messageHeight;
        private RLConsole rogueConsole;
        private int rogueWidth;
        private int rogueHeight;
        private RLConsole actionConsole;
        private int actionWidth;
        private int actionHeight;
        public static bool inventoryOpen = false;
        public Renderer(RLRootConsole _rootConsole, RLConsole _mapConsole, int _mapWidth, int _mapHeight, RLConsole _messageConsole, int _messageWidth, int _messageHeight, RLConsole _rogueConsole, int _rogueWidth, int _rogueHeight, RLConsole _actionConsole, int _actionWidth, int _actionHeight)
        {
            rootConsole = _rootConsole;
            mapConsole = _mapConsole;
            mapWidth = _mapWidth;
            mapHeight = _mapHeight;

            messageConsole = _messageConsole;
            messageWidth = _messageWidth;
            messageHeight = _messageHeight;

            rogueConsole = _rogueConsole;
            rogueWidth = _rogueWidth;
            rogueHeight = _rogueHeight;

            actionConsole = _actionConsole;
            actionWidth = _actionWidth;
            actionHeight = _actionHeight;
            rootConsole.Render += Render;

            CreateConsoleBorder(messageConsole);
            messageConsole.Print(11, 0, " Message Log ", RLColor.White);
            CreateConsoleBorder(rogueConsole);
            rogueConsole.Print(14, 0, " The Rogue @ ", RLColor.White);
            CreateConsoleBorder(actionConsole);
            actionConsole.Print(33, 0, " Actions ", RLColor.White);
        }
        public void Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            if (Program.gameActive)
            {
                RenderMap();
                RenderLog();
                RenderRogue();
                RenderAction();
            }
            else { RenderMenu(); }
            rootConsole.Draw();
        }
        public void RenderMenu()
        {
            CreateConsoleBorder(rootConsole);
            rootConsole.Print((rootConsole.Width / 2) - 9, (rootConsole.Height / 3) - 15, "The Ruins Of Ipsus", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 7, (rootConsole.Height / 2) - 6, "New Game: [N]", RLColor.White);
            if (SaveDataManager.savePresent) { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.White); }
            else { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.Gray); }
            rootConsole.Print((rootConsole.Width / 2) - 5, rootConsole.Height / 2, "Quit: [Q]", RLColor.White);
        }
        public void RenderMap()
        {
            RLConsole.Blit(mapConsole, 0, 0, mapWidth, mapHeight, rootConsole, 0, 0);
            foreach (Tile tile in Map.map)
            {
                Visibility vis = tile.GetComponent<Visibility>();
                Coordinate coordinate = tile.GetComponent<Coordinate>();
                if (Map.sfx[coordinate.x, coordinate.y] != null) { Map.sfx[coordinate.x, coordinate.y].GetComponent<Draw>().DrawToScreen(mapConsole); }
                else if (!vis.visible && !vis.explored) { mapConsole.Set(coordinate.x, coordinate.y, RLColor.Black, RLColor.Black, tile.GetComponent<Draw>().character); }
                else if (!vis.visible && vis.explored)
                { Draw draw = tile.GetComponent<Draw>();
                    mapConsole.Set(coordinate.x, coordinate.y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(draw.bColor), .55f), draw.character); }
                else if (tile.actor != null) { tile.actor.GetComponent<Draw>().DrawToScreen(mapConsole); }
                else if (tile.item != null) { tile.item.GetComponent<Draw>().DrawToScreen(mapConsole); }
                else { Draw draw = tile.GetComponent<Draw>(); 
                    mapConsole.Set(coordinate.x, coordinate.y, ColorFinder.ColorPicker(draw.fColor), ColorFinder.ColorPicker(draw.bColor), draw.character); }
            }
            CreateConsoleBorder(mapConsole);
            mapConsole.Print(37, 0, " Map ", RLColor.White);
        }
        public void RenderLog()
        {
            RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, mapWidth, 0);
        }
        public void RenderRogue()
        {
            RLConsole.Blit(rogueConsole, 0, 0, rogueWidth, rogueHeight, rootConsole, mapWidth + messageWidth, 0);
        }
        public void RenderAction()
        {
            RLConsole.Blit(actionConsole, 0, 0, actionWidth, actionHeight, rootConsole, 0, mapHeight);
        }
        public static void CreateConsoleBorder(RLConsole console)
        {
            int h = console.Height - 1;
            int w = console.Width - 1;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)192); }
                    else if (y == h && x == w) { console.Set(x, y, RLColor.White, RLColor.Black, (char)217); }
                    else if (y == 0 && x == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)218); }
                    else if (x == w && y == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)191); }
                    else if (y == 0 || y == h) { console.Set(x, y, RLColor.White, RLColor.Black, (char)196); }
                    else if (x == 0 || x == w) { console.Set(x, y, RLColor.White, RLColor.Black, (char)179); }
                }
            }
        }
    }
}
