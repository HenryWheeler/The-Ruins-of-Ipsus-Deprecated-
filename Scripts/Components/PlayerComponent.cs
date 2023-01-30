using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SadConsole.Components;
using SadConsole;
using SadConsole.Input;
using Microsoft.Xna.Framework;

namespace TheRuinsOfIpsus
{
    class PlayerComponent: Component
    {
        public PlayerComponent() { }
    }
    class KeyboardComponent : KeyboardConsoleComponent
    {
        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            if (Program.gameActive)
            {
                if (Program.player.GetComponent<TurnFunction>().turnActive)
                {
                    if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Up)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(0, -1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Down)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(0, 1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Left)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(-1, 0))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Right)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(1, 0))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad8)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(0, -1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad9)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(1, -1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(1, 0))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(1, 1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(0, 1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(-1, 1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(-1, 0))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad7)) { Program.player.GetComponent<Movement>().Move(new Vector2(Program.player.GetComponent<Vector2>(), new Vector2(-1, -1))); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPlus))
                    {
                        Vector2 vector2 = Program.player.GetComponent<Vector2>();
                        if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '>') { World.GenerateNewFloor(true); }
                    }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                    {
                        Vector2 vector2 = Program.player.GetComponent<Vector2>();
                        if (World.tiles[vector2.x, vector2.y].entity.GetComponent<Draw>().character == '<') { World.GenerateNewFloor(false); }
                    }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.OemPeriod)) { Program.player.GetComponent<TurnFunction>().EndTurn(); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.L)) { Look.StartLooking(Program.player.GetComponent<Vector2>()); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.I)) { InventoryManager.OpenInventory(); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.E)) { InventoryManager.OpenEquipment(); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G)) { InventoryManager.GetItem(Program.player); Log.DisplayLog(); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.J)) { SaveDataManager.CreateSave(); Program.gameActive = false; Renderer.running = false; SadConsole.Game.Instance.Exit(); }
                    else if (info.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.V))
                    {
                        foreach (Traversable tile in World.tiles)
                        {
                            if (tile != null)
                            {
                                Vector2 vector2 = tile.entity.GetComponent<Vector2>();
                                ShadowcastFOV.SetVisible(vector2, true, 1000, vector2.x, vector2.y, true);
                            }
                        }
                    }
                }
                else if (InventoryManager.inventoryOpen)
                {

                }
                else if (InventoryManager.equipmentOpen)
                {

                }
            }
            else
            {
                Action.MenuAction(info);
            }
            handled = true;
            Global.CurrentScreen.IsDirty = true;
        }
    }
}
