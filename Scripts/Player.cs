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

            id = 0;
            character = '@';
            fColor = "Green";
            bColor = "Black";
            name = "Player";
            sight = 5;
            actMax = 1;
            hpCap = 500;
            hp = hpCap;
            ac = 10;
            description = "It's you.";
            inventory = new List<Item>();
            bodyPlotName = "Basic_Humanoid";
            bodyPlot = BodyPlots.bodyPlots[bodyPlotName];
            attacks = new List<AtkData>();

            StartTurn();
            RLKey key = RLKey.Unknown;
            Action.PlayerAction(this, key);
        }
        public Player(Player player, RLRootConsole _rootConsole)
        {
            rootConsole = _rootConsole;
            rootConsole.Update += Update;

            x = player.x;
            y = player.x;
            id = 0;
            character = player.character;
            fColor = player.fColor;
            bColor = player.bColor;
            name = player.name;
            sight = player.sight;
            actMax = player.actMax;
            hpCap = player.hpCap;
            hp = player.hp;
            ac = player.ac;
            description = player.description;
            inventory = new List<Item>(); inventory = player.inventory;
            bodyPlot = new EquipmentSlot[player.bodyPlot.Length]; bodyPlot = bodyPlot = player.bodyPlot;
            attacks = new List<AtkData>(); attacks = player.attacks;
        }
        public Player() { rootConsole = Program.rootConsole; rootConsole.Update += Update; }
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
                    Attack(Map.map[x + _x, y + _y].actor, this, 0);
                }
            }
            else { Log.AddToStoredLog("You cannot move there."); Log.DisplayLog(); }
        }
        public void Clear() { ShadowcastFOV.ClearSight(); }
        public void FOV() { ShadowcastFOV.Compute(x, y, sight); }
    }
}
