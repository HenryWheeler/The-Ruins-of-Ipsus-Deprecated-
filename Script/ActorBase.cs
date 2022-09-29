using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    public abstract class ActorBase : IDraw
    {
        public int x { get; set; }
        public int y { get; set; }
        public int sight { get; set; }
        public char character { get; set; }
        public ActorBase actor = null;
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public string name { get; set; }
        public bool turnActive = false;
        public List<int> speed = new List<int>();
        public void Draw(RLConsole console) { console.Set(x, y, fColor, bColor, character); }
        public abstract void StartTurn();
        public abstract void EndTurn();
    }
}
