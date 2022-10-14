using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Log
    {
        private static RLConsole console;
        private static List<string> storedLog = new List<string>();
        public Log(RLConsole _console) { console = _console; }
        public static void DisplayLog()
        {
            ClearLogDisplay();

            if (storedLog.Count != 0)
            {
                storedLog.Reverse();

                string logOut = null;
                foreach (string log in storedLog) { logOut += log; }
                string[] outPut = logOut.Split(' ');
                int y = 2;
                int c = 0;
                foreach (string text in outPut)
                {
                    if (c + text.Length > console.Width - 4) { y += 2; c = 1; }
                    console.Print(c + 1, y, text, RLColor.White);
                    c += text.Length + 1;
                }
                ClearStoredLog();
            }
        }
        public static void AddToStoredLog(string logAdd, bool display = false)
        {
            string newString = null;
            newString = " " + logAdd;
            storedLog.Add(newString);
            if (display) DisplayLog();
        }
        public static void ClearStoredLog() { storedLog.Clear(); }
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
