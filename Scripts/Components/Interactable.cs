using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    class Interactable: Component
    {
        public HashSet<string> interactions = new HashSet<string>();
        public Interactable(HashSet<string> _interactions) { interactions = _interactions; }
        public Interactable() { }
    }
}
