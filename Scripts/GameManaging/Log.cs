using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Log
    {
        private static RLConsole console;
        private static List<string> storedLog = new List<string>();
        private static string spacer { get; set; }
        public Log(RLConsole _console) { console = _console; spacer = " + "; }
        public static void DisplayLog()
        {
            ClearLogDisplay();

            if (storedLog.Count != 0)
            {
                storedLog.Reverse();

                string logOut = null;
                foreach (string log in storedLog) { logOut += log; }
                CMath.DisplayToConsole(console, logOut, 1, 1, 0, 0);
                storedLog.Clear();
            }
        }
        public static void AddToStoredLog(string logAdd, bool display = false)
        {
            string newString = null; newString = spacer + logAdd;
            storedLog.Add(newString);
            if (display) DisplayLog();
        }
        public static void ClearLogDisplay()
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
