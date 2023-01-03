using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Commerce: Component
    {
        public int value { get; set; }
        public Commerce(int _value)
        { 
            value = _value;
        }
        public Commerce() { }
    }
}
