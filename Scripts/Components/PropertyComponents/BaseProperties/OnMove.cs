using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnMove: Component
    {
        public abstract void Move(Vector2 initialPosition, Vector2 finalPosition);
        public OnMove ReturnBase() { return this; }
        public OnMove() { }
    }
}
