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
        public Monster(MonsterData data)
        {
            x = data.x;
            y = data.y;
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
        }
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
    public class MonsterData
    {
        public bool turnActive = false;
        public int x { get; set; }
        public int y { get; set; }
        public int hpCap { get; set; }
        public int ac { get; set; }
        public int sight { get; set; }
        public char character { get; set; }
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public string name { get; set; }
        public float actMax { get; set; }
        public AI ai { get; set; }
        public int maxMemory { get; set; }
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
                            AtkData atkData = new AtkData("1-4-0-0");
                            aiHolder.Attack(atkData, target, aiHolder);
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
