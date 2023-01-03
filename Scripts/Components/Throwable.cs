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
            Vector2 vector3 = entity.GetComponent<Coordinate>().vector2;
            if (World.GetTraversable(vector3).itemLayer == entity) { World.GetTraversable(vector3).itemLayer = null; }
        }
        public Throwable() { }
    }
}
