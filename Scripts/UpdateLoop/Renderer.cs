using System;
using RLNET;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace TheRuinsOfIpsus
{
    public class Renderer
    {
        private static RLRootConsole rootConsole;
        public static RLConsole mapConsole;
        public static int mapWidth;
        public static int mapHeight;
        private static RLConsole messageConsole;
        public static int messageWidth;
        private static int messageHeight;
        private static RLConsole rogueConsole;
        public static int rogueWidth;
        private static int rogueHeight;
        private static RLConsole actionConsole;
        public static int actionWidth;
        private static int actionHeight;
        public static bool inventoryOpen = false;
        public static bool threadRunning = false;
        public static int minX { get; set; }
        public static int maxX { get; set; }
        public static int minY { get; set; }
        public static int maxY { get; set; }
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
        }
        public static bool CheckIfInRenderDistance(Coordinate coordinate) { if (coordinate.x > maxX || coordinate.x < minX || coordinate.y > maxY || coordinate.y < minY) { return false; } else { return true; } }
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
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 14, " ______  _             ____         __                   ___   __                          ", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 13, "|__  __|| |__   ____  |__  | __ __ |__| ___  ____   ___ | __| |  | ____  ____  __ __  ____ ", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 12, "  |  |  | |  | | __ | |   _||  |  ||  ||   ||  __| | _ ||  _| |  || __ ||  __||  |  ||  __|", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 11, "  |  |  |  _  || ___| | | | |  |  ||  || | ||___ | ||_||| |   |  ||  __||___ ||  |  ||___ |", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 10, "  |__|  |_| |_||____| |_|_| |_____||__||_|_||____| |___||_|   |__||_|   |____||_____||____|", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 7, (rootConsole.Height / 2) - 6, "New Game: [N]", RLColor.White);
            if (SaveDataManager.savePresent) { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.White); }
            else { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.Gray); }
            rootConsole.Print((rootConsole.Width / 2) - 5, rootConsole.Height / 2, "Quit: [Q]", RLColor.White);
        }
        public static void StartAnimationThread(List<SFX> sfx, int repeatCount, int delay)
        {
            if (!threadRunning)
            {
                Thread thread = new Thread(() => PlayAnimation(sfx, repeatCount, delay));
                thread.Start();
            }
            else
            {
                Thread thread = new Thread(() => WaitList(sfx, repeatCount, delay));
                thread.Start();
            }
        }
        public static void WaitList(List<SFX> sfx, int repeatCount, int delay)
        {
            try
            {
                while (threadRunning)
                {
                    Thread.Sleep(10);
                }
                Thread thread = new Thread(() => PlayAnimation(sfx, repeatCount, delay));
                thread.Start();
                Thread thisThread = Thread.CurrentThread;
                thisThread.Abort();
            }
            catch (Exception ex) { ex = null; return; }
        }
        public static void PlayAnimation(List<SFX> sfx, int repeatCount, int delay)
        {
            try
            {
                threadRunning = true;
                TurnManager.threadRunning = true;
                int current = 0;
                do
                {
                    foreach (SFX frame in sfx)
                    {
                        if (frame != null)
                        {
                            Coordinate coordinate = frame.GetComponent<Coordinate>();
                            Map.sfx[coordinate.x, coordinate.y] = frame;
                            frame.GetComponent<AnimationFunction>().ProgressFrame();
                        }
                    }
                    //RenderMap();
                    Thread.Sleep(delay);
                    current++;
                } while (current != repeatCount);
                foreach (SFX frame in sfx)
                {
                    if (frame != null)
                    {
                        Coordinate coordinate = frame.GetComponent<Coordinate>();
                        Map.sfx[coordinate.x, coordinate.y] = null;
                    }
                }
                threadRunning = false;
                TurnManager.threadRunning = false;
                Thread thread = Thread.CurrentThread;
                thread.Abort();
            }
            catch (Exception ex) { ex = null; return; }
        }
        public static void MoveCamera(Coordinate currentPosition)
        {
            minX = currentPosition.x - mapWidth / 2;
            maxX = minX + mapWidth;
            minY = currentPosition.y - mapHeight / 2;
            maxY = minY + mapHeight;
        }
        public static void RenderMap()
        {
            //mapConsole.Clear();
            RLConsole.Blit(mapConsole, 0, 0, mapWidth, mapHeight, rootConsole, messageWidth, 0);

            int y = 0;
            for (int ty = minY; ty < maxY; ty++)
            {
                int x = 0;
                for (int tx = minX; tx < maxX; tx++)
                {
                    if (CMath.CheckBounds(tx, ty))
                    {
                        Tile tile = Map.map[tx, ty];
                        Visibility visibility = tile.GetComponent<Visibility>();
                        if (Map.sfx[tx, ty] != null) { Map.sfx[tx, ty].GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (!visibility.visible && !visibility.explored) { mapConsole.Set(x, y, RLColor.Black, RLColor.Black, tile.GetComponent<Draw>().character); }
                        else if (!visibility.visible && visibility.explored) { Draw draw = tile.GetComponent<Draw>(); mapConsole.Set(x, y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(draw.bColor), .55f), draw.character); }
                        else if (tile.actor != null) { tile.actor.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (tile.item != null) { tile.item.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (tile.terrain != null) { tile.terrain.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else { tile.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                    }
                    else { mapConsole.Set(x, y, ColorFinder.ColorPicker("Gray"), ColorFinder.ColorPicker("Black"), '+'); }
                    x++;
                }
                y++;
            }
            CreateConsoleBorder(mapConsole);
            mapConsole.Print((mapWidth / 2) - 3, 0, " Map ", RLColor.White);
        }
        public static void RenderLog()
        {
            RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, 0, 0);
        }
        public static void RenderRogue()
        {
            RLConsole.Blit(rogueConsole, 0, 0, rogueWidth, rogueHeight, rootConsole, mapWidth + messageWidth, 0);
        }
        public static void RenderAction()
        {
            RLConsole.Blit(actionConsole, 0, 0, actionWidth, actionHeight, rootConsole, 0, messageHeight);
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
