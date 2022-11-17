using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class SpecialDefences: Component
    {
        public List<string> statusImmunities= new List<string>();
        public SpecialDefences(List<string> _statusImmunities) { statusImmunities = _statusImmunities; }
        public SpecialDefences() { }
    }
}
