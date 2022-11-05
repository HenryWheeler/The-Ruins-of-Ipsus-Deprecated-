using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnHittingProperty : Component
    {
        public abstract void OnHitting(Entity target, int dmg, string type);
        public OnHittingProperty() { }
    }
}
