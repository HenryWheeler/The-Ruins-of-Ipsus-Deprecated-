using System;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Stats
    {
        public static Player player;
        public static RLConsole console;
        private static string spacer { get; set; }
        private static string displayStats { get; set; }
        public Stats(RLConsole _console, Player _player) { console = _console; player = _player; spacer = " + "; }
        public Stats(Player _player) { player = _player; }
        public static void UpdateStats()
        {
            ClearStats();

            displayStats += "Health: " + player.hp + "/" + player.hpCap + spacer;
            displayStats += "Armor: " + player.ac + spacer;
            displayStats += "Speed: " + player.actMax + spacer;
            displayStats += "Sight: " + player.sight + spacer + spacer;

            displayStats += "Equipment: " + spacer + spacer;

            foreach (EquipmentSlot slot in player.bodyPlot)
            {
                if (slot != null)
                {
                    if (slot.item != null) { displayStats += slot.name + ": " + slot.item.name + spacer; }
                    else { displayStats += slot.name + ": Empty" + spacer; }
                }
            }

            CMath.DisplayToConsole(console, displayStats, 0, 2, 1);
        }
        public static void ClearStats()
        {
            displayStats = "";
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
