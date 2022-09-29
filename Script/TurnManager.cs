using System;
using System.Collections.Generic;
using RLNET;
namespace RoguelikeTest
{
    public class TurnManager
    {
        private static List<ActorBase> actors = new List<ActorBase>();
        private static List<ActorBase> activeTurn = new List<ActorBase>();
        private int turn = 0;
        private int actorTurn = 0;
        private RLRootConsole console;
        private static bool actorChange = true;
        public TurnManager(RLRootConsole _console) { console = _console; console.Update += Update; }
        public void ProgressTurn()
        {
            actorChange = false;
            activeTurn.Clear();
            if (turn <= 15) turn = 1;
            else turn++;
            foreach (ActorBase actor in actors)
            {
                if (actor.speed.Contains(turn)) activeTurn.Add(actor);
            }
            actorChange = true;
        }
        public void Update(object sender, UpdateEventArgs e)
        {
            if (actorChange)
            {
                if (actorTurn <= activeTurn.Count - 1)
                {
                    activeTurn[actorTurn].StartTurn();
                }
                else ProgressTurn();
            }
        }
        public static void AddActor(ActorBase actor) { actors.Add(actor); }
        public static void RemoveActor(ActorBase actor) { actors.Remove(actor); }
        public static void ActorTurnEnd() { actorChange = true; }
    }
}
