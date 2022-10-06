using System;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Renderer
    {
        private RLRootConsole rootConsole;
        private RLConsole mapConsole;
        private RLConsole messageConsole;
        private RLConsole statConsole;
        private RLConsole inventoryConsole;
        public Renderer(RLRootConsole _rootConsole, RLConsole _mapConsole, RLConsole _messageConsole, RLConsole _statConsole, RLConsole _inventoryConsole)
        {
            rootConsole = _rootConsole;
            mapConsole = _mapConsole;
            messageConsole = _messageConsole;
            statConsole = _statConsole;
            inventoryConsole = _inventoryConsole;
            rootConsole.Render += Render;
        }
        public void Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            RenderMap();
            RenderLog();
            RenderStats();
            RenderInventory();
            rootConsole.Draw();
        }
        public void RenderMap()
        {
            RLConsole.Blit(mapConsole, 0, 0, 80, 70, rootConsole, 0, 0);
            foreach (Tile tile in Map.map) tile.Draw(mapConsole);
            CreateConsoleBorder(mapConsole);
            mapConsole.Print(37, 0, " Map ", RLColor.White);
        }
        public void RenderLog()
        {
            RLConsole.Blit(messageConsole, 0, 0, 155, 12, rootConsole, 0, 70);
            CreateConsoleBorder(messageConsole);
            messageConsole.Print(35, 0, " Message Log ", RLColor.White);
        }
        public void RenderStats()
        {
            RLConsole.Blit(statConsole, 0, 0, 41, 70, rootConsole, 80, 0);
            CreateConsoleBorder(statConsole);
            statConsole.Print(14, 0, " The Rogue @ ", RLColor.White);
        }
        public void RenderInventory()
        {
            RLConsole.Blit(inventoryConsole, 0, 0, 34, 70, rootConsole, 121, 0);
            CreateConsoleBorder(inventoryConsole);
            inventoryConsole.Print(11, 0, " Inventory ", RLColor.White);
        }
        public void CreateConsoleBorder(RLConsole console)
        {
            int h = console.Height - 1;
            int w = console.Width - 1;
            for (int y = (h); y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if ((y == (h) && x == 0) || (y == (h) && x == (w)) || (y == 0 && x == 0) || (x == (w) && y == 0) || (y == 0) || (y == (h)) || (x == 0) || (x == (w)))
                    {
                        console.Set(x, y, RLColor.White, RLColor.Black, '+');
                    }
                }
            }
        }
    }
}
