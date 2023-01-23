﻿using System;
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
        private static int maxItemPerPage = 6;
        private static string spacer = " + + ";
        public static List<List<Entity>> inventoryDisplay = new List<List<Entity>>();
        public InventoryManager(RLConsole _console, Entity _player) { console = _console; player = _player; }
        public static void GetItem(Entity entity)
        {
            Vector2 vector2 = entity.GetComponent<Coordinate>().vector2;
            Traversable traversable = World.GetTraversable(vector2);
            if (traversable.itemLayer != null)
            {
                Entity itemRef = traversable.itemLayer;
                if (entity.display)
                {
                    Log.Add($"You picked up the {traversable.itemLayer.GetComponent<Description>().name}.");

                    Log.OutputParticleLog(itemRef.GetComponent<Description>().name, itemRef.GetComponent<Draw>().fColor, vector2);

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
            inventoryOpen = true; 
            player.GetComponent<TurnFunction>().turnActive = false; 
            selection = 0; currentPage = 0;
            StatManager.ClearStats();

            if (player.GetComponent<Inventory>().inventory.Count == 0) 
            {
                console.Print(2, 2, "Inventory is Empty.", RLColor.White);
                //Action.InventoryAction(player);
            }
            else  
            {
                Refresh();
                DisplayItem();
                DisplayInventory();
                //Action.InventoryAction(player, RLKey.Unknown);
                //CMath.DisplayToConsole(Log.console, "Welcome to your inventory!", 1, 1);
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
                if (entity.GetComponent<Inventory>().ReturnSlot(item.GetComponent<Equippable>().slot).item != null)
                {
                    if (entity.GetComponent<Inventory>().ReturnSlot(item.GetComponent<Equippable>().slot).item.GetComponent<Equippable>().unequipable)
                    {
                        UnequipItem(entity, entity.GetComponent<Inventory>().ReturnSlot(item.GetComponent<Equippable>().slot).item);
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
                        Log.Add($"You cannot equip the {item.GetComponent<Description>().name} because the {entity.GetComponent<Inventory>().ReturnSlot(item.GetComponent<Equippable>().slot).item.GetComponent<Description>().name} cannot be unequipped.");
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
            Vector2 placement = CMath.ReturnNearestValidCoordinate("Item", targetCoordinate.vector2);
            item.GetComponent<Coordinate>().vector2 = placement;
            World.tiles[placement.x, placement.y].itemLayer = item;
            EntityManager.UpdateMap(item);

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
            console.Print(12, 13, " Page:" + (currentPage + 1) + "/" + inventoryDisplay.Count + " ", RLColor.White);
            console.Print((Renderer.messageWidth / 2) - 5, 0, " Inventory ", RLColor.White);
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
                    addition += $"{spacer}Yellow*Can be equipped/unequipped in Yellow*{slot[0]} with Yellow*[E]."; 
                }
                else 
                { 
                    addition += $"{spacer}Yellow*Can be equipped/unequipped in Yellow*{slot[0]} Yellow*{slot[1]} with Yellow*[E]."; 
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
                addition += $"{spacer}Yellow*Can be used with Yellow*[U].";
            }
            else
            { 
                addition += $"{spacer}Yellow*Cannot be used.";
            }
            addition += $"{spacer}Yellow*Can be thrown with Yellow*[T].";

            Description description = inventoryDisplay[currentPage][selection].GetComponent<Description>();
            CMath.DisplayToConsole(Program.rogueConsole, description.description + addition, 1, 1);
            string[] nameParts = description.name.Split(' ');
            string name = "";
            foreach (string part in nameParts)
            {
                string[] temp = part.Split('*');
                if (temp.Length == 1)
                {
                    name += temp[0] + " ";
                }
                else
                {
                    name += temp[1] + " ";
                }
            }
            int start = 17 - (int)Math.Ceiling((double)name.Length / 2);

            Program.rogueConsole.Print(start, 0, " ", RLColor.White, RLColor.Black);

            start++;

            foreach (string part in nameParts)
            {
                string[] temp = part.Split('*');
                if (temp.Length == 1)
                {
                    Program.rogueConsole.Print(start, 0, temp[0] + " ", RLColor.White, RLColor.Black);
                    start += temp[0].Length + 1;
                }
                else
                {
                    Program.rogueConsole.Print(start, 0, temp[1] + " ", ColorFinder.ColorPicker(temp[0]), RLColor.Black);
                    start += temp[1].Length + 1;
                }
            }
        }
    }
}
