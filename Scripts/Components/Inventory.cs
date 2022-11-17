using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Inventory: Component
    {
        public bool display { get; set; }
        public List<Entity> inventory = new List<Entity>();
        public Inventory(bool _display = false) { display = _display; inventory = new List<Entity>(); }
        public Inventory() { }
    }
}
