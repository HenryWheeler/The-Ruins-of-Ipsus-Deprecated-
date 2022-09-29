using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeTest
{
    public abstract class ActorBase
    {
        public int x;
        public int y;
        public char character;
        public RLColor color;
        public string name;
        public bool turnActive = false;
        public abstract void StartTurn();
        public abstract void EndTurn();
    }
}
