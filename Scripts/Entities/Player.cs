﻿using System;
using System.Collections.Generic;
using RLNET;
using System.Threading.Tasks;
using System.Threading;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Player : Entity
    {
        public Player(List<Component> components = null)
        {
            Program.rootConsole.Update += Update;
            uID = 0;
            tempID = 0;

            if (components != null)
            {
                foreach (Component component in components)
                {
                    if (component != null) { AddComponent(component); }
                }
            }
            else
            {
                AddComponent(new Coordinate());
                AddComponent(new Draw("White", "Black", '@'));
                AddComponent(new Description("You", "It's you."));
                AddComponent(PronounReferences.pronounSets["Player"]);
                AddComponent(new Stats(7, 10, 1f, 500, 99, 99, true));
                AddComponent(new TurnFunction(GetComponent<Stats>().maxAction, true));
                AddComponent(new Movement(true, true, false, true));
                AddComponent(new Inventory(true));
                AddComponent(new BodyPlot("Basic_Humanoid"));
                AddComponent(new Visibility(false, false, false));
                AddComponent(new OnHit());
                AddComponent(new Faction("Human"));
                AddComponent(new DijkstraProperty());
                AddComponent(new UpdateCameraOnMove());

                Entity startingWeapon = JsonDataManager.ReturnEntity(2, 1);
                InventoryManager.AddToInventory(this, startingWeapon);
                Action.PlayerAction(this);  
            }
        }
        public Player() { }
        public void Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = Program.rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (GetComponent<TurnFunction>().turnActive) { Action.PlayerAction(this, keyPress.Key); }
                else if (Look.looking) { Action.LookAction(keyPress.Key); }
                else if (InventoryManager.inventoryOpen) { Action.InventoryAction(this, keyPress.Key); }
                else if (TargetReticle.targeting) { Action.TargetAction(this, keyPress.Key); }
            }
        }
    }
}
