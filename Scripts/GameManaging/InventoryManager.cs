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
        private static Entity player;
        public static int selection = 0;
        public static int currentPage = 0;
        private static int maxItemPerPage = 25;
        private static string spacer = " + + ";
        public static List<List<Entity>> inventoryDisplay = new List<List<Entity>>();
        public InventoryManager(RLConsole _console, Entity _player) { console = _console; player = _player; }
        public InventoryManager(Entity _player) { player = _player; }
        public static void GetItem(Entity entity)
        {
            Traversable traversable = World.GetTraversable(entity.GetComponent<Coordinate>().vector2);
            if (traversable.itemLayer != null)
            {
                Entity itemRef = traversable.itemLayer;
                if (entity.display)
                {
                    Log.Add($"You picked up the {traversable.itemLayer.GetComponent<Description>().name}.");
                }
                AddToInventory(entity, traversable.itemLayer); traversable.itemLayer = null;
                entity.GetComponent<TurnFunction>().EndTurn();
                EntityManager.UpdateMap(itemRef);
            }
            else 
            { 
                if (entity.display)
                {
                    Log.Add("There is nothing to pick up.");
                }
            }
        }
        public static void DropItem(Entity entity, Entity item)
        {
            RemoveFromInventory(entity, item);
            PlaceItem(entity.GetComponent<Coordinate>(), item);
            if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) 
            {
                UnequipItem(entity, item); 
            }
            if (entity.display) 
            { 
                CloseInventory(); 
                Log.Add($"You dropped the {item.GetComponent<Description>().name}."); 
            }
            entity.GetComponent<TurnFunction>().EndTurn();
        }
        public static void AddToInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null && actor.GetComponent<Inventory>().inventory != null)
            {
                actor.GetComponent<Inventory>().inventory.Add(item);
            }
        }
        public static void RemoveFromInventory(Entity actor, Entity item)
        {
            if (actor != null && item != null)
            {
                actor.GetComponent<Inventory>().inventory.Remove(item);
            }
        }
        public static void OpenInventory()
        {
            CMath.ClearConsole(console);
            CMath.DisplayToConsole(console, "", 0, 0);
            inventoryOpen = true; 
            player.GetComponent<TurnFunction>().turnActive = false; 
            selection = 0; currentPage = 0;
            StatManager.ClearStats();
            console.Print((Renderer.rogueWidth / 2) - 5, 0, " Inventory ", RLColor.White);
            Action.InventoryAction(player);

            if (player.GetComponent<Inventory>().inventory.Count == 0) 
            {
                console.Print(2, 2, "Inventory is Empty.", RLColor.White); 
            }
            else  
            { 
                Refresh(); 
                MovePage(0); 
            }
        }
        public static void CloseInventory()
        {
            inventoryDisplay.Clear();
            inventoryOpen = false; player.GetComponent<TurnFunction>().turnActive = true;
            Action.PlayerAction(player);
            StatManager.UpdateStats(player);
            Log.DisplayLog();
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
                        if (entity.display)
                        {
                            Log.Add($"You equip the {item.GetComponent<Description>().name}.");
                            CloseInventory();
                        }
                        entity.GetComponent<TurnFunction>().EndTurn();
                    }
                    else if (entity.display)
                    {
                        Log.Add($"You cannot equip the {item.GetComponent<Description>().name} because the {entity.GetComponent<BodyPlot>().ReturnSlot(item.GetComponent<Equippable>().slot).item.GetComponent<Description>().name} cannot be unequipped.");
                    }
                }
                else
                {
                    item.GetComponent<Equippable>().Equip(entity);
                    if (entity.display)
                    {
                        Log.Add($"You equip the {item.GetComponent<Description>().name}.");
                        CloseInventory();
                    }
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else if (entity.display)
            {
                Log.Add($"You cannot equip the {item.GetComponent<Description>().name} ."); 
            }
        }
        public static void UnequipItem(Entity entity, Entity item, bool display = false)
        {
            if (item.GetComponent<Equippable>().unequipable)
            {
                item.GetComponent<Equippable>().Unequip(entity);
                if (entity.display && display)
                {
                    CloseInventory();
                    Log.Add($"You unequip the {item.GetComponent<Description>().name}.");
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
            }
            else if (entity.display)
            {
                Log.Add($"You cannot unequip the {item.GetComponent<Description>().name}."); 
            }
        }
        public static void UseItem(Entity entity, Entity item)
        {
            entity.GetComponent<Inventory>().inventory.Remove(item);
            if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped)
            {
                item.GetComponent<Equippable>().Unequip(entity);
            }
            if (entity.display) { CloseInventory(); }
            item.GetComponent<Usable>().Use(entity, null);
            if (!TargetReticle.targeting)
            {
                entity.GetComponent<TurnFunction>().EndTurn();
            }
        }
        public static void PlaceItem(Coordinate targetCoordinate, Entity item)
        {
            Vector2 finalLanding = targetCoordinate.vector2;
            Vector2 targetVector3 = targetCoordinate.vector2;
            int itemMoveCount = 0;
            if (World.GetTraversable(targetVector3).itemLayer == null)
            {
                item.GetComponent<Coordinate>().vector2 = targetVector3;
                World.GetTraversable(targetVector3).itemLayer = item;
                EntityManager.UpdateMap(item); return;
            }
            do
            {
                Vector2 start = finalLanding;
                for (int y = start.y - 1; y <= start.y + 1; y++)
                {
                    for (int x = start.x - 1; x <= start.x + 1; x++)
                    {
                        Traversable traversable = World.GetTraversable(new Vector2(x, y));
                        if (CMath.CheckBounds(x, y) && traversable.terrainType != 0 && traversable.itemLayer == null)
                        {
                            item.GetComponent<Coordinate>().vector2 = new Vector2(x, y);
                            traversable.itemLayer = item;
                            EntityManager.UpdateMap(item);
                            return;
                        }
                        else 
                        { 
                            finalLanding = new Vector2(x, y); 
                            continue; 
                        }
                    }
                }
                itemMoveCount++;
                if (itemMoveCount >= 25) 
                { 
                    break;
                }
            } while (World.GetTraversable(new Vector2(finalLanding.x, finalLanding.y)).itemLayer != null);
        }
        public static void DisplayInventory()
        {
            CMath.ClearConsole(console);
            int x = 0;
            string output = "";
            foreach (Entity item in inventoryDisplay[currentPage])
            {
                string addOn = "";
                if (item.GetComponent<Equippable>() != null && item.GetComponent<Equippable>().equipped) 
                { 
                    addOn = " - Equipped"; 
                }
                if (selection == x) 
                {
                    output += "X " + item.GetComponent<Description>().name + addOn + " + "; 
                } 
                else 
                {
                    output += item.GetComponent<Description>().name + addOn + " + "; 
                } 
                x++;
            }
            CMath.DisplayToConsole(console, output, 2, 0, 0, 2);
            console.Print(7, 49, " Page:" + (currentPage + 1) + "/" + inventoryDisplay.Count + " ", RLColor.White);
        }
        public static void MoveSelection(int move)
        {
            if (player.GetComponent<Inventory>().inventory.Count != 0)
            {
                if (selection + move > -1 && selection + move < inventoryDisplay[currentPage].Count) 
                {
                    selection += move; 
                }
                else if (selection + move <= -1)
                { 
                    selection = inventoryDisplay[currentPage].Count - 1;
                }
                else if (selection + move >= inventoryDisplay[currentPage].Count)
                {
                    selection = 0; 
                }
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
                    if (currentPage + move > -1 && currentPage + move < inventoryDisplay.Count)
                    { 
                        currentPage += move; selection = 0;
                    }
                    else if (currentPage + move <= -1) 
                    {
                        currentPage = inventoryDisplay.Count - 1; selection = 0;
                    }
                    else if (currentPage + move >= inventoryDisplay.Count)
                    { 
                        currentPage = 0; selection = 0; 
                    }
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
                if (slot.Count() == 1) 
                {
                    addition += $"{spacer}Yellow*Can be equipped in Yellow*{slot[0]}."; 
                }
                else 
                { 
                    addition += $"{spacer}Yellow*Can be equipped in Yellow*{slot[0]} Yellow*{slot[1]}."; 
                }

                if (inventoryDisplay[currentPage][selection].GetComponent<AttackFunction>() != null)
                {
                    string[] function = inventoryDisplay[currentPage][selection].GetComponent<AttackFunction>().details.Split('-');
                    addition += $"{spacer} Does Yellow*{function[1]}d{function[2]} damage with a damage modifier of Yellow*{function[3]} and a bonus to hit of Yellow*{function[4]}.";
                }
            }
            else 
            {
                addition += $"{spacer}Yellow*Cannot be equipped."; 
            }
            if (inventoryDisplay[currentPage][selection].GetComponent<Usable>() != null) 
            { 
                addition += $"{spacer}Yellow*Can be used.";
            }
            else
            { 
                addition += $"{spacer}Yellow*Cannot be used.";
            }
            CMath.DisplayToConsole(Log.console, inventoryDisplay[currentPage][selection].GetComponent<Description>().Describe() + addition, 1, 1);
        }
    }
}
