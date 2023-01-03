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
        public override void OnTurn()
        { 
            //EntityManager.UpdateAll(); 
        }
        public DijkstraProperty() { start = false; }
    }
}
