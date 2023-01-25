using System;
using System.Collections.Generic;
using System.Linq;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class Log
    {
        public static RLConsole console;
        private static Queue<string> log = new Queue<string>();
        private static int maxLogCount = 15;
        private static string spacer { get; set; }
        public Log(RLConsole _console) 
        { 
            console = _console;
            spacer = " + ";
            for (int i = 0; i < maxLogCount; i++)
            {
                log.Enqueue("");
            }
        }
        public static void DisplayLog()
        {
            ClearLogDisplay();

            int m = 0;
            int y = 0;
            int c = 1;

            string[] temp = log.ToArray<string>();
            //temp.Reverse();

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                string[] outPut = temp[i].Split(' ');

                foreach (string text in outPut)
                {
                    string[] split = text.Split('*');
                    if (split.Count() == 1)
                    {
                        if (split[0].Contains("+")) { y += 2 + m; c = 1; }
                        else
                        {
                            if (c + split[0].Length > console.Width - 5) { y += 2 + m; c = 1; }
                            console.Print(c + 1, y, split[0], RLColor.Blend(RLColor.Gray, RLColor.White, 1 - .05f * (i * 2)));
                            c += split[0].Length + 1;
                        }
                    }
                    else
                    {
                        if (split[1].Contains("+")) { y += 2 + m; c = 1; }
                        else
                        {
                            if (c + split[0].Length > console.Width - 5) { y += 2 + m; c = 1; }
                            console.Print(c + 1, y, split[1], ColorFinder.ColorPicker(split[0]));
                            c += split[1].Length + 1;
                        }
                    }
                }
            }
            Renderer.CreateConsoleBorder(console);
            console.Print(11, 0, " Message Log ", RLColor.White);
        }
        public static void OutputParticleLog(string log, string color, Vector2 position)
        {
            string name = "";

            foreach (string text in log.Split(' '))
            {
                string[] split = text.Split('*');
                if (split.Count() == 1)
                {
                    name += split[0] + " ";
                }
                else
                {
                    name += split[1] + " ";
                }
            }

            char[] characters = name.ToCharArray();
            int firstX = position.x - (characters.Length / 2);

            for (int i = 0; i < characters.Length; i++)
            {
                if (CMath.CheckBounds(firstX, position.y))
                {
                    Entity particle = new Entity(new List<Component>
                                {
                                    new Vector2(0, 0),
                                    new Draw(color, "Black", characters[i]),
                                    new ParticleComponent(World.random.Next(9, 12), 2, "North", 1, new Draw[] { new Draw(color, "Black", characters[i]) })
                                });
                    Renderer.AddParticle(firstX, position.y, particle);
                }

                firstX++;
            }
        }
        public static void AddToStoredLog(string logAdd)
        {
            string newString = spacer + logAdd;
            log.Enqueue(newString);
            if (log.Count > maxLogCount) 
            {
                log.Dequeue();
            }
        }
        public static void Add(string logAdd)
        {
            log.Enqueue(spacer + logAdd);
            if (log.Count > maxLogCount)
            {
                log.Dequeue();
            }
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
