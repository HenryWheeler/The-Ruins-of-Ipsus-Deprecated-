using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheRuinsOfIpsus
{
    public class Renderer
    {
        public static int mapWidth;
        public static int mapHeight;
        public static TitleConsole mapConsole;
        public static bool inventoryOpen = false;
        public static List<ParticleComponent> particles = new List<ParticleComponent>();
        public static List<Entity> clearList = new List<Entity>();
        public static int current = 1;
        public static bool running = false;
        public static int minX { get; set; }
        public static int maxX { get; set; }
        public static int minY { get; set; }
        public static int maxY { get; set; }


        public Renderer(TitleConsole _mapConsole, int _mapWidth, int _mapHeight)
        {
            //rootConsole = _rootConsole;
            mapConsole = _mapConsole;
            mapWidth = _mapWidth;
            mapHeight = _mapHeight;
        }
        public static void AddParticle(int x, int y, Entity particle)
        {
            if (particles.Count > 0)
            {
                Log.Add("Started Thread");
                Thread thread = new Thread(() => RenderParticles());
                thread.Start();
            }
            particle.GetComponent<Vector2>().x = x;
            particle.GetComponent<Vector2>().y = y;
            World.tiles[x, y].sfxLayer = particle;
            ParticleComponent particleComponent = particle.GetComponent<ParticleComponent>();
            particles.Add(particleComponent);
        }
        public static void StartAnimation(List<Entity> particlesRef)
        {
            foreach (Entity particle in particlesRef)
            {
                if (particle != null)
                {
                    Vector2 vector2 = particle.GetComponent<Vector2>();
                    World.tiles[vector2.x, vector2.y].sfxLayer = particle;
                    ParticleComponent particleComponent = particle.GetComponent<ParticleComponent>();
                    particles.Add(particleComponent);
                }
            }
            Log.Add("Started Thread");
            Thread thread = new Thread(() => RenderParticles());
            thread.Start();
            thread.Join();
        }
        public static void RenderParticles()
        {
            while (particles.Count > 0)
            {
                if (particles.Count > 0)
                {
                    for (int i = 0; i < particles.Count; i++)
                    {
                        ParticleComponent particle = particles[i];
                        switch (particle.speed)
                        {
                            case 1: { if (current == 1) { break; } else { continue; } }
                            case 2: { if (current == 2 || current == 7) { break; } else { continue; } }
                            case 3: { if (current == 3 || current == 6 || current == 9) { break; } else { continue; } }
                            case 4: { if (current == 2 || current == 4 || current == 6 || current == 8 || current == 10) { break; } else { continue; } }
                            case 5: { break; }
                        }
                        particle.Progress();
                    }
                }

                DrawToScreen();

                Thread.Sleep(TimeSpan.FromMilliseconds(16.66f));

                if (current == 10)
                {
                    current = 1;
                }
                else
                {
                    current++;
                }
            }
        }  
        public static void RenderMenu()
        {
            /*
            if (Menu.openingScreen)
            {
                rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 14, " ______  _             ____         __                   ___   __                          ", RLColor.White);
                rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 13, "|__/ __||/|__   ____  |__  | __ __ |__| ___  ____   ___ |/__| |/ | ____  ____  __ __  ____ ", RLColor.White);
                rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 12, "  |/ |  |/|  | |/__ | |/  _||/ |  ||/ ||/  ||/ __| |/_ ||/ _| |/ ||/__ ||/ __||/ |  ||/ __|", RLColor.White);
                rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 11, "  |/ |  |/ _  ||/___| |/| | |/ |  ||/ ||/| ||___ | ||_|||/|   |/ ||/ __||___ ||/ |  ||___ |", RLColor.White);
                rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 10, "  |__|  |_| |_||____| |_|_| |_____||__||_|_||____| |___||_|   |__||_|   |____||_____||____|", RLColor.White);
                rootConsole.Print((rootConsole.Width / 2) - 7, (rootConsole.Height / 2) - 6, "New Game: [N]", RLColor.White);
                if (SaveDataManager.savePresent) { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.White); }
                else { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.Gray); }
                rootConsole.Print((rootConsole.Width / 2) - 5, rootConsole.Height / 2, "Quit: [Q]", RLColor.White);
            }
            else
            {
                for (int x = 0; x < 100; x++)
                {
                    for (int y = 48; y > 40; y--)
                    {
                        //rootConsole.Set(x, y, ColorFinder.ColorPicker("Dark_Brown"), ColorFinder.ColorPicker("Black"), (char)177);
                    }
                }
                for (int x = 0; x < 100; x++)
                {
                    for (int y = 40; y > 32; y--)
                    {
                        //rootConsole.Set(x, y, ColorFinder.ColorPicker("Dark_Brown"), ColorFinder.ColorPicker("Dark_Gray"), (char)177);
                    }
                }
                for (int x = 0; x < 100; x++)
                {
                    //rootConsole.Set(x, 32, ColorFinder.ColorPicker("Dark_Brown"), ColorFinder.ColorPicker("Dark_Gray"), ',');
                }
                for (int x = 0; x < 100; x++)
                {
                    //rootConsole.Set(x, 31, ColorFinder.ColorPicker("Green"), ColorFinder.ColorPicker("Dark_Green"), 'x');
                }
                for (int x = 20; x < 80; x++)
                {
                    for (int y = 30; y > 5; y--)
                    {
                        //rootConsole.Set(x, y, ColorFinder.ColorPicker("Gray"), ColorFinder.ColorPicker("Dark_Gray"), (char)177);
                    }
                }

                string[] nameParts = Menu.causeOfDeath.Split(' ');
                string name = "";
                foreach (string part in nameParts)
                {
                    string[] temp = part.Split('*');
                    if (temp.Length == 1)
                    {
                        name += temp[0] + " ";
                    }
                    else
                    {
                        name += temp[1] + " ";
                    }
                }

                //Program.rootConsole.Print(50 - (int)Math.Ceiling((double)name.Length/ 2 + 1), (rootConsole.Height / 3) - 2, " " + name.Trim() + ". ", ColorFinder.ColorPicker("Light_Gray"), RLColor.Black);

                rootConsole.Print((rootConsole.Width / 2) - 13, (rootConsole.Height / 2) + 16, "New Game: [N] - Quit: [Q]", RLColor.Brown, RLColor.Black);
            }
            //CreateConsoleBorder(rootConsole);
            */
        }
        public static void MoveCamera(Vector2 vector3)
        {
            minX = vector3.x - mapWidth / 2;
            maxX = minX + mapWidth;
            minY = vector3.y - mapHeight / 2;
            maxY = minY + mapHeight;
        }
        public static void DrawToScreen()
        {
            int y = 0;
            for (int ty = minY; ty < maxY; ty++)
            {
                int x = 0;
                for (int tx = minX; tx < maxX; tx++)
                {
                    if (CMath.CheckBounds(tx, ty))
                    {
                        Entity tile = World.tiles[tx, ty].entity;
                        Visibility visibility = tile.GetComponent<Visibility>();
                        Traversable traversable = tile.GetComponent<Traversable>();
                        Entity sfx = traversable.sfxLayer;
                        if (sfx != null)
                        {
                            sfx.GetComponent<Draw>().DrawToScreen(mapConsole, x, y);
                        }
                        else if (!visibility.visible && !visibility.explored) { mapConsole.SetCellAppearance(x, y, new Cell(Color.Black, Color.Black, '?')); }
                        else if (!visibility.visible && visibility.explored)
                        {
                            if (traversable.obstacleLayer != null)
                            {
                                Draw draw = traversable.obstacleLayer.GetComponent<Draw>();
                                mapConsole.SetCellAppearance(x, y, new Cell(Color.DarkGray, Color.Black, draw.character));
                            }
                            else
                            {
                                Draw draw = tile.GetComponent<Draw>();
                                mapConsole.SetCellAppearance(x, y, new Cell(Color.DarkGray, Color.Black, draw.character));
                            }
                        }
                        else if (traversable.actorLayer != null) { traversable.actorLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (traversable.itemLayer != null) { traversable.itemLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (traversable.obstacleLayer != null) { traversable.obstacleLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else { tile.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                    }
                    else { mapConsole.SetCellAppearance(x, y, new Cell(Color.Black, Color.Black, '?')); }
                    x++;
                }
                y++;
            }

            if (clearList.Count != 0)
            {
                Entity[] newArray = new Entity[10000];
                clearList.CopyTo(newArray);
                foreach (Entity particle in newArray)
                {
                    if (particle != null)
                    {
                        Vector2 position = particle.GetComponent<Vector2>();
                        if (CMath.CheckBounds(position.x, position.y))
                        {
                            World.tiles[position.x, position.y].sfxLayer = null;
                            clearList.Remove(particle);
                        }
                    }
                }
            }

            CreateConsoleBorder(mapConsole, " Map ");

            Global.CurrentScreen.IsDirty = true;
            //mapConsole.Print((mapWidth / 2) - 2, 0, " Map ", RLColor.White);
        }
        public static void RenderMap()
        {
        }
        public static void RenderLog()
        {
            //RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, mapWidth, rogueHeight);
        }
        public static void RenderRogue()
        {
            //RLConsole.Blit(rogueConsole, 0, 0, rogueWidth, rogueHeight, rootConsole, mapWidth, 0);
        }
        public static void CreateConsoleBorder(Console console, string title)
        {
            int h = console.Height - 1;
            int w = console.Width - 1;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 192)); }
                    else if (y == h && x == w) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 217)); }
                    else if (y == 0 && x == 0) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 218)); }
                    else if (x == w && y == 0) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 191)); }
                    else if (y == 0 || y == h) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 196)); }
                    else if (x == 0 || x == w) { console.SetCellAppearance(x, y, new Cell(Color.White, Color.Black, 179)); }
                }
            }

            console.Print(0, 0, title.Align(HorizontalAlignment.Center, console.Width), Color.Black, Color.White);
        }
    }
    public class ParticleComponent: Component
    {
        public int life { get; set; }
        public int speed { get; set; }
        public string direction { get; set; }
        public int threshold { get; set; }
        public int currentThreshold = 0;
        public Draw[] particles { get; set; }
        public int currentParticle = 0;
        public bool animation = false;
        public void Progress()
        {
            Vector2 position = entity.GetComponent<Vector2>();

            if (CMath.CheckBounds(position.x, position.y))
            {
                World.tiles[position.x, position.y].sfxLayer = null;
            }

            switch (direction)
            {
                case "Attached":
                    {
                        //Work for later, go make particle that can be stuck to an entity.
                        break;
                    }
                case "Target":
                    {
                        Vector2 newPosition = DijkstraMaps.PathFromMap(position, "ParticlePath");
                        entity.GetComponent<Vector2>().x = newPosition.x;
                        entity.GetComponent<Vector2>().y = newPosition.y;
                        break;
                    }
                case "Wander": 
                    {
                        position.x += World.random.Next(-1, 2);
                        position.y += World.random.Next(-1, 2);
                        break;
                    }
                case "None": { break; }
                case "North": 
                    {
                        position.y--;
                        break; 
                    }
                case "NorthEast": 
                    {
                        position.x--; 
                        position.y--;
                        break;
                    }
                case "East": 
                    { 
                        position.x--; 
                        break;
                    }
                case "SouthEast":
                    {
                        position.x--;
                        position.y++;  
                        break;
                    }
                case "South":
                    {
                        position.y++;
                        break;
                    }
                case "SouthWest":
                    {
                        position.x++; 
                        position.y++;
                        break;
                    }
                case "West":
                    {
                        position.x++;
                        break;
                    }
                case "NorthWest": 
                    {
                        position.x++; 
                        position.y--;
                        break; 
                    }
                case "WanderNorth":
                    {
                        position.x += World.random.Next(-1, 2);
                        position.y += World.random.Next(-1, 0);
                        break;
                    }
                case "WanderNorthEast":
                    {
                        position.x += World.random.Next(-1, 0);
                        position.y += World.random.Next(-1, 0);
                        break;
                    }
                case "WanderEast":
                    {
                        position.x += World.random.Next(-1, 0);
                        position.y += World.random.Next(-1, 2);
                        break;
                    }
                case "WanderSouthEast":
                    {
                        position.x += World.random.Next(-1, 0);
                        position.y += World.random.Next(0, 2);
                        break;
                    }
                case "WanderSouth":
                    {
                        position.x += World.random.Next(-1, 2);
                        position.y += World.random.Next(0, 2);
                        break;
                    }
                case "WanderSouthWest":
                    {
                        position.x += World.random.Next(0, 2);
                        position.y += World.random.Next(0, 2);
                        break;
                    }
                case "WanderWest":
                    {
                        position.x += World.random.Next(0, 2);
                        position.y += World.random.Next(-1, 2);
                        break;
                    }
                case "WanderNorthWest":
                    {
                        position.x += World.random.Next(0, 2);
                        position.y += World.random.Next(-1, 0);
                        break;
                    }
            }

            if (CMath.CheckBounds(position.x, position.y))
            {
                World.tiles[position.x, position.y].sfxLayer = entity;
            }

            currentThreshold--;

            if (currentThreshold <= 0)
            {
                currentThreshold = threshold;
                if (currentParticle != particles.Length - 1)
                {
                    currentParticle++;
                }
                else
                {
                    currentParticle = 0;
                }

                Draw draw = entity.GetComponent<Draw>();
                draw.character = particles[currentParticle].character;
                draw.fColor = particles[currentParticle].fColor;
                draw.bColor = particles[currentParticle].bColor;
            }

            life--;
            if (life <= 0)
            {
                KillParticle();
                return;
            }
        }
        public void KillParticle()
        {
            Renderer.particles.Remove(this);
            Renderer.clearList.Add(entity);
        }
        public ParticleComponent(int _life, int _speed, string _direction, int _threshHold, Draw[] _particles, Vector2 target = null, bool _animation = false)
        {
            life = _life;
            speed = _speed;
            direction = _direction;
            threshold = _threshHold;
            particles = _particles;

            if (target != null)
            {
                DijkstraMaps.CreateMap(target, "ParticlePath");
            }

            if (_animation)
            {
                animation = true;
            }
        }
    }
}
