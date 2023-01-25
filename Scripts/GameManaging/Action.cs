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
        private static bool actorPresent = false;
        private static bool itemPresent = false;
        private static bool obstaclePresent = false;
        private static bool terrainPresent = false;
        private static string interactionText = "";
        private static string interactionKeyText = "";
        public static bool choosingDirection = false;
        public static bool choosingTarget = false;
        public static bool interacting = false;
        public static bool throwing = false;
        private static Entity chosenEntity { get; set; }
        private static Vector2 chosenLocation { get; set; }
        private static RLConsole console { get; set; }
        public Action(RLConsole _console) { console = _console; }
        public static Entity targetWeapon { get; set; }
        public static void MenuAction(RLKey key, RLRootConsole console)
        {
            switch (key)
            {
                case RLKey.N: Menu.MakeSelection(0); break;
                case RLKey.L: Menu.MakeSelection(1); break;
                case RLKey.Q: Menu.MakeSelection(2); break;
            }
        }
        public static void PlayerAction(Entity player, RLKey key = RLKey.Unknown)
        {
            if (key != RLKey.Unknown)
            {
                switch (key)
                {
                    case RLKey.Up: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(0, -1))); break;
                    case RLKey.Down: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(0, 1))); break;
                    case RLKey.Left: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(-1, 0))); break;
                    case RLKey.Right:  player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(1, 0))); break;
                    case RLKey.Keypad8: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(0, -1))); break;
                    case RLKey.Keypad9: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(1, -1))); break;
                    case RLKey.Keypad6: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(1, 0))); break;
                    case RLKey.Keypad3: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(1, 1))); break;
                    case RLKey.Keypad2: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(0, 1))); break;
                    case RLKey.Keypad1: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(-1, 1))); break;
                    case RLKey.Keypad4: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(-1, 0))); break;
                    case RLKey.Keypad7: player.GetComponent<Movement>().Move(new Vector2(player.GetComponent<Vector2>(), new Vector2(-1, -1))); break;
                    case RLKey.KeypadMinus:
                        {
                            Vector2 vector2 = player.GetComponent<Vector2>();
                            if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '<') { World.GenerateNewFloor(true); }
                            break;
                        }
                    case RLKey.KeypadPlus:
                        {
                            Vector2 vector2 = player.GetComponent<Vector2>();
                            if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '>') { World.GenerateNewFloor(false); }
                            break;
                        }
                    case RLKey.Plus:
                        {
                            Vector2 vector2 = player.GetComponent<Vector2>();
                            if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '>') { World.GenerateNewFloor(true); }
                            break;
                        }
                    case RLKey.Minus:
                        {
                            Vector2 vector2 = player.GetComponent<Vector2>();
                            if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '<') { World.GenerateNewFloor(false); }
                            break;
                        }
                    case RLKey.Period: player.GetComponent<TurnFunction>().EndTurn(); break;
                    case RLKey.L: Look.StartLooking(player.GetComponent<Vector2>()); break;
                    case RLKey.I: InventoryManager.OpenInventory(); break;
                    case RLKey.G: InventoryManager.GetItem(player); Log.DisplayLog(); break;
                    case RLKey.J: SaveDataManager.CreateSave(); Program.gameActive = false; Renderer.running = false; Program.rootConsole.Close(); break;
                    case RLKey.V:
                        foreach (Traversable tile in World.tiles)
                        {
                            if (tile != null)
                            {
                                Vector2 vector2 = tile.entity.GetComponent<Vector2>();
                                ShadowcastFOV.SetVisible(vector2, true, 1000, vector2.x, vector2.y, true);
                            }
                        }
                        break;
                    case RLKey.Space:
                        {
                            player.GetComponent<TurnFunction>().turnActive = false;
                            InventoryManager.inventoryOpen = false;
                            TargetReticle.targeting = false;
                            interacting = true;
                            choosingDirection = true;
                            Interaction(player);
                            break;
                        }
                }
            }
        }
        public static void Interaction(Entity player, RLKey key = RLKey.Unknown)
        {
            if (choosingDirection) { CMath.DisplayToConsole(Log.console, "Choose a direction.", 1, 1); DisplayActions("Directions:[NumPad]" + " End Interaction:[Escape]"); }
            else
            {
                CMath.DisplayToConsole(Log.console, interactionText, 1, 1);
                DisplayActions(interactionKeyText);
            }
            if (key != RLKey.Unknown)
            {
                if (choosingDirection)
                {
                    switch (key)
                    {
                        case RLKey.Up: ChooseDirection(player, 0, -1); break;
                        case RLKey.Down: ChooseDirection(player, 0, 1); break;
                        case RLKey.Left: ChooseDirection(player, -1, 0); break;
                        case RLKey.Right: ChooseDirection(player, 1, 0); break;
                        case RLKey.Keypad8: ChooseDirection(player, 0, -1); break;
                        case RLKey.Keypad9: ChooseDirection(player, 1, -1); break;
                        case RLKey.Keypad6: ChooseDirection(player, 1, 0); break;
                        case RLKey.Keypad3: ChooseDirection(player, 1, 1); break;
                        case RLKey.Keypad2: ChooseDirection(player, 0, 1); break;
                        case RLKey.Keypad1: ChooseDirection(player, -1, 1); break;
                        case RLKey.Keypad4: ChooseDirection(player, -1, 0); break;
                        case RLKey.Keypad7: ChooseDirection(player, -1, -1); break;
                        case RLKey.Keypad5: ChooseDirection(player, 0, 0); break;
                        case RLKey.Escape: EscapeInteraction(player); break;
                    }
                }
                else if (choosingTarget)
                {
                    switch (key)
                    {
                        case RLKey.E: if (actorPresent) { ChooseEntity(player, 0); } break;
                        case RLKey.I: if (itemPresent) { ChooseEntity(player, 1 ); } break;
                        case RLKey.O: if (obstaclePresent) { ChooseEntity(player, 2); } break;
                        case RLKey.T: if (terrainPresent) { ChooseEntity(player, 3); } break;
                        case RLKey.Escape: EscapeInteraction(player); break;
                    }
                }
                else
                {
                    switch (key)
                    {
                        case RLKey.A:
                            {
                                if (chosenEntity.GetComponent<Interactable>().interactions.Contains("Attack"))
                                {
                                    EscapeInteraction(player);
                                    AttackManager.MeleeAllStrike(player, chosenEntity); 
                                }
                                else
                                {
                                    ReturnFalseMethodMessage();
                                }
                                break;
                            }
                        case RLKey.O:
                            {
                                if (chosenEntity.GetComponent<Interactable>().interactions.Contains("Openable"))
                                {
                                    DoorFunction doorFunction = chosenEntity.GetComponent<DoorFunction>();
                                    if (doorFunction != null)
                                    {
                                        if (doorFunction.open)
                                        {
                                            doorFunction.Close();
                                            Log.Add("You close the door");
                                        }
                                        else
                                        {
                                            doorFunction.Open();
                                            Log.Add("You open the door.");
                                        }
                                        EscapeInteraction(player);
                                    }
                                    else
                                    {
                                        ReturnFalseMethodMessage();
                                    }
                                }
                                else
                                {
                                    ReturnFalseMethodMessage();
                                }
                                break;
                            }
                        case RLKey.Escape: EscapeInteraction(player); break;
                    }
                }
            }
        }
        public static void ReturnFalseMethodMessage()
        {
            DisplayActions("You cannot interact via that method. Choose another method of interaction");
        }
        public static void EscapeInteraction(Entity player)
        { 
            player.GetComponent<TurnFunction>().turnActive = true; 
            interacting = false; 
            choosingDirection = false; 
            choosingTarget = false; 
            PlayerAction(player);
            StatManager.UpdateStats(player);
            Log.DisplayLog(); 
        }
        public static void InventoryAction(Entity player, RLKey key = RLKey.Unknown)
        {
            if (key != RLKey.Unknown)
            {
                CMath.DisplayToConsole(Log.console, "", 0, 0);
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
                    case RLKey.D: if (player.GetComponent<Inventory>().inventory.Count != 0) { InventoryManager.DropItem(player, InventoryManager.inventoryDisplay[InventoryManager.currentPage][InventoryManager.selection]); } break;
                    case RLKey.E:
                        {
                            if (player.GetComponent<Inventory>().inventory.Count != 0)
                            {
                                int first = InventoryManager.currentPage;
                                int second = InventoryManager.selection;
                                if (InventoryManager.inventoryDisplay[first][second].GetComponent<Equippable>() != null)
                                {
                                    if (InventoryManager.inventoryDisplay[first][second].GetComponent<Equippable>().equipped) { InventoryManager.UnequipItem(player, InventoryManager.inventoryDisplay[first][second], true); }
                                    else { InventoryManager.EquipItem(player, InventoryManager.inventoryDisplay[first][second]); }
                                } else { CMath.DisplayToConsole(Log.console, "You cannot equip the " + InventoryManager.inventoryDisplay[first][second].GetComponent<Description>().name + ".", 1, 1); }
                            }
                            break;
                        }
                    case RLKey.U:
                        {
                            if (player.GetComponent<Inventory>().inventory.Count != 0)
                            {
                                int first = InventoryManager.currentPage;
                                int second = InventoryManager.selection;
                                if (InventoryManager.inventoryDisplay[first][second].GetComponent<Usable>() != null) 
                                {
                                    targetWeapon = InventoryManager.inventoryDisplay[first][second];
                                    InventoryManager.UseItem(player, InventoryManager.inventoryDisplay[first][second]);
                                }
                                else { CMath.DisplayToConsole(Log.console, "You cannot use the " + InventoryManager.inventoryDisplay[first][second].GetComponent<Description>().name + ".", 1, 1); }
                            }
                            break;
                        }
                    case RLKey.T:
                        {
                            if (player.GetComponent<Inventory>().inventory.Count != 0)
                            {
                                int first = InventoryManager.currentPage;
                                int second = InventoryManager.selection;
                                targetWeapon = InventoryManager.inventoryDisplay[first][second];
                                TargetReticle.StartTargeting(true, true);
                            }
                            break;
                        }
                    default:
                        {
                            InventoryManager.DisplayItem();
                            InventoryManager.DisplayInventory();
                            break;
                        }
                }
            }
        }
        public static void TargetAction(Entity player, RLKey key = RLKey.Unknown)
        {
            if (key != RLKey.Unknown)
            {
                switch (key)
                {
                    case RLKey.Up: TargetReticle.Move(0, -1); break;
                    case RLKey.Down: TargetReticle.Move(0, 1); break;
                    case RLKey.Left: TargetReticle.Move(-1, 0); break;
                    case RLKey.Right: TargetReticle.Move(1, 0); break;
                    case RLKey.Keypad8: TargetReticle.Move(0, -1); break;
                    case RLKey.Keypad9: TargetReticle.Move(1, -1); break;
                    case RLKey.Keypad6: TargetReticle.Move(1, 0); break;
                    case RLKey.Keypad3: TargetReticle.Move(1, 1); break;
                    case RLKey.Keypad2: TargetReticle.Move(0, 1); break;
                    case RLKey.Keypad1: TargetReticle.Move(-1, 1); break;
                    case RLKey.Keypad4: TargetReticle.Move(-1, 0); break;
                    case RLKey.Keypad7: TargetReticle.Move(-1, -1); break;
                    case RLKey.Escape: TargetReticle.StopTargeting(); break;
                    case RLKey.S: TargetReticle.StopTargeting(); break;
                    case RLKey.U: Throw(player); break;
                    case RLKey.T: Throw(player); break;
                    case RLKey.Enter: Throw(player); break;
                }
            }
        }
        public static void Throw(Entity player)
        {
            if (throwing)
            {
                TargetReticle.ThrowWeapon(targetWeapon);
            }
            else
            {
                Vector2 vector2 = TargetReticle.ReturnCoords(true);
                if (vector2 != null)
                {
                    foreach (OnUse component in targetWeapon.GetComponent<Usable>().onUseComponents)
                    {
                        if (component != null)
                        {
                            component.Use(player, vector2);
                        }
                    }
                }
            }
        }
        public static void LookAction(RLKey key = RLKey.Unknown)
        {
            if (key != RLKey.Unknown)
            {
                switch (key)
                {
                    case RLKey.Up: Look.Move(0, -1); break;
                    case RLKey.Down: Look.Move(0, 1); break;
                    case RLKey.Left: Look.Move(-1, 0); break;
                    case RLKey.Right: Look.Move(1, 0); break;
                    case RLKey.Keypad8: Look.Move(0, -1); break;
                    case RLKey.Keypad9: Look.Move(1, -1); break;
                    case RLKey.Keypad6: Look.Move(1, 0); break;
                    case RLKey.Keypad3: Look.Move(1, 1); break;
                    case RLKey.Keypad2: Look.Move(0, 1); break;
                    case RLKey.Keypad1: Look.Move(-1, 1); break;
                    case RLKey.Keypad4: Look.Move(-1, 0); break;
                    case RLKey.Keypad7: Look.Move(-1, -1); break;
                    case RLKey.Escape: Look.StopLooking(); break;
                    case RLKey.L: Look.StopLooking(); break;
                }
            }
        }
        public static void ChooseEntity(Entity player, int type)
        {
            interactionText = "";
            interactionKeyText = "";
            Traversable traversable = World.tiles[chosenLocation.x, chosenLocation.y];
            if (type == 0) 
            { chosenEntity = traversable.actorLayer; }
            else if (type == 1) 
            { chosenEntity = traversable.itemLayer; }
            else if (type == 2) 
            { chosenEntity = traversable.obstacleLayer; }
            else if (type == 3) 
            { chosenEntity = traversable.entity; }
            choosingTarget = false;
            if (chosenEntity.GetComponent<Interactable>().interactions.Count == 0)
            {
                interactionText = "There is no way to interact with the " + chosenEntity.GetComponent<Description>().name;
                interactionKeyText = "End Interaction:[Escape] + ";
                Interaction(player);
            }
            else
            {
                interactionText = "How do you interact?";
                foreach (string interaction in chosenEntity.GetComponent<Interactable>().interactions)
                {
                    if (interaction == "Openable")
                    {
                        interactionKeyText += $"Open/Close:[{interaction.ToArray()[0]}] +";
                    }
                    else
                    {
                        interactionKeyText += $"{interaction}:[{interaction.ToArray()[0]}] +";
                    }
                }
                interactionKeyText += "End Interaction:[Escape]";
                Interaction(player);
            }
        }
        public static void ChooseDirection(Entity player, int x, int y)
        {
            Vector2 vector2 = player.GetComponent<Vector2>();
            chosenLocation = new Vector2(vector2.x + x, vector2.y + y);
            Traversable traversable = World.tiles[chosenLocation.x, chosenLocation.y];
            interactionKeyText = "";
            int interactions = 0;
            if (x != 0 || y != 0)
            {
                if (traversable.actorLayer != null) 
                { 
                    actorPresent = true;
                    interactionKeyText += traversable.actorLayer.GetComponent<Description>().name + ":[E] + ";
                    interactions++;
                }
                else { actorPresent = false; }
            } else { actorPresent = false; }
            if (traversable.itemLayer != null) 
            { 
                itemPresent = true;
                interactionKeyText += traversable.itemLayer.GetComponent<Description>().name + ":[I] + ";
                interactions++;
            }
            else { itemPresent = false; }
            if (traversable.obstacleLayer != null) 
            { 
                obstaclePresent = true;
                interactionKeyText += traversable.obstacleLayer.GetComponent<Description>().name + ":[O] + ";
                interactions++;
            }
            else { obstaclePresent = false; }
            if (traversable.entity.GetComponent<Interactable>() != null) 
            {
                terrainPresent = true;
                interactionKeyText += traversable.entity.GetComponent<Description>().name + ":[T] + ";
                interactions++;
            }
            else { terrainPresent = false; }
            if (!actorPresent && !itemPresent && !obstaclePresent && !terrainPresent)
            { interactionText = "There is nothing there."; CMath.DisplayToConsole(Log.console, interactionText, 1, 1); }
            else if (interactions == 1)
            {
                choosingDirection = false;
                if (actorPresent) { ChooseEntity(player, 0); }
                else if (itemPresent) { ChooseEntity(player, 1); }
                else if (obstaclePresent) { ChooseEntity(player, 2); }
                else if (terrainPresent) { ChooseEntity(player, 3); }
            }
            else
            {
                interactionText = "What do you interact with?";
                interactionKeyText += "End Interaction:[Escape]";
                choosingDirection = false;
                choosingTarget = true;
                Interaction(player);
            }
        }
        public static void DisplayActions(string actions) 
        { CMath.ClearConsole(console); CMath.DisplayToConsole(console, actions, 0, 2); console.Print(8, 0, " Actions ", RLColor.White); }
    }
}
