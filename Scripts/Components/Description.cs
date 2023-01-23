using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Description: Component
    {
        public string name { get; set; }
        public string description { get; set; }
        public Description(string _name, string _description) 
        { 
            name = _name; 
            description = _description; 
        }
        public Description(Description _description) 
        { 
            name = _description.name; 
            description = _description.description; 
        }
        public Description() { }
    }
}
