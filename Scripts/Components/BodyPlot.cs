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
        public string plotString { get; set; }
        public EquipmentSlot[] bodyPlot { get; set; }
        public EquipmentSlot ReturnSlot(string slotName)
        {
            foreach (EquipmentSlot slot in bodyPlot) { if (slot != null && slot.name == slotName) { return slot; } }
            return null;
        }
        public BodyPlot(string _plotString) 
        { 
            plotString = _plotString;  
            bodyPlot = new EquipmentSlot[BodyPlots.bodyPlots[_plotString].Count()]; 
            for (int i = 0; i < BodyPlots.bodyPlots[_plotString].Count(); i++)
            {
                bodyPlot[i] = BodyPlots.bodyPlots[_plotString][i];
            }
        }
        public BodyPlot() { }
    }
}
