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
        public StatManager(RLConsole _console) 
        { 
            console = _console; 
            spacer = " + ";
        }
        public static void UpdateStats(Entity entity)
        {
            ClearStats();

            Stats stats = entity.GetComponent<Stats>();

            string display = "";

            display += $"Red*Health: {stats.hp}/{stats.hpCap}{spacer}";
            display += $"Light_Gray*Armor: {stats.ac}{spacer}";
            display += $"Yellow*Speed: {stats.maxAction}{spacer}";
            display += $"Brown*Might: {stats.strength}{spacer}";
            display += $"Cyan*Acuity: {stats.acuity}{spacer}";
            display += $"Sight: {stats.sight}{spacer}";

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

            CMath.DisplayToConsole(Program.rogueConsole, $"Open Inventory Yellow*[I] {spacer}Open Equipment Yellow*[E]", 0, 2, 1, 29, false);

            console.Print(2, 0, $" Stats {(char)196} ", RLColor.White);
            console.Print(11, 0, $"Equipment ", RLColor.Gray);
            console.Print(21, 0, $"{(char)196}", RLColor.White);
            console.Print(22, 0, $" Inventory ", RLColor.Gray);
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
