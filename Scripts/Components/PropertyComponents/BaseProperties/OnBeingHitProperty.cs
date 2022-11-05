using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnBeingHitProperty : Component
    {
        public abstract void OnBeingHit(Entity attacker, int dmg, string type);
        public OnBeingHitProperty() { }
    }
}
