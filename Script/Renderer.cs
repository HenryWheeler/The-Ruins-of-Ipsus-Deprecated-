using System;
using RLNET;

namespace RoguelikeTest
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
            rootConsole.Update += OnRootConsoleUpdate;
        }
        private void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            statConsole.SetBackColor(0, 0, 45, 70, RLColor.Brown);
            statConsole.Print(1, 1, "Stats", RLColor.White);
            inventoryConsole.SetBackColor(0, 0, 30, 70, RLColor.Cyan);
            inventoryConsole.Print(1, 1, "Inventory", RLColor.White);
        }
        public void Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            RLConsole.Blit(mapConsole, 0, 0, 80, 70, rootConsole, 0, 0);
            RLConsole.Blit(statConsole, 0, 0, 45, 70, rootConsole, 80, 0);
            RLConsole.Blit(messageConsole, 0, 0, 155, 12, rootConsole, 0, 70);
            RLConsole.Blit(inventoryConsole, 0, 0, 30, 70, rootConsole, 125, 0);
            foreach (Tile tile in Map.map) tile.Draw(mapConsole);
            rootConsole.Draw();
        }
    }
}
