using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Stats: Component
    {
        public int sight { get; set; }
        public int ac { get; set; }
        public float maxAction { get; set; }
        public int hp { get; set; }
        public int hpCap { get; set; }
        public int strength { get; set; }
        public int acuity { get; set; }
        public List<string> immunities = new List<string>();
        public void ModifyAllStats(Entity entityRef, bool add)
        {
            Stats stats = entityRef.GetComponent<Stats>();
            if (add)
            {
                stats.sight += sight;
                stats.ac += ac;
                stats.maxAction += maxAction;
                stats.hp += hp;
                stats.hp += hpCap;
                stats.strength += strength;
                stats.acuity += acuity;
            }
            else
            {
                stats.sight -= sight;
                stats.ac -= ac;
                stats.maxAction -= maxAction;
                stats.hp -= hp;
                stats.hp -= hpCap;
                stats.strength -= strength;
                stats.acuity -= acuity;
            }
            if (entity.display) { StatManager.UpdateStats(entity); }
        }
        public Stats(int _sight, int _ac, float _maxAction, int _hpCap, int _strength, int _acuity, List<string> _immunities = null) 
        { 
            sight = _sight; ac = _ac; maxAction = _maxAction; hp = _hpCap; hpCap = _hpCap;
            strength = _strength; acuity = _acuity; if (_immunities != null) { immunities = _immunities; }
        }
        public Stats() { }
    }
}
