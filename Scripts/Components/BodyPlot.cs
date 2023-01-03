using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class BodyPlot: Component
    {
        public EquipmentSlot[] bodyPlot { get; set; }
        public EquipmentSlot ReturnSlot(string slotName)
        {
            foreach (EquipmentSlot slot in bodyPlot) 
            { 
                if (slot != null && slot.name == slotName)
                { 
                    return slot; 
                } 
            }
            return null;
        }
        public BodyPlot() 
        { 
            bodyPlot = new EquipmentSlot[4];
            bodyPlot[0] = new EquipmentSlot("Armor");
            bodyPlot[1] = new EquipmentSlot("Weapon");
            bodyPlot[2] = new EquipmentSlot("Off Hand");
            bodyPlot[3] = new EquipmentSlot("Magic Item");
        }
    }
    public class EquipmentSlot
    {
        public string name { get; set; }
        public Entity item { get; set; }
        public EquipmentSlot(string _name)
        {
            name = _name;
        }
        public EquipmentSlot() { }
    }
}
