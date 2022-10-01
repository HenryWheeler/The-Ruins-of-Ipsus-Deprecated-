using System;
using System.Collections.Generic;
using RLNET;
namespace RoguelikeTest
{
    public class TurnManager
    {
        private static List<ActorBase> actors = new List<ActorBase>();
        private static int turn = 0;
        private static int actorTurn = 0;
        public static void ProgressTurn()
        {
            turn++;
            if (actorTurn >= actors.Count - 1) actorTurn = 0;
            else actorTurn++;
            actors[actorTurn].StartTurn();
            actors[actorTurn].speed += (int)Math.Floor(actors[actorTurn].speedCap);
        }
        public static void AddActor(ActorBase actor) { actors.Add(actor); }
        public static void RemoveActor(ActorBase actor) { actors.Remove(actor); }
        public static void ActorTurnEnd() { ProgressTurn(); }
    }
}
