using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class DijkstraProperty: OnTurnProperty
    {
        public int tick { get; set; }
        public int tickMax { get; set; }
        public override void OnTurn()
        {
            if (tick == 0) { DijkstraMaps.CreateMap(entity.GetComponent<Coordinate>(), entity.GetComponent<Description>().name + entity.tempID); tick = tickMax; }
            else { tick--; }
        }
        public DijkstraProperty(int _tickMax) { tickMax = _tickMax; start = false; special = true; }
    }
}
