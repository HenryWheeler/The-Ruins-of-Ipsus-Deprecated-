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
        public string rangeModel { get; set; }
        public int range { get; set; }
        public int strength { get; set; }
        public bool singleUse { get; set; }
        public string itemType { get; set; }
        public abstract void OnUse(Entity entity, Vector2 target = null);
        public OnUseProperty() { }
    }
}
