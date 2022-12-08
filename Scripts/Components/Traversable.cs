using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    class Traversable: Component
    {
        public int terrainType { get; set; }
        public Traversable(int _terrainType) { terrainType = _terrainType; }
        public Traversable() { }
    }
}
