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
            if (storedLog.Count != 0)
            {
                storedLog.Reverse();

                string logOut = null;
                foreach (string log in storedLog)
                {
                    logOut += log;
                }
                logOut += ".";
                char[] outPut = logOut.ToCharArray();
                int y = 2;
                int s = 0;
                for (int c = 0; c < outPut.Length - 1; c++)
                {
                    switch (c)
                    {
                        case 153: { y = 3; s = 153; break; }
                        case 306: { y = 4; s = 306; break; }
                        case 459: { y = 5; s = 459; break; }
                        case 612: { y = 6; s = 612; break; }
                        case 765: { y = 7; s = 765; break; }
                        case 918: { y = 8; s = 918; break; }
                        case 1071: { y = 9; s = 1071; break; }
                        case 1224: { y = 10; s = 1224; break; }
                    }
                    console.Set(c + 1 - s, y, RLColor.White, RLColor.Black, outPut[c]);
                }
                ClearStoredLog();
            }
        }
        public static void AddToStoredLog(string logAdd)
        {
            string newString = null;
            newString = " " + logAdd;
            storedLog.Add(newString);
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
