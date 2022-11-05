using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class AI : Component
    {
        public abstract void Action();
        public abstract void OnHit(Entity attacker);
        public AI() { }
    }
}
