using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnThrowProperty : Component
    {
        public abstract void OnThrow(Entity user, Coordinate landingSite);
        public OnThrowProperty() { }
    }
}
