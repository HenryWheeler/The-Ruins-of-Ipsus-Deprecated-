using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Monster : ActorBase
    {
        public AI ai { get; set; }
        public int memory { get; set; }
        public int maxMemory { get; set; }
        public Monster(int _x, int _y, int _hpCap, int _ac, int _sight, char _character, RLColor _fColor, RLColor _bColor, bool _opaque, string _name, string _description, float _actMax, AI _ai, int _maxMemory, string _bodyPlotName)
        {
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
            _ai.Set(this);
            maxMemory = _maxMemory;
            description = _description;
            inventory = new List<Item>();
            bodyPlotName = _bodyPlotName;
            bodyPlot = BodyPlots.bodyPlots[bodyPlotName];
            spacer = " + ";
            attacks = new List<AtkData>();
        }
        public Monster(Monster data, int _x, int _y)
        {
            x = _x;
            y = _y;
            hpCap = data.hpCap;
            hp = hpCap;
            sight = data.sight;
            character = data.character;
            fColor = data.fColor;
            bColor = data.bColor;
            opaque = data.opaque;
            name = data.name;
            actMax = data.actMax;
            data.ai.Set(this);
            maxMemory = data.maxMemory;
            description = data.description;
            inventory = new List<Item>();
            bodyPlotName = data.bodyPlotName;
            bodyPlot = BodyPlots.bodyPlots[bodyPlotName];
            spacer = " + ";
            attacks = new List<AtkData>();
        }
        public override string Describe() { return name + ": " + spacer + description; }
        public void Move(int _x, int _y)
        {
            if (Map.map[_x, _y].walkable && Map.map[_x, _y].actor == null)
            {
                Map.map[x, y].actor = null;
                x = _x; y = _y;
                Map.map[x, y].actor = this;
            }
            EndTurn();
        }
        public override void OnHit(int dmg, ActorBase attacker) { hp -= dmg; ai.OnHit(attacker); if (hp <= 0) Death(); }
        public override void Death() { Map.map[x, y].actor = null; Log.AddToStoredLog(name + " has died."); TurnManager.RemoveActor(this); }
        public override void StartTurn() { turnActive = true; ai.Action(); }
        public override void EndTurn() { turnActive = false; TurnManager.ProgressActorTurn(this); }
    }
    [Serializable]
    public abstract class AI
    {
        public Monster aiHolder;
        public int action;
        public abstract void Set(Monster monster);
        public abstract void Action();
        public abstract void OnHit(ActorBase attacker);
        public Node Path(string map) { return DijkstraMaps.PathFromMap(aiHolder.x, aiHolder.y, map);}
    }
    public class ChaseAI : AI
    {
        public ActorBase target;
        public override void Set(Monster monster) { monster.ai = this; aiHolder = monster; target = Program.player; }
        public override void Action()
        {
            target = Program.player;
            if (target != null)
            {
                if (CMath.Sight(aiHolder.x, aiHolder.y, target.x, target.y, aiHolder.sight)) { aiHolder.memory = aiHolder.maxMemory; }
                else { if (aiHolder.memory > 0) { aiHolder.memory--; } }
                if (aiHolder.memory > 0)
                {
                    Node targetNode = Path(target.name);
                    if (targetNode != null)
                    {
                        if (CMath.Distance(target.x, target.y, targetNode.x, targetNode.y) == 0)
                        {
                            aiHolder.Attack(target, aiHolder, 0);
                        }
                        else aiHolder.Move(targetNode.x, targetNode.y);
                    }
                    else aiHolder.EndTurn();
                }
                else aiHolder.EndTurn();
            }
            else aiHolder.EndTurn();
        }
        public override void OnHit(ActorBase attacker) { target = attacker; }
    }
}
