using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class InventoryManager
    {
        private static RLConsole console;
        public static bool inventoryOpen = false;
        private static Player player;
        public static int selection = 0;
        public static int currentPage = 0;
        private static int maxItemPerPage = 25;
        private static string spacer = " + + ";
        public static List<List<Entity>> inventoryDisplay = new List<List<Entity>>();
        public InventoryManager(RLConsole _console, Player _player) { console = _console; player = _player; }
        public InventoryManager(Player _player) { player = _player; }
        public static void GetItem(Entity entity, bool display = false)
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            Tile tile = Map.map[coordinate.x, coordinate.y];
            if (tile.item != null)
            {
                if (display) Log.AddToStoredLog("You picked up the " + tile.item.GetComponent<Description>().name + ".");
                AddToInventory(entity, tile.item); tile.item = null;
                entity.GetComponent<TurnFunction>().EndTurn();
            }
            else { if (display) Log.AddToStoredLog("There is nothing to pick up.", true); }
        }
        public static void DropItem(Entity entity, Entity item, bool display = false)
        {
            Coordinate actorCoordinate = entity.GetComponent<Coordinate>();
            Coordinate itemCoordinate = item.GetComponent<Coordinate>();
            Tile tile = Map.map[actorCoordinate.x, actorCoordinate.y];
            if (tile.item == null)
            {
                RemoveFromInventory(entity, item);
                tile.item = item;
                Coordinate coordinate = tile.GetComponent<Coordinate>();
                itemCoordinate.x = coordinate.x;
                itemCoordinate.y = coordinate.y;
                if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) { UnequipItem(entity, item, display); }
                if (display) { CloseInventory(); Log.AddToStoredLog("You dropped the " + item.GetComponent<Description>().name + "."); }
                entity.GetComponent<TurnFunction>().EndTurn();
            }
            else { if (display) Log.AddToStoredLog("There is no room to drop this item.", true); }
        }
        public static void AddToInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null) actor.GetComponent<Inventory>().inventory.Add(item);
        }
        public static void RemoveFromInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null) actor.GetComponent<Inventory>().inventory.Remove(item);
        }
        public static void OpenInventory()
        {
            inventoryOpen = true; player.GetComponent<TurnFunction>().turnActive = false; selection = 0; currentPage = 0;
            StatManager.ClearStats();
            Renderer.CreateConsoleBorder(console); console.Print(15, 0, " Inventory ", RLColor.White);
            Action.InventoryAction(player);

            if (player.GetComponent<Inventory>().inventory.Count == 0) { console.Print(2, 2, "Inventory is Empty.", RLColor.White); }
            else  { Refresh(); MovePage(0); }
        }
        public static void CloseInventory()
        {
            inventoryDisplay.Clear();
            inventoryOpen = false; player.GetComponent<TurnFunction>().turnActive = true;
            StatManager.DisplayStats();
            RLKey key = RLKey.Unknown; Action.PlayerAction(player, key);
            StatManager.UpdateStats(player);
        }
        public static void Refresh()
        {
            inventoryDisplay.Clear();
            int itemCount = 0;
            int pageCount = 0;
            inventoryDisplay.Add(new List<Entity>());
            foreach (Entity item in player.GetComponent<Inventory>().inventory)
            {
                if (itemCount == maxItemPerPage - 1) { itemCount = 0; pageCount++; inventoryDisplay.Add(new List<Entity>()); }
                inventoryDisplay[pageCount].Add(item);
                itemCount++;
            }
            DisplayInventory();
        }
        public static void EquipItem(Entity entity, Entity item, bool display = false)
        {
            if (item.GetComponent<Equippable>() != null)
            {
                if (entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item != null)
                {
                    UnequipItem(entity, entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item, display);
                }
                item.GetComponent<Equippable>().Equip(entity);
                if (display) { Log.AddToStoredLog("You equip the " + item.GetComponent<Description>().name + "."); CloseInventory(); }
                entity.GetComponent<TurnFunction>().EndTurn();
            }
            else if (display) { Log.AddToStoredLog("You cannot equip the " + item.GetComponent<Description>().name + ".", true); }
        }
        public static void UnequipItem(Entity entity, Entity item, bool display = false)
        {
            item.GetComponent<Equippable>().Unequip(entity);
            if (display) { Refresh(); Log.AddToStoredLog("You unequip the " + item.GetComponent<Description>().name + ".", true); }
        }
        public static void DisplayInventory()
        {
            CMath.ClearConsole(console);
            int x = 0;
            foreach (Entity item in inventoryDisplay[currentPage])
            {
                string addOn = "";
                if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) { addOn = " - Equipped"; } 
                if (selection == x) console.Print(2, (x * 3) + 2, "X " + item.GetComponent<Description>().name + " [" + item.GetComponent<Draw>().character + "]" + addOn, RLColor.White);
                else console.Print(2, (x * 3) + 2, item.GetComponent<Description>().name + " [" + item.GetComponent<Draw>().character + "]" + addOn, RLColor.White);
                x++;
            }
            console.Print(2, 80, "Page: " + (currentPage + 1) + "/" + inventoryDisplay.Count, RLColor.White);
        }
        public static void MoveSelection(int move)
        {
            if (player.GetComponent<Inventory>().inventory.Count != 0)
            {
                if (selection + move > -1 && selection + move < inventoryDisplay[currentPage].Count) { selection += move; }
                else if (selection + move <= -1) { selection = inventoryDisplay[currentPage].Count - 1; }
                else if (selection + move >= inventoryDisplay[currentPage].Count) { selection = 0; }
                DisplayItem();
                DisplayInventory();
            }
        }
        public static void MovePage(int move)
        {
            if (player.GetComponent<Inventory>().inventory.Count != 0)
            {
                if (inventoryDisplay.Count > 1)
                {
                    if (currentPage + move > -1 && currentPage + move < inventoryDisplay.Count) { currentPage += move; selection = 0; }
                    else if (currentPage + move <= -1) { currentPage = inventoryDisplay.Count - 1; selection = 0; }
                    else if (currentPage + move >= inventoryDisplay.Count) { currentPage = 0; selection = 0; }
                }
                DisplayItem();
                DisplayInventory();
            }
        }
        public static void DisplayItem()
        {
            string addition = "";
            if (inventoryDisplay[currentPage][selection].GetComponent<Equippable>() != null) 
            { addition += spacer + "Can be equipped in " + inventoryDisplay[currentPage][selection].GetComponent<Equippable>().slot + "."; AttackFunction function = inventoryDisplay[currentPage][selection].GetComponent<AttackFunction>();
                if (function != null) 
                { addition += spacer + "Does " + function.die1 + "d" + function.die2 + " damage with a damage modifier of " + function.dmgModifier + " and a bonus to hit of " + function.toHitModifier + "."; } }
            else { addition += spacer + "Cannot be equipped."; }
            if (inventoryDisplay[currentPage][selection].GetComponent<Usable>() != null) { addition += spacer + "Can be used."; }
            else { addition += spacer + "Cannot be used."; }
            Log.AddToStoredLog(inventoryDisplay[currentPage][selection].GetComponent<Description>().Describe() + addition, true);
        }
    }
}
