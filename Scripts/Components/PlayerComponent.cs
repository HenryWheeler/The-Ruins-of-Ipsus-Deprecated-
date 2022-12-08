using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    class PlayerComponent: Component
    {
        public void Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = Program.rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (entity.GetComponent<TurnFunction>().turnActive) { Action.PlayerAction((Player)entity, keyPress.Key); }
                else if (Look.looking) { Action.LookAction(keyPress.Key); }
                else if (InventoryManager.inventoryOpen) { Action.InventoryAction((Player)entity, keyPress.Key); }
                else if (TargetReticle.targeting) { Action.TargetAction((Player)entity, keyPress.Key); }
            }
        }
        public PlayerComponent(RLRootConsole console) { console.Update += Update; }
        public PlayerComponent() { }
    }
}
