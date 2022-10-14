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
        public static void PlayerAction(Player player, RLKey key)
        {
            DisplayActions("Move: NumPad & Arrow Keys. End Turn: Period. Look: L. Open Inventory: I. Get Item: G.");

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
            }
        }
        public static void InventoryAction(Player player, RLKey key)
        {
            DisplayActions("Close Inventory: I & Escape. Change Selection: NumPad & Arrow Keys. Drop Item: D");

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
            }
        }
        public static void LookAction(RLKey key)
        {
            DisplayActions("Stop Looking: L & Escape. Move: NumPad & Arrow Keys.");

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
            string[] outPut = actions.Split(' ');
            int y = 2;
            int c = 0;
            foreach (string text in outPut)
            {
                if (c + text.Length > console.Width - 4) { y += 2; c = 0; }
                console.Print(c + 2, y, text, RLColor.White);
                c += text.Length + 1;
            }
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
