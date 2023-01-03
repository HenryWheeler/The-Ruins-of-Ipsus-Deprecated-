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
        public List<string> interactions = new List<string>();
        public Interactable(List<string> _interactions) { interactions = _interactions; }
        public Interactable() { }
    }
}
