using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnTurn : Component
    {
        public bool start { get; set; }
        public abstract void Turn();
        public OnTurn ReturnBase() { return this; }
        public OnTurn() { }
    }
}
