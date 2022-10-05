using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    [Serializable]
    public class Monster : ActorBase
    {
        public AI ai;
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
        }
        public void Move(int _x, int _y)
        {
            if (Map.map[_x, _y].walkable && Map.map[_x, _y].actor == null)
            {
                Map.map[x, y].actor = null;
                x = _x; y = _y;
                Map.map[x, y].actor = this;
                Log.AddToStoredLog("Monster moved to tile (" + x.ToString() + ", " + y.ToString() + ").");
            }
            EndTurn();
        }
        public override void Death() { Map.map[x, y].actor = null; TurnManager.RemoveActor(this); }
        public override void StartTurn() { turnActive = true; ai.Action(this); }
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
    }
    [Serializable]
    public abstract class AI
    {
        public int action;
        public abstract void Set(Monster monster);
        public abstract void Action(Monster monster);
        public Node Path(Monster monster, string map) { return DijkstraMaps.PathFromMap(monster.x, monster.y, map);}
    }
    public class ChaseAI : AI
    {
        public ActorBase target;
        public override void Set(Monster monster) { monster.ai = this; target = Program.player; }
        public override void Action(Monster monster)
        {
            Node targetNode = Path(monster, target.name);
            if (targetNode != null)
            {
                if (CMath.Distance(target.x, target.y, targetNode.x, targetNode.y) == 0)
                {
                    AtkData atkData = new AtkData("1-4-0-0");
                    monster.Attack(atkData, target);
                }
                else monster.Move(targetNode.x, targetNode.y);
            }
            else monster.EndTurn();
        }
    }
}
