using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Traversable: Component
    {
        public int terrainType { get; set; }
        public Entity sfxLayer { get; set; }
        public Entity actorLayer { get; set; }
        public Entity itemLayer { get; set; }
        public Entity obstacleLayer { get; set; }
        public Traversable(int _terrainType) { terrainType = _terrainType; }
        public Traversable() { }
    }
}
