using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class BodyPlots
    {
        public static Dictionary<string, EquipmentSlot> usableSlots = new Dictionary<string, EquipmentSlot>();
        public static Dictionary<string, EquipmentSlot[]> bodyPlots = new Dictionary<string, EquipmentSlot[]>();
        public BodyPlots()
        {
            usableSlots.Add("Head", new EquipmentSlot("Head"));
            usableSlots.Add("Face", new EquipmentSlot("Face"));
            usableSlots.Add("Torso", new EquipmentSlot("Torso"));
            usableSlots.Add("Legs", new EquipmentSlot("Legs"));
            usableSlots.Add("Feet", new EquipmentSlot("Feet"));
            usableSlots.Add("Off_Hand", new EquipmentSlot("Off_Hand"));
            usableSlots.Add("Main_Hand", new EquipmentSlot("Main_Hand"));

            InitializeBodyPlots();
        }
        private void InitializeBodyPlots()
        {
            EquipmentSlot[] basicHumanoidPlot = new EquipmentSlot[7];
            basicHumanoidPlot[0] = usableSlots["Head"];
            basicHumanoidPlot[1] = usableSlots["Face"];
            basicHumanoidPlot[2] = usableSlots["Torso"];
            basicHumanoidPlot[3] = usableSlots["Legs"];
            basicHumanoidPlot[4] = usableSlots["Feet"];
            basicHumanoidPlot[5] = usableSlots["Off_Hand"];
            basicHumanoidPlot[6] = usableSlots["Main_Hand"];
            bodyPlots.Add("Basic_Humanoid", basicHumanoidPlot);

            EquipmentSlot[] basicCreaturePlot = new EquipmentSlot[5];
            basicHumanoidPlot[0] = usableSlots["Head"];
            basicHumanoidPlot[1] = usableSlots["Face"];
            basicHumanoidPlot[2] = usableSlots["Torso"];
            basicHumanoidPlot[3] = usableSlots["Legs"];
            basicHumanoidPlot[4] = usableSlots["Feet"];
            bodyPlots.Add("Basic_Creature", basicCreaturePlot);
        }
    }
    [Serializable]
    public class EquipmentSlot
    {
        public string name { get; set; }
        public Item item { get; set; }
        public EquipmentSlot(string _name) { name = _name; }
        public EquipmentSlot() { }
    }
}
