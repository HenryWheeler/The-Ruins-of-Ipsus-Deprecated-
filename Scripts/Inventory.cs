using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Inventory
    {
        private static RLConsole console;
        public static bool inventoryOpen = false;
        private static Player player;
        public static int selection = 0;
        public static int currentPage = 0;
        private static int maxItemPerPage = 25;
        public static List<List<ItemBase>> inventoryDisplay = new List<List<ItemBase>>();
        public Inventory(RLConsole _console, Player _player) { console = _console; player = _player; }
        public static bool GetItem(ActorBase actor, bool display = false)
        {
            Tile tile = Map.map[actor.x, actor.y];
            if (tile.item != null)
            {
                if (display) Log.AddToStoredLog("You picked up the " + tile.item.name + ".", true);
                AddToInventory(actor, tile.item); tile.item = null;
                return true;
            }
            else { if (display) Log.AddToStoredLog("There is nothing to pick up.", true); return false; }
        }
        public static bool DropItem(ActorBase actor, ItemBase item, bool display = false)
        {
            Tile tile = Map.map[actor.x, actor.y];
            if (tile.item == null)
            {
                if (display) Log.AddToStoredLog("You dropped the " + item.name + ".", true);
                RemoveFromInventory(actor, item);
                tile.item = item;
                tile.item.x = tile.x;
                tile.item.y = tile.y;
                if (actor.name == "Player") { CloseInventory(); }
                return true;
            }
            else { if (display) Log.AddToStoredLog("There is no room to drop this item.", true); return false; }
        }
        public static void AddToInventory(ActorBase actor, ItemBase item)
        {
            if (actor != null && item != null) actor.inventory.Add(item);
        }
        public static void RemoveFromInventory(ActorBase actor, ItemBase item)
        {
            if (actor != null && item != null) actor.inventory.Remove(item);
        }
        public static void OpenInventory()
        {
            inventoryOpen = true; player.turnActive = false; selection = 0; currentPage = 0;
            Stats.ClearStats();
            Renderer.CreateConsoleBorder(console); console.Print(15, 0, " Inventory ", RLColor.White);
            RLKey key = RLKey.Unknown; Action.InventoryAction(player, key);

            if (player.inventory.Count == 0) { console.Print(2, 2, "Inventory is Empty.", RLColor.White); }
            else { RefreshInventoryDisplay(); MoveSelection(0); }
        }
        public static void CloseInventory() 
        {
            inventoryDisplay.Clear();
            inventoryOpen = false; player.turnActive = true;
            Stats.DisplayStats(); 
            RLKey key = RLKey.Unknown; Action.PlayerAction(player, key); 
            Stats.UpdateStats(); 
        }
        public static void EquipItem(ActorBase actor, ItemBase item, bool display = false)
        {
            if (item.equipable)
            {

            }
            else { if (display) { Log.AddToStoredLog("You cannot equip this item.", true); } }
        }
        public static void UnequipItem(ActorBase actor, ItemBase item, bool display = false)
        {

        }
        public static void RefreshInventoryDisplay()
        {
            inventoryDisplay.Clear();
            int itemCount = 0;
            int pageCount = 0;
            inventoryDisplay.Add(new List<ItemBase>());
            foreach (ItemBase item in player.inventory)
            {
                if (itemCount == maxItemPerPage - 1) { itemCount = 0; pageCount++; inventoryDisplay.Add(new List<ItemBase>()); }
                inventoryDisplay[pageCount].Add(item);
                itemCount++;
            }
        }
        public static void DisplayInventory()
        {
            ClearInventoryDisplay();
            int x = 0;
            foreach (ItemBase item in inventoryDisplay[currentPage])
            {
                if (selection == x) console.Print(2, (x * 3) + 2, "X " + item.name, RLColor.White);
                else console.Print(2, (x * 3) + 2, item.name, RLColor.White);
                x++;
            }
            console.Print(2, 80, "Page: " + (currentPage + 1) + "/" + inventoryDisplay.Count, RLColor.White);
        }
        public static void MoveSelection(int move)
        {
            if (player.inventory.Count != 0)
            {
                if (selection + move > -1 && selection + move < inventoryDisplay[currentPage].Count) { selection += move; }
                else if (selection + move <= -1) { selection = inventoryDisplay[currentPage].Count - 1; }
                else if (selection + move >= inventoryDisplay[currentPage].Count) { selection = 0; }
                Log.AddToStoredLog(inventoryDisplay[currentPage][selection].Describe(), true);
                DisplayInventory();
            }
        }
        public static void MovePage(int move)
        {
            if (player.inventory.Count != 0)
            {
                if (inventoryDisplay.Count > 1)
                {
                    if (currentPage + move > -1 && currentPage + move < inventoryDisplay.Count) { currentPage += move; selection = 0; }
                    else if (currentPage + move <= -1) { currentPage = inventoryDisplay.Count - 1; selection = 0; }
                    else if (currentPage + move >= inventoryDisplay.Count) { currentPage = 0; selection = 0; }
                }
                Log.AddToStoredLog(inventoryDisplay[currentPage][selection].Describe(), true);
                DisplayInventory();
            }
        }
        public static void ClearInventoryDisplay()
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
