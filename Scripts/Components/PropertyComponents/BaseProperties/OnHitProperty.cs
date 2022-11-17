using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnHitProperty : Component
    {
        public bool attack { get; set; }
        public abstract void OnHit(Entity attacker, Entity target, int dmg, string type);
        public OnHitProperty() { }
    }
}
