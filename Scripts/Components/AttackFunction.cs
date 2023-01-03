using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class AttackFunction: Component
    {
        public string dmgType { get; set; }
        public string details { get; set; }
        public AttackFunction(string _details, string _dmgType)
        {
            details = _details;
            dmgType = _dmgType; 
        }
        public AttackFunction() { }
    }
}
