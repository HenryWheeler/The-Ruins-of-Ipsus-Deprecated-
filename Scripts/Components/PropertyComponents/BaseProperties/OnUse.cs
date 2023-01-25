using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnUse: Component
    {
        public string rangeModel { get; set; }
        public int range { get; set; }
        public int strength { get; set; }
        public bool singleUse { get; set; }
        public string itemType { get; set; }
        public abstract void Use(Entity entity, Vector2 target = null);
        public OnUse ReturnBase() { return this; }
        public OnUse() { }
    }
}
