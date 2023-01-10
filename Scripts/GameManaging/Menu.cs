using System;
using System.Windows.Forms;
using RLNET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class Menu
    {
        private static RLRootConsole rootConsole { get; set; }
        public Menu(RLRootConsole _rootConsole) { rootConsole = _rootConsole; rootConsole.Update += Update; }
        public static void Update(object sender, UpdateEventArgs e)
        {
            if (!Program.gameActive)
            {
                RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
                if (keyPress != null) { Action.MenuAction(keyPress.Key, rootConsole); }
            }
        }
        public static void MakeSelection(int selection)
        {
            if (selection == 0) { Program.NewGame(); rootConsole.Update -= Update; }
            else if (selection == 1 && SaveDataManager.savePresent) { SaveDataManager.LoadSave(); rootConsole.Update -= Update; }
            else if (selection == 2) { Program.gameActive = false; Renderer.running = false; Program.rootConsole.Close(); }
        }
    }
}
