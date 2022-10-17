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
        public static List<List<Item>> inventoryDisplay = new List<List<Item>>();
        public Inventory(RLConsole _console, Player _player) { console = _console; player = _player; }
        public Inventory(Player _player) { player = _player; }
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
        public static bool DropItem(ActorBase actor, Item item, bool display = false)
        {
            Tile tile = Map.map[actor.x, actor.y];
            if (tile.item == null)
            {
                RemoveFromInventory(actor, item);
                tile.item = item;
                tile.item.x = tile.x;
                tile.item.y = tile.y;
                if (item.equipped) UnequipItem(actor, item, display);
                if (display) { Refresh(); Log.AddToStoredLog("You dropped the " + item.name + ".", true); }
                return true;
            }
            else { if (display) Log.AddToStoredLog("There is no room to drop this item.", true); return false; }
        }
        public static void AddToInventory(ActorBase actor, Item item)
        {
            if (actor != null && item != null) actor.inventory.Add(item);
        }
        public static void RemoveFromInventory(ActorBase actor, Item item)
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
            else  { Refresh(); }
        }
        public static void CloseInventory()
        {
            inventoryDisplay.Clear();
            inventoryOpen = false; player.turnActive = true;
            Stats.DisplayStats();
            RLKey key = RLKey.Unknown; Action.PlayerAction(player, key);
            Stats.UpdateStats();
        }
        public static void Refresh()
        {
            inventoryDisplay.Clear();
            int itemCount = 0;
            int pageCount = 0;
            inventoryDisplay.Add(new List<Item>());
            foreach (Item item in player.inventory)
            {
                if (itemCount == maxItemPerPage - 1) { itemCount = 0; pageCount++; inventoryDisplay.Add(new List<Item>()); }
                inventoryDisplay[pageCount].Add(item);
                itemCount++;
            }
            DisplayInventory();
        }
        public static bool EquipItem(ActorBase actor, Item item, bool display = false)
        {
            foreach (EquipmentSlot slot in actor.bodyPlot)
            {
                if (slot.name == item.slot)
                {
                    if (slot != null) UnequipItem(actor, slot.item, display);
                    slot.item = item;
                    item.equipped = true;

                    switch (item.type)
                    {
                        case 0: actor.ac += item.ac; break;
                        case 1: actor.attacks.Add(item.atkData); break;
                        case 2: break;
                        case 3: break;
                    }

                    if (display) { Refresh(); Log.AddToStoredLog("You equipped " + item.name, true); }
                    return true;
                }
            }
            if (display) { Log.AddToStoredLog("You cannot unequip this item.", true); }
            return false;
        }
        public static bool UnequipItem(ActorBase actor, Item item, bool display = false)
        {
            foreach (EquipmentSlot slot in actor.bodyPlot)
            {
                if (item != null)
                {
                    if (slot.name == item.slot)
                    {
                        slot.item = null;
                        item.equipped = false;

                        switch (item.type)
                        {
                            case 0: actor.ac -= item.ac; break;
                            case 1: actor.attacks.Remove(item.atkData); break;
                            case 2: break;
                            case 3: break;
                        }

                        if (display) { Refresh(); Log.AddToStoredLog("You unequipped " + item.name, true); }
                        return true;
                    }
                }
            }
            if (display) { Log.AddToStoredLog("You cannot equip this item.", true); }
            return false;
        }
        public static void DisplayInventory()
        {
            CMath.ClearConsole(console);
            int x = 0;
            foreach (Item item in inventoryDisplay[currentPage])
            {
                string addOn = "";
                if (item.equipped) addOn = " - Equipped"; 
                if (selection == x) console.Print(2, (x * 3) + 2, "X " + item.name + addOn, RLColor.White);
                else console.Print(2, (x * 3) + 2, item.name + addOn, RLColor.White);
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
    }
}
