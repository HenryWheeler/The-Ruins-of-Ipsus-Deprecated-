using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    [Serializable]
    public abstract class ActorBase : IDraw
    {
        public bool turnActive = false;
        public int x { get; set; }
        public int y { get; set; }
        public int hp { get; set; }
        public int hpCap { get; set; }
        public int sight { get; set; }
        public char character { get; set; }
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public string name { get; set; }
        public float speed { get; set; }
        public float speedCap { get; set; }
        public void Draw(RLConsole console) { console.Set(x, y, fColor, bColor, character); }
        public void CheckTurn() { speed--; if (speed <= 0) EndTurn(); }
        public abstract void StartTurn();
        public abstract void EndTurn();
    }
}
