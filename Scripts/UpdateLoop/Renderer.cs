using System;
using RLNET;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace TheRuinsOfIpsus
{
    public class Renderer
    {
        private static RLRootConsole rootConsole;
        public static RLConsole mapConsole;
        public static int mapWidth;
        public static int mapHeight;
        private static RLConsole messageConsole;
        public static int messageWidth;
        private static int messageHeight;
        private static RLConsole rogueConsole;
        public static int rogueWidth;
        private static int rogueHeight;
        private static RLConsole actionConsole;
        public static int actionWidth;
        private static int actionHeight;
        public static bool inventoryOpen = false;
        public static bool threadRunning = false;
        public static List<ParticleComponent> particles = new List<ParticleComponent>();
        public static int current = 1;
        public static bool running = false;
        public static bool playingAnimation = false;
        public static int minX { get; set; }
        public static int maxX { get; set; }
        public static int minY { get; set; }
        public static int maxY { get; set; }
        public Renderer(RLRootConsole _rootConsole, RLConsole _mapConsole, int _mapWidth, int _mapHeight, RLConsole _messageConsole, int _messageWidth, int _messageHeight, RLConsole _rogueConsole, int _rogueWidth, int _rogueHeight, RLConsole _actionConsole, int _actionWidth, int _actionHeight)
        {
            rootConsole = _rootConsole;
            mapConsole = _mapConsole;
            mapWidth = _mapWidth;
            mapHeight = _mapHeight;

            messageConsole = _messageConsole;
            messageWidth = _messageWidth;
            messageHeight = _messageHeight;

            rogueConsole = _rogueConsole;
            rogueWidth = _rogueWidth;
            rogueHeight = _rogueHeight;

            actionConsole = _actionConsole;
            actionWidth = _actionWidth;
            actionHeight = _actionHeight;
            rootConsole.Render += Render;

            running = true;

            Thread thread = new Thread(() => RenderParticles());
            thread.Start();
        }
        public void Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            if (Program.gameActive)
            {
                RenderMap();
                RenderLog();
                RenderRogue();
                RenderAction();
            }
            else { RenderMenu(); }
            rootConsole.Draw();
        }
        public static void AddParticle(int x, int y, Entity particle)
        {
            particle.GetComponent<Coordinate>().vector2 = new Vector2(x, y);
            World.tiles[x, y].sfxLayer = particle;
            particles.Add(particle.GetComponent<ParticleComponent>());
        }
        public static void RenderParticles()
        {
            while (running)
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
        public void RenderMenu()
        {
            CreateConsoleBorder(rootConsole);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 14, " ______  _             ____         __                   ___   __                          ", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 13, "|__  __|| |__   ____  |__  | __ __ |__| ___  ____   ___ | __| |  | ____  ____  __ __  ____ ", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 12, "  |  |  | |  | | __ | |   _||  |  ||  ||   ||  __| | _ ||  _| |  || __ ||  __||  |  ||  __|", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 11, "  |  |  |  _  || ___| | | | |  |  ||  || | ||___ | ||_||| |   |  ||  __||___ ||  |  ||___ |", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 46, (rootConsole.Height / 3) - 10, "  |__|  |_| |_||____| |_|_| |_____||__||_|_||____| |___||_|   |__||_|   |____||_____||____|", RLColor.White);
            rootConsole.Print((rootConsole.Width / 2) - 7, (rootConsole.Height / 2) - 6, "New Game: [N]", RLColor.White);
            if (SaveDataManager.savePresent) { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.White); }
            else { rootConsole.Print((rootConsole.Width / 2) - 10, (rootConsole.Height / 2) - 3, "Load Save Game: [L]", RLColor.Gray); }
            rootConsole.Print((rootConsole.Width / 2) - 5, rootConsole.Height / 2, "Quit: [Q]", RLColor.White);
        }
        public static void StartAnimationThread(List<Entity> sfx, int repeatCount, int delay)
        {
            if (!threadRunning)
            {
                Thread thread = new Thread(() => PlayAnimation(sfx, repeatCount, delay));
                thread.Start();
            }
            else
            {
                Thread thread = new Thread(() => WaitList(sfx, repeatCount, delay));
                thread.Start();
            }
        }
        public static void WaitList(List<Entity> sfx, int repeatCount, int delay)
        {
            try
            {
                while (threadRunning)
                {
                    Thread.Sleep(10);
                }
                Thread thread = new Thread(() => PlayAnimation(sfx, repeatCount, delay));
                thread.Start();
                Thread thisThread = Thread.CurrentThread;
                thisThread.Abort();
            }
            catch (Exception ex) { ex = null; return; }
        }
        public static void PlayAnimation(List<Entity> sfx, int repeatCount, int delay)
        {
            try
            {
                threadRunning = true;
                TurnManager.threadRunning = true;
                int current = 0;
                do
                {
                    foreach (Entity frame in sfx)
                    {
                        if (frame != null)
                        {
                            World.GetTraversable(frame.GetComponent<Coordinate>().vector2).sfxLayer = frame;
                            frame.GetComponent<AnimationFunction>().ProgressFrame();
                        }
                    }
                    //RenderMap();
                    Thread.Sleep(delay);
                    current++;
                } while (current != repeatCount);
                foreach (Entity frame in sfx)
                {
                    if (frame != null)
                    {
                        World.GetTraversable(frame.GetComponent<Coordinate>().vector2).sfxLayer = null;
                    }
                }
                threadRunning = false;
                TurnManager.threadRunning = false;
                Thread thread = Thread.CurrentThread;
                thread.Abort();
            }
            catch (Exception ex) { ex = null; return; }
        }
        public static void MoveCamera(Vector2 vector3)
        {
            minX = vector3.x - mapWidth / 2;
            maxX = minX + mapWidth;
            minY = vector3.y - mapHeight / 2;
            maxY = minY + mapHeight;
        }
        public static void RenderMap()
        {
            //mapConsole.Clear();
            RLConsole.Blit(mapConsole, 0, 0, mapWidth, mapHeight, rootConsole, messageWidth, 0);

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
                        if (traversable.sfxLayer != null) { traversable.sfxLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (!visibility.visible && !visibility.explored) { mapConsole.Set(x, y, RLColor.Black, RLColor.Black, '?'); }
                        else if (!visibility.visible && visibility.explored)
                        {
                            if (traversable.obstacleLayer != null)
                            {
                                Draw draw = traversable.obstacleLayer.GetComponent<Draw>();
                                mapConsole.Set(x, y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(draw.bColor), .55f), draw.character);
                            }
                            else
                            {
                                Draw draw = tile.GetComponent<Draw>();
                                mapConsole.Set(x, y, ColorFinder.ColorPicker("Dark_Gray"), RLColor.Blend(RLColor.Black, ColorFinder.ColorPicker(draw.bColor), .55f), draw.character);
                            }
                        }
                        else if (traversable.actorLayer != null) { traversable.actorLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (traversable.itemLayer != null) { traversable.itemLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else if (traversable.obstacleLayer != null) { traversable.obstacleLayer.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                        else { tile.GetComponent<Draw>().DrawToScreen(mapConsole, x, y); }
                    }
                    else { mapConsole.Set(x, y, RLColor.Black, RLColor.Black, '?'); }
                    x++;
                }
                y++;
            }
            CreateConsoleBorder(mapConsole);
            mapConsole.Print((mapWidth / 2) - 3, 0, " Map ", RLColor.White);
        }
        public static void RenderLog()
        {
            RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, 0, 0);
        }
        public static void RenderRogue()
        {
            RLConsole.Blit(rogueConsole, 0, 0, rogueWidth, rogueHeight, rootConsole, mapWidth + messageWidth, 0);
        }
        public static void RenderAction()
        {
            RLConsole.Blit(actionConsole, 0, 0, actionWidth, actionHeight, rootConsole, 0, messageHeight);
        }
        public static void CreateConsoleBorder(RLConsole console)
        {
            int h = console.Height - 1;
            int w = console.Width - 1;
            for (int y = h; y >= 0; y--)
            {
                for (int x = 0; x < w + 1; x++)
                {
                    if (y == h && x == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)192); }
                    else if (y == h && x == w) { console.Set(x, y, RLColor.White, RLColor.Black, (char)217); }
                    else if (y == 0 && x == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)218); }
                    else if (x == w && y == 0) { console.Set(x, y, RLColor.White, RLColor.Black, (char)191); }
                    else if (y == 0 || y == h) { console.Set(x, y, RLColor.White, RLColor.Black, (char)196); }
                    else if (x == 0 || x == w) { console.Set(x, y, RLColor.White, RLColor.Black, (char)179); }
                }
            }
            if (console == messageConsole) { console.Print(6, 0, " Message Log ", RLColor.White); }
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
            Vector2 position = entity.GetComponent<Coordinate>().vector2;

            if (CMath.CheckBounds(position.x, position.y))
            {
                World.tiles[position.x, position.y].sfxLayer = null;
            }

            switch (direction)
            {
                case "Target":
                    {
                        Vector2 newPosition = DijkstraMaps.PathFromMap(position, "ParticlePath");
                        if (position == newPosition)
                        {
                            KillParticle();
                            return;
                        }
                        position = newPosition;
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
            }

            if (CMath.CheckBounds(position.x, position.y))
            {
                World.tiles[position.x, position.y].sfxLayer = entity;
            }

            currentThreshold--;

            if (currentThreshold <= 0)
            {
                currentThreshold = threshold;
                if (currentParticle == particles.Length - 1)
                {
                    currentParticle = 0;
                }
                else
                {
                    currentParticle++;
                }

                Draw draw = entity.GetComponent<Draw>();
                draw.character = particles[currentParticle].character;
                draw.fColor = particles[currentParticle].fColor;
                draw.bColor = particles[currentParticle].bColor;
            }

            life--;
            if (life <= 0 && direction != "Target")
            {
                KillParticle();
                return;
            }
        }
        public void KillParticle()
        {
            Vector2 position = entity.GetComponent<Coordinate>().vector2;
            Renderer.particles.Remove(this);
            if (CMath.CheckBounds(position.x, position.y))
            {
                World.tiles[position.x, position.y].sfxLayer = null;
            }
            if (animation)
            {
                Renderer.playingAnimation = false;
            }
        }
        public ParticleComponent(int _life, int _speed, string _direction, int _threshHold, Draw[] _particles, Vector2 target = null, bool animation = false)
        {
            life = _life;
            speed = _speed;
            direction = _direction;
            threshold = _threshHold;
            particles = _particles;

            if (target != null)
            {
                DijkstraMaps.CreateMap(target, "ParticlePath");
                direction = "Target";
            }

            if (animation)
            {
                animation = true;
                Renderer.playingAnimation = true;
            }
        }
    }
}
