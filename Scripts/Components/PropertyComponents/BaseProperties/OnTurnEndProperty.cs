using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnTurnEndProperty: Component
    {
        public abstract void OnTurnEnd();
        public OnTurnEndProperty() { }
    }
}
