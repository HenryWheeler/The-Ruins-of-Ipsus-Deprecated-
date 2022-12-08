using System;
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
            uID = 0;

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
                AddComponent(new Stats(7, 10, 1f, 500, 500, 99, 99));
                AddComponent(new TurnFunction());
                AddComponent(new Movement(true, true, false));
                AddComponent(new Inventory());
                AddComponent(new BodyPlot("Basic_Humanoid"));
                AddComponent(new OnHit());
                AddComponent(new Faction("Human"));
                AddComponent(new DijkstraProperty());
                AddComponent(new UpdateCameraOnMove());
                AddComponent(new PlayerComponent(Program.rootConsole));

                Entity startingWeapon = JsonDataManager.ReturnEntity(1001);
                InventoryManager.AddToInventory(this, startingWeapon);
                Action.PlayerAction(this);  
            }
        }
        public Player() { }
    }
}
