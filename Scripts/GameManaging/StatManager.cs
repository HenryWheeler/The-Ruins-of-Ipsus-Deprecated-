﻿using System;
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
        public StatManager(RLConsole _console) { console = _console; spacer = " + "; }
        public static void UpdateStats(Entity entity)
        {
            ClearStats();

            Stats stats = entity.GetComponent<Stats>();

            stats.display = true;

            string display = "";

            display += "Red*Health: " + stats.hp + "/" + stats.hpCap + spacer;
            display += "Light_Gray*Armor: " + stats.ac + spacer;
            display += "Yellow*Celerity: " + stats.maxAction + spacer;
            display += "Brown*Might: " + stats.strength + " (" + (stats.strength - 10)/2 + ")" + spacer;
            display += "Blue*Acuity: " + stats.acuity + " (" + (stats.acuity - 10)/2 + ")" + spacer;
            display += "Sight: " + stats.sight + spacer + spacer;

            display += "Equipment: " + spacer + spacer;

            foreach (EquipmentSlot slot in entity.GetComponent<BodyPlot>().bodyPlot)
            {
                if (slot != null)
                {
                    if (slot.item != null) { display += slot.name + ": " + slot.item.GetComponent<Description>().name + spacer; }
                    else { display += slot.name + ": Empty" + spacer; }
                }
            }

            display += spacer + "Status:" + spacer;

            foreach (Component component in entity.components)
            {
                if (component.special && component.componentName != "") { display += component.componentName + ", "; }
            }

            CMath.DisplayToConsole(console, display, 0, 2, 1);
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
        public static void DisplayStats() { Renderer.CreateConsoleBorder(console); console.Print(14, 0, " The Rogue @ ", RLColor.White); }
    }
}