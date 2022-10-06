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
        public Stats(RLConsole _console, Player _player) { console = _console; player = _player; }
        public static void UpdateStats()
        {
            ClearStats();
            console.Print(2, 2, "Health: " + player.hp + "/" + player.hpCap, RLColor.White);
            console.Print(2, 4, "Armor: " + player.ac, RLColor.White);
            console.Print(2, 6, "Speed: " + player.actMax, RLColor.White);
            console.Print(2, 8, "Sight: " + player.sight, RLColor.White);
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
