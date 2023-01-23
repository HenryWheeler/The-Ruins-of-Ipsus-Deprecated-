using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class StatManager
    {
        public static RLConsole console;
        private static string spacer { get; set; }
        public static float average = 0;
        public static List<float> averages = new List<float>();
        public StatManager(RLConsole _console) 
        { 
            console = _console; 
            spacer = " + ";
        }
        public static void Average(long time)
        {
            averages.Add(time);

            float newAverage = 0;

            foreach (float current in averages)
            {
                newAverage += current;
            }
            newAverage /= averages.Count;
            average = newAverage;
            UpdateStats(Program.player);
        }
        public static void UpdateStats(Entity entity)
        {
            ClearStats();

            Stats stats = entity.GetComponent<Stats>();

            string display = "";

            //display += $"Average Tick Speed: {average}{spacer}";
            //display += $"Average Millisecond Speed: {average / 10000}{spacer}";
            display += $"Red*Health: {stats.hp}/{stats.hpCap}{spacer}";
            display += $"Light_Gray*Armor: {stats.ac}{spacer}";
            display += $"Yellow*Celerity: {stats.maxAction}{spacer}";
            display += $"Brown*Might: {stats.strength}{spacer}";
            display += $"Cyan*Acuity: {stats.acuity}{spacer}";
            display += $"Sight: {stats.sight}{spacer}";

            display += "Equipment: " + spacer;
            foreach (EquipmentSlot slot in entity.GetComponent<Inventory>().equipment)
            {
                if (slot != null)
                {
                    if (slot.item != null) 
                    {
                        display += $"{slot.name}: {slot.item.GetComponent<Description>().name},{spacer}"; 
                    }
                    else 
                    {
                        display += $"{slot.name}: Empty,{spacer}"; 
                    }
                }
            }

            display += $"Status:{spacer}";
            for (int i = 0; i < entity.GetComponent<Harmable>().statusEffects.Count; i++)
            {
                if (i == entity.GetComponent<Harmable>().statusEffects.Count - 1)
                { 
                    display += $"{entity.GetComponent<Harmable>().statusEffects[i]}."; 
                } 
                else 
                { 
                    display += $"{entity.GetComponent<Harmable>().statusEffects[i]}, "; 
                }
            }

            CMath.DisplayToConsole(console, display, 0, 2, 1);
            console.Print(11, 0, " The Rogue @ ", RLColor.White);
        }
        public static void ClearStats()
        {
            int h = console.Height - 2;
            int w = console.Width - 2;
            for (int y = (h); y >= 2; y--)
            {
                for (int x = 1; x < w + 1; x++)
                {
                    console.SetColor(x, y, RLColor.Black);
                }
            }
        }
    }
}
