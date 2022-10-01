using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    [Serializable]
    public class Monster : ActorBase
    {
        public AI ai;
        public ActorBase target;
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
            speedCap = data.speedCap;
            data.ai.Set(this);
        }
        public void Move(int _x, int _y)
        {
            if (Map.map[_x, _y].walkable && Map.map[_x, _y].actor == null)
            {
                Map.map[x, y].actor = null;
                x = _x; y = _y;
                Map.map[x, y].actor = this;
                CheckTurn();
            }
            else CheckTurn();
        }
        public override void StartTurn() { turnActive = true; ai.Action(this); }
        public override void EndTurn() { turnActive = false; TurnManager.ActorTurnEnd(); }
    }
    [Serializable]
    public class MonsterData
    {
        public bool turnActive = false;
        public int x { get; set; }
        public int y { get; set; }
        public int hpCap { get; set; }
        public int sight { get; set; }
        public char character { get; set; }
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public string name { get; set; }
        public float speedCap { get; set; }
        public AI ai { get; set; }
    }
    [Serializable]
    public abstract class AI
    {
        public int action;
        public abstract void Set(Monster monster);
        public abstract void Action(Monster monster);
        public Node Path(Monster monster, ActorBase target) { return null;}
    }
    public class ChaseAI : AI
    {
        public override void Set(Monster monster) { monster.ai = this; monster.target = Program.player; }
        public override void Action(Monster monster)
        {
            monster.CheckTurn();
        }
    }
}
