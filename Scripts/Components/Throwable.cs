using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Throwable : Component
    {
        public bool consumable = true;
        public string throwMessage { get; set; }
        public void Throw(Entity user, Coordinate landingSite)
        {
            SpecialComponentManager.TriggerOnThrow(user, entity, landingSite);
        }
        public Throwable(string _throwMessage) 
        {
            throwMessage = _throwMessage;
        }
        public Throwable() { }
    }
}
