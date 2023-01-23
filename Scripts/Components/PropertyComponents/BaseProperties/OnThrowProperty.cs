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
        public string rangeModel { get; set; }
        public int strength { get; set; }
        public string itemType { get; set; }
        public abstract void OnThrow(Entity user, Coordinate landingSite);
        public OnThrowProperty() { }
    }
}
