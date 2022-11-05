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
        public static void PlayerAction(Player player, RLKey key = RLKey.Unknown)
        {
            DisplayActions("Move: [NumPad/Arrow Keys]" + "     Debug Save Game: [N]" + spacer + "End Turn: [.]"  + "     Debug Load Game: [M]" + "     Save & Quit: [J]" + spacer 
                + "Look: [L]" + "     Debug Reveal All: [V]" + spacer + "Open Inventory: [I]" + spacer + "Get Item: [G]" + "     Target: [T]");

            if (key != RLKey.Unknown)
            {
                Log.ClearLogDisplay();
                switch (key)
                {
                    case RLKey.Up: player.GetComponent<Movement>().Move(0, -1); break;
                    case RLKey.Down: player.GetComponent<Movement>().Move(0, 1); break;
                    case RLKey.Left: player.GetComponent<Movement>().Move(-1, 0); break;
                    case RLKey.Right:  player.GetComponent<Movement>().Move(1, 0); break;
                    case RLKey.Keypad8: player.GetComponent<Movement>().Move(0, -1); break;
                    case RLKey.Keypad9: player.GetComponent<Movement>().Move(1, -1); break;
                    case RLKey.Keypad6: player.GetComponent<Movement>().Move(1, 0); break;
                    case RLKey.Keypad3: player.GetComponent<Movement>().Move(1, 1); break;
                    case RLKey.Keypad2: player.GetComponent<Movement>().Move(0, 1); break;
                    case RLKey.Keypad1: player.GetComponent<Movement>().Move(-1, 1); break;
                    case RLKey.Keypad4: player.GetComponent<Movement>().Move(-1, 0); break;
                    case RLKey.Keypad7: player.GetComponent<Movement>().Move(-1, -1); break;
                    case RLKey.Period: player.GetComponent<TurnFunction>().EndTurn(); break;
                    case RLKey.L: Look.StartLooking(player.GetComponent<Coordinate>()); break;
                    case RLKey.I: InventoryManager.OpenInventory(); break;
                    case RLKey.G: InventoryManager.GetItem(player, true); break;
                    case RLKey.N: SaveDataManager.CreateDebugSave(player); Log.AddToStoredLog("Debug Saved!", true); break;
                    case RLKey.M: SaveDataManager.LoadDebugSave(player); Log.AddToStoredLog("Debug Loaded!", true); break;
                    case RLKey.J: SaveDataManager.CreateSave(player); Program.rootConsole.Close(); break;
                    case RLKey.V: foreach (Tile tile in Map.map) { if (tile != null) { Coordinate coordinate = tile.GetComponent<Coordinate>();
                                ShadowcastFOV.SetVisible(coordinate.x, coordinate.y, true, true); } } break;
                    case RLKey.T:
                        {
                            Log.ClearLogDisplay();
                            bool hasMissile = false;
                            foreach (EquipmentSlot slot in player.GetComponent<BodyPlot>().bodyPlot) { if (slot != null && slot.name == "Missile") { if (slot.item != null) { hasMissile = true; } } }
                            if (hasMissile) { TargetReticle.StartTargeting(); }
                            else { Log.AddToStoredLog("You have no missile to target with.", true); }
                            break;
                        }
                }
            }
        }
        public static void InventoryAction(Player player, RLKey key = RLKey.Unknown)
        {
            DisplayActions("Close Inventory: [I/Escape]" + spacer + "Change Selection: [NumPad/Arrow Keys]" + spacer + "Drop Item: [D]" + spacer + "Equip/Unequip: [E]");

            if (key != RLKey.Unknown)
            {
                Log.ClearLogDisplay();
                switch (key)
                {
                    case RLKey.I: InventoryManager.CloseInventory(); break;
                    case RLKey.Escape: InventoryManager.CloseInventory(); break;
                    case RLKey.Up: InventoryManager.MoveSelection(-1); break;
                    case RLKey.Down: InventoryManager.MoveSelection(1); break;
                    case RLKey.Keypad8: InventoryManager.MoveSelection(-1); break;
                    case RLKey.Keypad2: InventoryManager.MoveSelection(1); break;
                    case RLKey.Left: InventoryManager.MovePage(-1); break;
                    case RLKey.Keypad4: InventoryManager.MovePage(-1); break;
                    case RLKey.Right: InventoryManager.MovePage(1); break;
                    case RLKey.Keypad6: InventoryManager.MovePage(1); break;
                    case RLKey.D: if (player.GetComponent<Inventory>().inventory.Count != 0) { InventoryManager.DropItem(player, InventoryManager.inventoryDisplay[InventoryManager.currentPage][InventoryManager.selection], true); } break;
                    case RLKey.E:
                        {
                            if (player.GetComponent<Inventory>().inventory.Count != 0)
                            {
                                int first = InventoryManager.currentPage;
                                int second = InventoryManager.selection;
                                if (InventoryManager.inventoryDisplay[first][second].GetComponent<Equippable>().equipped) { InventoryManager.UnequipItem(player, InventoryManager.inventoryDisplay[first][second], true); }
                                else { InventoryManager.EquipItem(player, InventoryManager.inventoryDisplay[first][second], true); }
                            }
                            break;
                        }
                }
            }
        }
        public static void TargetAction(RLKey key = RLKey.Unknown)
        {
            DisplayActions("Stop Targeting: [T/Escape]" + spacer + "Move: [NumPad/Arrow Keys]" + spacer + "Fire: [F]");

            if (key != RLKey.Unknown)
            {
                switch (key)
                {
                    case RLKey.Up: Log.ClearLogDisplay(); TargetReticle.Move(0, -1); break;
                    case RLKey.Down: Log.ClearLogDisplay(); TargetReticle.Move(0, 1); break;
                    case RLKey.Left: Log.ClearLogDisplay(); TargetReticle.Move(-1, 0); break;
                    case RLKey.Right: Log.ClearLogDisplay(); TargetReticle.Move(1, 0); break;
                    case RLKey.Keypad8: Log.ClearLogDisplay(); TargetReticle.Move(0, -1); break;
                    case RLKey.Keypad9: Log.ClearLogDisplay(); TargetReticle.Move(1, -1); break;
                    case RLKey.Keypad6: Log.ClearLogDisplay(); TargetReticle.Move(1, 0); break;
                    case RLKey.Keypad3: Log.ClearLogDisplay(); TargetReticle.Move(1, 1); break;
                    case RLKey.Keypad2: Log.ClearLogDisplay(); TargetReticle.Move(0, 1); break;
                    case RLKey.Keypad1: Log.ClearLogDisplay(); TargetReticle.Move(-1, 1); break;
                    case RLKey.Keypad4: Log.ClearLogDisplay(); TargetReticle.Move(-1, 0); break;
                    case RLKey.Keypad7: Log.ClearLogDisplay(); TargetReticle.Move(-1, -1); break;
                    case RLKey.Escape: Log.ClearLogDisplay(); TargetReticle.StopTargeting(); break;
                    case RLKey.T: Log.ClearLogDisplay(); TargetReticle.StopTargeting(); break;
                    case RLKey.F: Log.ClearLogDisplay(); TargetReticle.Fire(); break;
                }
            }
        }
        public static void LookAction(RLKey key = RLKey.Unknown)
        {
            DisplayActions("Stop Looking: [L/Escape]" + spacer + "Move: [NumPad/Arrow Keys]");

            if (key != RLKey.Unknown)
            {
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
        }
        public static void DisplayActions(string actions) { CMath.ClearConsole(console); CMath.DisplayToConsole(console, actions, 0, 2); }
    }
}
