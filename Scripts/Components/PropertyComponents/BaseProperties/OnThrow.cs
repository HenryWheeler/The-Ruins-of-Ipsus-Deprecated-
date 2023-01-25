using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnThrow : Component
    {
        public string rangeModel { get; set; }
        public int strength { get; set; }
        public string itemType { get; set; }
        public abstract void Throw(Entity user, Vector2 landingSite);
        public OnThrow ReturnBase() { return this; }
        public OnThrow() { }
    }
}
