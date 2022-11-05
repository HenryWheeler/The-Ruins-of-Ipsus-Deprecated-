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
        public int die1 { get; set; }
        public int die2 { get; set; }
        public int dmgModifier { get; set; }
        public int toHitModifier { get; set; }
        public string dmgType { get; set; }
        public string weaponType { get; set; }
        public AttackFunction(int _die1, int _die2, int _dmgModifier, int _toHitModifier, string _dmgType, string _weaponType)
        { die1 = _die1; die2 = _die2; dmgModifier = _dmgModifier; toHitModifier = _toHitModifier; dmgType = _dmgType; weaponType = _weaponType; }
        public AttackFunction() { }
    }
}
