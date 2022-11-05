using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Coordinate: Component
    {
        public int x { get; set; }
        public int y { get; set; }
        public Coordinate(int _x, int _y) { x = _x; y = _y; }
        public Coordinate() { }
    }
}
