using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    [Serializable]
    public class Player : ActorBase
    {
        RLRootConsole rootConsole;
        public Player(RLRootConsole _rootConsole)
        {
            rootConsole = _rootConsole;
            rootConsole.Update += Update;
            Initialize();
            StartTurn();
        }
        public void Initialize()
        {
            character = '@';
            fColor = RLColor.Green;
            bColor = RLColor.Black;
            name = "Player";
            sight = 10;
            actMax = 1;
            hpCap = 500;
            hp = hpCap;
            ac = 10;
        }
        public override void Death() { }
        public override void StartTurn() { turnActive = true; Log.DisplayLog(); Log.ClearStoredLog(); }
        public override void EndTurn() { MakeMap(); turnActive = false; TurnManager.ProgressActorTurn(this); }
        public void Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null && turnActive)
            {
                switch (keyPress.Key)
                {
                    case RLKey.Up: Log.ClearLogDisplay(); Move(0, -1); break;
                    case RLKey.Down: Log.ClearLogDisplay(); Move(0, 1); break;
                    case RLKey.Left: Log.ClearLogDisplay(); Move(-1, 0); break;
                    case RLKey.Right: Log.ClearLogDisplay(); Move(1, 0); break;
                    case RLKey.Keypad8: Log.ClearLogDisplay(); Move(0, -1); break;
                    case RLKey.Keypad9: Log.ClearLogDisplay(); Move(1, -1); break;
                    case RLKey.Keypad6: Log.ClearLogDisplay(); Move(1, 0); break;
                    case RLKey.Keypad3: Log.ClearLogDisplay(); Move(1, 1); break;
                    case RLKey.Keypad2: Log.ClearLogDisplay(); Move(0, 1); break;
                    case RLKey.Keypad1: Log.ClearLogDisplay(); Move(-1, 1); break;
                    case RLKey.Keypad4: Log.ClearLogDisplay(); Move(-1, 0); break;
                    case RLKey.Keypad7: Log.ClearLogDisplay(); Move(-1, -1); break;
                    case RLKey.Space: Log.ClearLogDisplay(); EndTurn(); break;
                }
            }
        }
        public void MakeMap()
        {
            List<Node> nodes = new List<Node>
            {
                new Node(x, y, 0)
            };
            DijkstraMaps.CreateMap(new Node(x, y, 0), "Player");
            // ShowMap("Player");
        }
        public void ShowMap(string name)
        {
            Node[,] map = DijkstraMaps.maps[name];
            foreach (Node node in map)
            {
                if (node != null)
                {
                    switch (node.v)
                    {
                        case 0: Map.map[node.x, node.y].character = '0'; break;
                        case 1: Map.map[node.x, node.y].character = '1'; break;
                        case 2: Map.map[node.x, node.y].character = '2'; break;
                        case 3: Map.map[node.x, node.y].character = '3'; break;
                        case 4: Map.map[node.x, node.y].character = '4'; break;
                        case 5: Map.map[node.x, node.y].character = '5'; break;
                        case 6: Map.map[node.x, node.y].character = '6'; break;
                        case 7: Map.map[node.x, node.y].character = '7'; break;
                        case 8: Map.map[node.x, node.y].character = '8'; break;
                        case 9: Map.map[node.x, node.y].character = '9'; break;
                    }
                }
            }
        }
        public void Move(int _x, int _y)
        {
            if (Map.map[x + _x, y + _y].walkable && Map.map[x + _x, y + _y].actor == null)
            {
                Clear();
                Map.map[x, y].actor = null;
                x += _x; y += _y;
                Map.map[x, y].actor = this;
                FOV();
                Log.AddToStoredLog("Player moved to tile (" + x.ToString() + ", " + y.ToString() + ")."); 
                EndTurn();
            }
        }
        public void Clear() { ShadowcastFOV.ClearSight(); }
        public void FOV() { ShadowcastFOV.Compute(x, y, sight); }
    }
}
