using System;
using System.Collections.Generic;
using RLNET;

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
                AddComponent(new Stats(7, 10, 1f, 500, 10, 10, true));
                AddComponent(new TurnFunction(GetComponent<Stats>().maxAction, true));
                AddComponent(new Movement(true));
                AddComponent(new Inventory(true));
                AddComponent(new BodyPlot("Basic_Humanoid"));
                AddComponent(new Visibility(false, false, false));
                AddComponent(new OnHit());
                AddComponent(new DijkstraProperty(1));

                Entity startingWeapon = JsonDataManager.ReturnEntity(2, 1);
                InventoryManager.AddToInventory(this, startingWeapon);

                GetComponent<TurnFunction>().StartTurn();
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
                else if (TargetReticle.targeting) { Action.TargetAction(keyPress.Key); }
            }
        }
    }
}
