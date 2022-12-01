using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnUseProperty: Component
    {
        public abstract void OnUse(Entity entity);
        public OnUseProperty() { }
    }
}
