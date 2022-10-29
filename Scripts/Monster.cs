using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Monster : ActorBase
    {
        public string ai { get; set; }
        public int memory { get; set; }
        public int maxMemory { get; set; }
        public Monster(int _id, int _x, int _y, int _hpCap, int _ac, int _sight, char _character, string _fColor, string _bColor, bool _opaque, string _name, string _description, float _actMax, string _ai, int _maxMemory, string _bodyPlotName, List<int> _moveTypes, bool _canSwim)
        {
            id = _id;
            x = _x;
            y = _y;
            hpCap = _hpCap;
            hp = hpCap;
            sight = _sight;
            character = _character;
            fColor = _fColor;
            bColor = _bColor;
            opaque = _opaque;
            name = _name;
            actMax = _actMax;
            ai = _ai;
            maxMemory = _maxMemory;
            description = _description;
            inventory = new List<Item>();
            bodyPlotName = _bodyPlotName;
            bodyPlot = BodyPlots.bodyPlots[bodyPlotName];
            spacer = " + ";
            attacks = new List<AtkData>();
            moveTypes = _moveTypes;
            canSwim = _canSwim;
        }
        public Monster(Monster data, int _x, int _y, EquipmentSlot[] _bodyPlot, List<Item> _inventory, List<AtkData> _attacks)
        {
            x = _x;
            y = _y;
            id = data.id;
            hpCap = data.hpCap;
            hp = hpCap;
            sight = data.sight;
            character = data.character;
            fColor = data.fColor;
            bColor = data.bColor;
            opaque = data.opaque;
            name = data.name;
            actMax = data.actMax;
            ai = data.ai;
            maxMemory = data.maxMemory;
            description = data.description;
            spacer = " + ";
            bodyPlotName = data.bodyPlotName; bodyPlot = _bodyPlot;
            inventory = new List<Item>(); inventory = _inventory;
            attacks = new List<AtkData>(); attacks = _attacks;
            moveTypes = data.moveTypes;
            canSwim = data.canSwim;
        }
        public Monster() { }
        public override string Describe() { return name + ": " + spacer + description; }
        public void Move(int _x, int _y)
        {
            if (CMath.CheckBounds(_x, _y) && moveTypes.Contains(Map.map[_x, _y].moveType) && Map.map[_x, _y].actor == null)
            {
                Map.map[x, y].actor = null;
                x = _x; y = _y;
                Map.map[x, y].actor = this;
            }
            else if (CMath.CheckBounds(_x, _y) && Map.map[_x, _y].moveType == 2 && canSwim && Map.map[_x, _y].actor == null)
            {
                Map.map[x, y].actor = null;
                x = _x; y = _y;
                Map.map[x, y].actor = this;
            }
            EndTurn();
        }
        public override void OnHit(int dmg, ActorBase attacker) { hp -= dmg; AIManager.aiDictionary[ai].OnHit(this, attacker); if (hp <= 0) Death(); }
        public override void Death() { Map.map[x, y].actor = null; Log.AddToStoredLog(name + " has died."); TurnManager.RemoveActor(this); }
        public override void StartTurn() { turnActive = true; AIManager.aiDictionary[ai].Action(this); ; }
        public override void EndTurn() { turnActive = false; TurnManager.ProgressActorTurn(this); }
    }
}
