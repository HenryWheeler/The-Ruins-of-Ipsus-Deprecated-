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
        public void Throw(Entity user, Coordinate landingSite) { SpecialComponentManager.TriggerOnThrow(user, entity, landingSite); }
        public void ConsumeItem()
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            if (Map.map[coordinate.x, coordinate.y].item == entity) { Map.map[coordinate.x, coordinate.y].item = null; }
        }
        public Throwable() { }
    }
}
