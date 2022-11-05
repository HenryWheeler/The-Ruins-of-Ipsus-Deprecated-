using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnTurnStartProperty: Component
    {
        public abstract void OnTurnStart();
        public OnTurnStartProperty() { }
    }
}
