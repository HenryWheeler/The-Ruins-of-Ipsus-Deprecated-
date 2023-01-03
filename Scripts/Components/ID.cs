using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    class ID: Component
    {
        public int id { get; set; }
        public ID(int _id) { id = _id; }
        public ID() { }
    }
}
