using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnTurnProperty : Component
    {
        public bool start { get; set; }
        public abstract void OnTurn();
        public OnTurnProperty() { }
    }
}
