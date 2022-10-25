using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Action
    {
        private static RLConsole console { get; set; }
        public Action(RLConsole _console) { console = _console; }
        private static string spacer = " + ";
        public static void MenuAction(RLKey key, RLRootConsole console)
        {
            switch (key)
            {
                case RLKey.N: Menu.MakeSelection(0); break;
                case RLKey.L: Menu.MakeSelection(1); break;
                case RLKey.Q: Menu.MakeSelection(2); break;
            }
        }
        public static void PlayerAction(Player player, RLKey key)
        {
            DisplayActions("Move: [NumPad/Arrow Keys]" + "     Debug Save Game: [N]" + spacer + "End Turn: [.]"  + "     Debug Load Game: [M]" + "     Save & Quit: [J]" + spacer 
                + "Look: [L]" + "     Debug Reveal All: [V]" + spacer + "Open Inventory: [I]" + spacer + "Get Item: [G]");

            switch (key)
            {
                case RLKey.Up: Log.ClearLogDisplay(); player.Move(0, -1); break;
                case RLKey.Down: Log.ClearLogDisplay(); player.Move(0, 1); break;
                case RLKey.Left: Log.ClearLogDisplay(); player.Move(-1, 0); break;
                case RLKey.Right: Log.ClearLogDisplay(); player.Move(1, 0); break;
                case RLKey.Keypad8: Log.ClearLogDisplay(); player.Move(0, -1); break;
                case RLKey.Keypad9: Log.ClearLogDisplay(); player.Move(1, -1); break;
                case RLKey.Keypad6: Log.ClearLogDisplay(); player.Move(1, 0); break;
                case RLKey.Keypad3: Log.ClearLogDisplay(); player.Move(1, 1); break;
                case RLKey.Keypad2: Log.ClearLogDisplay(); player.Move(0, 1); break;
                case RLKey.Keypad1: Log.ClearLogDisplay(); player.Move(-1, 1); break;
                case RLKey.Keypad4: Log.ClearLogDisplay(); player.Move(-1, 0); break;
                case RLKey.Keypad7: Log.ClearLogDisplay(); player.Move(-1, -1); break;
                case RLKey.Period: Log.ClearLogDisplay(); player.EndTurn(); break;
                case RLKey.L: Log.ClearLogDisplay(); Look.StartLooking(player.x, player.y); break;
                case RLKey.I: Log.ClearLogDisplay(); Inventory.OpenInventory(); break;
                case RLKey.G: Log.ClearLogDisplay(); if (Inventory.GetItem(player, true)) player.EndTurn(); break;
                case RLKey.N: Log.ClearLogDisplay(); SaveDataManager.CreateDebugSave(player); Log.AddToStoredLog("Debug Saved!", true); break;
                case RLKey.M: Log.ClearLogDisplay(); SaveDataManager.LoadDebugSave(player); Log.AddToStoredLog("Debug Loaded!", true); break;
                case RLKey.J: Log.ClearLogDisplay(); SaveDataManager.CreateSave(player); Program.rootConsole.Close(); break;
                case RLKey.V: Log.ClearLogDisplay(); foreach(Tile tile in Map.map) { if (tile != null) { tile.visible = true; tile.explored = true; } } break;
            }
        }
        public static void InventoryAction(Player player, RLKey key)
        {
            DisplayActions("Close Inventory: [I/Escape]" + spacer + "Change Selection: [NumPad/Arrow Keys]" + spacer + "Drop Item: [D]" + spacer + "Equip/Unequip: [E]");

            switch (key)
            {
                case RLKey.I: Log.ClearLogDisplay(); Inventory.CloseInventory(); break;
                case RLKey.Escape: Log.ClearLogDisplay(); Inventory.CloseInventory(); break;
                case RLKey.Up: Log.ClearLogDisplay(); Inventory.MoveSelection(-1); break;
                case RLKey.Down: Log.ClearLogDisplay(); Inventory.MoveSelection(1); break;
                case RLKey.Keypad8: Log.ClearLogDisplay(); Inventory.MoveSelection(-1); break;
                case RLKey.Keypad2: Log.ClearLogDisplay(); Inventory.MoveSelection(1); break;
                case RLKey.Left: Log.ClearLogDisplay(); Inventory.MovePage(-1); break;
                case RLKey.Keypad4: Log.ClearLogDisplay(); Inventory.MovePage(-1); break;
                case RLKey.Right: Log.ClearLogDisplay(); Inventory.MovePage(1); break;
                case RLKey.Keypad6: Log.ClearLogDisplay(); Inventory.MovePage(1); break;
                case RLKey.D: Log.ClearLogDisplay(); if (player.inventory.Count != 0) { Inventory.DropItem(player, Inventory.inventoryDisplay[Inventory.currentPage][Inventory.selection], true); } break;
                case RLKey.E:
                    {
                        Log.ClearLogDisplay(); if (player.inventory.Count != 0)
                        {
                            int first = Inventory.currentPage;
                            int second = Inventory.selection; 
                            if (Inventory.inventoryDisplay[first][second].equipped) { Inventory.UnequipItem(player, Inventory.inventoryDisplay[first][second], true); }
                            else { Inventory.EquipItem(player, Inventory.inventoryDisplay[first][second], true); }
                        }
                        break;
                    }
            }
        }
        public static void LookAction(RLKey key)
        {
            DisplayActions("Stop Looking: [L/Escape]" + spacer + "Move: [NumPad/Arrow Keys]");

            switch (key)
            {
                case RLKey.Up: Log.ClearLogDisplay(); Look.Move(0, -1); break;
                case RLKey.Down: Log.ClearLogDisplay(); Look.Move(0, 1); break;
                case RLKey.Left: Log.ClearLogDisplay(); Look.Move(-1, 0); break;
                case RLKey.Right: Log.ClearLogDisplay(); Look.Move(1, 0); break;
                case RLKey.Keypad8: Log.ClearLogDisplay(); Look.Move(0, -1); break;
                case RLKey.Keypad9: Log.ClearLogDisplay(); Look.Move(1, -1); break;
                case RLKey.Keypad6: Log.ClearLogDisplay(); Look.Move(1, 0); break;
                case RLKey.Keypad3: Log.ClearLogDisplay(); Look.Move(1, 1); break;
                case RLKey.Keypad2: Log.ClearLogDisplay(); Look.Move(0, 1); break;
                case RLKey.Keypad1: Log.ClearLogDisplay(); Look.Move(-1, 1); break;
                case RLKey.Keypad4: Log.ClearLogDisplay(); Look.Move(-1, 0); break;
                case RLKey.Keypad7: Log.ClearLogDisplay(); Look.Move(-1, -1); break;
                case RLKey.Escape: Log.ClearLogDisplay(); Look.StopLooking(); break;
                case RLKey.L: Log.ClearLogDisplay(); Look.StopLooking(); break;
            }
        }
        public static void DisplayActions(string actions)
        {
            ClearActionDisplay();
            CMath.DisplayToConsole(console, actions, 0, 2);
        }
        public static void ClearActionDisplay()
        {
            int h = console.Height - 2;
            int w = console.Width - 2;
            for (int y = (h); y >= 2; y--)
            {
                for (int x = 1; x < w + 1; x++)
                {
                    console.SetColor(x, y, RLColor.Black);
                }
            }
        }
    }
}
