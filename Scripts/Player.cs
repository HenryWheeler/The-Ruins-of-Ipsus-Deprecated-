using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Player : ActorBase
    {
        public RLRootConsole rootConsole;
        public Player(RLRootConsole _rootConsole)
        {
            rootConsole = _rootConsole;
            rootConsole.Update += Update;

            character = '@';
            fColor = RLColor.Green;
            bColor = RLColor.Black;
            name = "Player";
            sight = 5;
            actMax = 1;
            hpCap = 500;
            hp = hpCap;
            ac = 10;
            description = "It's you.";
            inventory = new List<ItemBase>();

            StartTurn();
            RLKey key = RLKey.Unknown;
            Action.PlayerAction(this, key);
        }
        public override string Describe() { return description; }
        public override void OnHit(int dmg, ActorBase attacker) { hp -= dmg; Stats.UpdateStats(); if (hp <= 0) Death(); }
        public override void Death() { }
        public override void StartTurn() { turnActive = true; Log.DisplayLog(); }
        public override void EndTurn() { MakeMap(); turnActive = false; TurnManager.ProgressActorTurn(this); }
        public void Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (turnActive) { Action.PlayerAction(this, keyPress.Key); }
                else if (Look.looking) { Action.LookAction(keyPress.Key); }
                else if (Inventory.inventoryOpen) { Action.InventoryAction(this, keyPress.Key); }
            }
        }
        public void MakeMap()
        {
            List<Node> nodes = new List<Node>
            {
                new Node(x, y, 0)
            };
            DijkstraMaps.CreateMap(new Node(x, y, 0), "Player");
        }
        public void Move(int _x, int _y)
        {
            if (Map.map[x + _x, y + _y].walkable)
            {
                if (Map.map[x + _x, y + _y].actor == null)
                {
                    Clear();
                    Map.map[x, y].actor = null;
                    x += _x; y += _y;
                    Map.map[x, y].actor = this;
                    FOV();
                    EndTurn();
                }
                else
                {
                    AtkData atkData = new AtkData("1-4-0-0");
                    Attack(atkData, Map.map[x + _x, y + _y].actor, this);
                }
            }
            else { Log.AddToStoredLog("You cannot move there."); Log.DisplayLog(); }
        }
        public void Clear() { ShadowcastFOV.ClearSight(); }
        public void FOV() { ShadowcastFOV.Compute(x, y, sight); }
    }
}
