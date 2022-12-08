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
            usableSlots.Add("Off Hand", new EquipmentSlot("Off Hand"));
            usableSlots.Add("Main Hand", new EquipmentSlot("Main Hand"));
            usableSlots.Add("Missile", new EquipmentSlot("Missile"));
            usableSlots.Add("Tail", new EquipmentSlot("Tail"));

            InitializeBodyPlots();
        }
        private void InitializeBodyPlots()
        {
            EquipmentSlot[] basicHumanoidPlot = new EquipmentSlot[8];
            basicHumanoidPlot[0] = usableSlots["Head"];
            basicHumanoidPlot[1] = usableSlots["Face"];
            basicHumanoidPlot[2] = usableSlots["Torso"];
            basicHumanoidPlot[3] = usableSlots["Legs"];
            basicHumanoidPlot[4] = usableSlots["Feet"];
            basicHumanoidPlot[5] = usableSlots["Off Hand"];
            basicHumanoidPlot[6] = usableSlots["Main Hand"];
            basicHumanoidPlot[7] = usableSlots["Missile"];
            bodyPlots.Add("Basic_Humanoid", basicHumanoidPlot);

            EquipmentSlot[] basicCreaturePlot = new EquipmentSlot[5];
            basicCreaturePlot[0] = usableSlots["Head"];
            basicCreaturePlot[1] = usableSlots["Face"];
            basicCreaturePlot[2] = usableSlots["Torso"];
            basicCreaturePlot[3] = usableSlots["Legs"];
            basicCreaturePlot[4] = usableSlots["Feet"];
            bodyPlots.Add("Basic_Creature", basicCreaturePlot);

            EquipmentSlot[] basicSpiderPlot = new EquipmentSlot[11];
            basicSpiderPlot[0] = usableSlots["Head"];
            basicSpiderPlot[1] = usableSlots["Face"];
            basicSpiderPlot[2] = usableSlots["Torso"];
            basicSpiderPlot[3] = usableSlots["Legs"];
            basicSpiderPlot[4] = usableSlots["Legs"];
            basicSpiderPlot[5] = usableSlots["Legs"];
            basicSpiderPlot[6] = usableSlots["Legs"];
            basicSpiderPlot[7] = usableSlots["Feet"];
            basicSpiderPlot[8] = usableSlots["Feet"];
            basicSpiderPlot[9] = usableSlots["Feet"];
            basicSpiderPlot[10] = usableSlots["Feet"];
            bodyPlots.Add("Basic_Spider", basicSpiderPlot);

            EquipmentSlot[] basicWormPlot = new EquipmentSlot[4];
            basicWormPlot[0] = usableSlots["Head"];
            basicWormPlot[1] = usableSlots["Face"];
            basicWormPlot[2] = usableSlots["Torso"];
            basicWormPlot[3] = usableSlots["Tail"];
            bodyPlots.Add("Basic_Worm", basicWormPlot);

            EquipmentSlot[] basicInsectPlot = new EquipmentSlot[9];
            basicInsectPlot[0] = usableSlots["Head"];
            basicInsectPlot[1] = usableSlots["Face"];
            basicInsectPlot[2] = usableSlots["Torso"];
            basicInsectPlot[3] = usableSlots["Legs"];
            basicInsectPlot[4] = usableSlots["Legs"];
            basicInsectPlot[5] = usableSlots["Legs"];
            basicInsectPlot[6] = usableSlots["Feet"];
            basicInsectPlot[7] = usableSlots["Feet"];
            basicInsectPlot[8] = usableSlots["Feet"];
            bodyPlots.Add("Basic_Insect", basicInsectPlot);

            EquipmentSlot[] basicCrustaceanPlot = new EquipmentSlot[13];
            basicCrustaceanPlot[0] = usableSlots["Head"];
            basicCrustaceanPlot[1] = usableSlots["Face"];
            basicCrustaceanPlot[2] = usableSlots["Torso"];
            basicCrustaceanPlot[3] = usableSlots["Legs"];
            basicCrustaceanPlot[4] = usableSlots["Legs"];
            basicCrustaceanPlot[5] = usableSlots["Legs"];
            basicCrustaceanPlot[6] = usableSlots["Legs"];
            basicCrustaceanPlot[7] = usableSlots["Feet"];
            basicCrustaceanPlot[8] = usableSlots["Feet"];
            basicCrustaceanPlot[9] = usableSlots["Feet"];
            basicCrustaceanPlot[10] = usableSlots["Feet"];
            basicCrustaceanPlot[11] = usableSlots["Main Hand"];
            basicCrustaceanPlot[12] = usableSlots["Off Hand"];
            bodyPlots.Add("Basic_Crustacean", basicCrustaceanPlot);
        }
    }
    [Serializable]
    public class EquipmentSlot
    {
        public string name { get; set; }
        public Entity item { get; set; }
        public EquipmentSlot(string _name) { name = _name; }
        public EquipmentSlot() { }
    }
}
