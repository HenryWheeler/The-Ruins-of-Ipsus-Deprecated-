using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class DijkstraProperty: OnTurnEndProperty
    {
        public override void OnTurnEnd()
        {
            DijkstraMaps.CreateMap(entity.GetComponent<Coordinate>(), entity.GetComponent<Description>().name + entity.tempID);
        }
        public DijkstraProperty() { property = true; }
    }
}
