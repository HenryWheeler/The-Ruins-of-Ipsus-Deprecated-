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
        public static void GetItem(Entity entity)
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            Tile tile = Map.map[coordinate.x, coordinate.y];
            if (tile.item != null)
            {
                Entity itemRef = tile.item;
                if (entity.display) Log.AddToStoredLog("You picked up the " + tile.item.GetComponent<Description>().name + ".");
                AddToInventory(entity, tile.item); tile.item = null;
                entity.GetComponent<TurnFunction>().EndTurn();
                EntityManager.UpdateMap(itemRef);
            }
            else { if (entity.display) Log.AddToStoredLog("There is nothing to pick up.", true); }
        }
        public static void DropItem(Entity entity, Entity item)
        {
            Coordinate actorCoordinate = entity.GetComponent<Coordinate>();
            RemoveFromInventory(entity, item);
            Coordinate coordinate = Map.map[actorCoordinate.x, actorCoordinate.y].GetComponent<Coordinate>();
            PlaceItem(coordinate, item);
            if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) { UnequipItem(entity, item); }
            if (entity.display) { CloseInventory(); Log.AddToStoredLog("You dropped the " + item.GetComponent<Description>().name + "."); }
            entity.GetComponent<TurnFunction>().EndTurn();
        }
        public static void AddToInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null && actor.GetComponent<Inventory>().inventory != null) actor.GetComponent<Inventory>().inventory.Add(item);
        }
        public static void RemoveFromInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null) actor.GetComponent<Inventory>().inventory.Remove(item);
        }
        public static void OpenInventory()
        {
            inventoryOpen = true; player.GetComponent<TurnFunction>().turnActive = false; selection = 0; currentPage = 0;
            StatManager.ClearStats();
            Renderer.CreateConsoleBorder(console); console.Print((Renderer.rogueWidth / 2) - 5, 0, " Inventory ", RLColor.White);
            Action.InventoryAction(player);

            if (player.GetComponent<Inventory>().inventory.Count == 0) { console.Print(2, 2, "Inventory is Empty.", RLColor.White); }
            else  { Refresh(); MovePage(0); }
        }
        public static void CloseInventory()
        {
            inventoryDisplay.Clear();
            inventoryOpen = false; player.GetComponent<TurnFunction>().turnActive = true;
            Action.PlayerAction(player);
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
        public static void EquipItem(Entity entity, Entity item)
        {
            if (item.GetComponent<Equippable>() != null)
            {
                if (entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item != null)
                {
                    if (entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item.GetComponent<Equippable>().unequipable)
                    {
                        UnequipItem(entity, entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item);
                        item.GetComponent<Equippable>().Equip(entity);
                        if (entity.display) { Log.AddToStoredLog("You equip the " + item.GetComponent<Description>().name + "."); CloseInventory(); }
                        entity.GetComponent<TurnFunction>().EndTurn();
                    }
                    else if (entity.display) {
                        Log.AddToStoredLog("You cannot equip the " + item.GetComponent<Description>().name + "because the " + 
                            entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item.GetComponent<Description>().name + " cannot be unequipped.", true); }
                }
                else
                {
                    item.GetComponent<Equippable>().Equip(entity);
                    if (entity.display) { Log.AddToStoredLog("You equip the " + item.GetComponent<Description>().name + "."); CloseInventory(); }
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else if (entity.display) { Log.AddToStoredLog("You cannot equip the " + item.GetComponent<Description>().name + ".", true); }
        }
        public static void UnequipItem(Entity entity, Entity item)
        {
            if (item.GetComponent<Equippable>().unequipable)
            {
                item.GetComponent<Equippable>().Unequip(entity);
                if (entity.display) { Refresh(); Log.AddToStoredLog("You unequip the " + item.GetComponent<Description>().name + ".", true); }
            }
            else { Log.AddToStoredLog("You cannot unequip the " + item.GetComponent<Description>().name + ".", true); }
        }
        public static void UseItem(Entity entity, Entity item, bool inInventory)
        {
            if (inInventory)
            {
                entity.GetComponent<Inventory>().inventory.Remove(item);
                if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) { item.GetComponent<Equippable>().Unequip(entity); }
            }
            else { Coordinate coordinate = item.GetComponent<Coordinate>(); Map.map[coordinate.x, coordinate.y].item = null; }
            if (entity.display) { CloseInventory(); }
            item.GetComponent<Usable>().Use(entity);
            entity.GetComponent<TurnFunction>().EndTurn();
        }
        public static void PlaceItem(Coordinate targetCoordinate, Entity item)
        {
            Coordinate finalLanding = targetCoordinate;
            int itemMoveCount = 0;
            if (Map.map[targetCoordinate.x, targetCoordinate.y].item == null)
            {
                item.GetComponent<Coordinate>().x = targetCoordinate.x; item.GetComponent<Coordinate>().y = targetCoordinate.y;
                Map.map[targetCoordinate.x, targetCoordinate.y].item = item;
                EntityManager.UpdateMap(item); return;
            }
            do
            {
                Coordinate start = finalLanding;
                for (int y = start.y - 1; y <= start.y + 1; y++)
                {
                    for (int x = start.x - 1; x <= start.x + 1; x++)
                    {
                        if (CMath.CheckBounds(x, y) && Map.map[x, y].moveType != 0 && Map.map[x, y].item == null)
                        { 
                            item.GetComponent<Coordinate>().x = x; item.GetComponent<Coordinate>().y = y; 
                            Map.map[x, y].item = item;
                            EntityManager.UpdateMap(item); return;
                        }
                        else { finalLanding = new Coordinate(x, y); continue; }
                    }
                }
                itemMoveCount++;
                if (itemMoveCount >= 25) { break; }
            } while (Map.map[finalLanding.x, finalLanding.y].item != null);
        }
        public static void DisplayInventory()
        {
            CMath.ClearConsole(console);
            int x = 0;
            string output = "";
            foreach (Entity item in inventoryDisplay[currentPage])
            {
                string addOn = "";
                if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) { addOn = " - Equipped"; }
                if (selection == x) { output += "X " + item.GetComponent<Description>().name + addOn + " + "; } 
                else { output += item.GetComponent<Description>().name + addOn + " + "; } 
                x++;
            }
            CMath.DisplayToConsole(console, output, 2, 0, 0, 2);
            console.Print(7, 49, " Page:" + (currentPage + 1) + "/" + inventoryDisplay.Count + " ", RLColor.White);
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
            {
                string[] slot = inventoryDisplay[currentPage][selection].GetComponent<Equippable>().slot.Split();
                if (slot.Count() == 1) { addition += spacer + "Yellow*Can be equipped in " + "Yellow*" + slot[0] + "."; }
                else { addition += spacer + "Yellow*Can be equipped in " + "Yellow*" + slot[0] + " " + "Yellow*" + slot[1] + "."; }
                AttackFunction function = inventoryDisplay[currentPage][selection].GetComponent<AttackFunction>();
                if (function != null) 
                { addition += spacer + "Does Yellow*" + function.die1 + "d" + function.die2 + " damage with a damage modifier of Yellow*" + function.dmgModifier + " and a bonus to hit of Yellow*" + function.toHitModifier + "."; } }
            else { addition += spacer + "Yellow*Cannot be equipped."; }
            if (inventoryDisplay[currentPage][selection].GetComponent<Usable>() != null) { addition += spacer + "Yellow*Can be used."; }
            else { addition += spacer + "Yellow*Cannot be used."; }
            Log.AddToStoredLog(inventoryDisplay[currentPage][selection].GetComponent<Description>().Describe() + addition, true);
        }
    }
}
