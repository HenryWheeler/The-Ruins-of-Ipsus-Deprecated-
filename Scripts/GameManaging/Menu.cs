using System;
using System.Windows.Forms;
using SadConsole;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Menu
    {
        public static bool openingScreen = true;
        public static string causeOfDeath { get; set; }
        public static void MakeSelection(int selection)
        {
            if (selection == 0) { Program.NewGame(); }
            else if (selection == 1 && SaveDataManager.savePresent) { SaveDataManager.LoadSave(); }
            else if (selection == 2) { Program.gameActive = false; Renderer.running = false; Game.Instance.Exit(); }
        }
        public static void EndGame(string _causeOfDeath)
        {
            causeOfDeath = "You were slain by " + _causeOfDeath;
            openingScreen = false;

            SaveDataManager.DeleteSave();
            foreach (TurnFunction entity in TurnManager.entities)
            {
                entity.turnActive = false;
            }
            TurnManager.entities.Clear();
            Program.gameActive = false;
        }
    }
}
