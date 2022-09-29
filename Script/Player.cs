using System;
using RLNET;

namespace RoguelikeTest
{
    class Player : ActorBase
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
            sight = 30;
            speed.Add(5); speed.Add(10); speed.Add(15);
        }
        public override void StartTurn()
        {
            turnActive = true;
        }
        public override void EndTurn()
        {
            turnActive = false;
            TurnManager.ActorTurnEnd();
        }
        public void Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null && turnActive)
            {
                switch (keyPress.Key)
                {
                    case RLKey.Up: Move(0, -1); break;
                    case RLKey.Down: Move(0, 1); break;
                    case RLKey.Left: Move(-1, 0); break;
                    case RLKey.Right: Move(1, 0); break;
                    case RLKey.Keypad8: Move(0, -1); break;
                    case RLKey.Keypad9: Move(1, -1); break;
                    case RLKey.Keypad6: Move(1, 0); break;
                    case RLKey.Keypad3: Move(1, 1); break;
                    case RLKey.Keypad2: Move(0, 1); break;
                    case RLKey.Keypad1: Move(-1, 1); break;
                    case RLKey.Keypad4: Move(-1, 0); break;
                    case RLKey.Keypad7: Move(-1, -1); break;
                }
            }
        }
        public void Move(int _x, int _y)
        {
            if (Map.map[x + _x, y + _y].walkable)
            {
                Clear();
                Map.map[x, y].actor = null;
                x += _x; y += _y;
                Map.map[x, y].actor = this;
                FOV();
                EndTurn();
            }
        }
        public void Clear() { ShadowcastFOV.ClearSight(); }
        public void FOV() { ShadowcastFOV.Compute(x, y, sight); }
    }
}
