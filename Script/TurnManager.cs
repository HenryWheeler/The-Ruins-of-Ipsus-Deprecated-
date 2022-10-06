using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    public class TurnManager
    {
        private static List<ActorBase> actors = new List<ActorBase>();
        private static int turn = 0;
        private static int actorTurn = 0;
        public static void ProgressTurnOrder()
        {
            turn++;
            if (actorTurn >= actors.Count - 1) actorTurn = 0;
            else actorTurn++;
            if (actors[actorTurn].actLeft <= 0) ProgressActorTurn(actors[actorTurn]);
            else actors[actorTurn].StartTurn();
        }
        public static void ProgressActorTurn(ActorBase actor)
        {
            if (actor.actLeft <= 0) { actor.actLeft += actor.actMax; ProgressTurnOrder(); }
            else
            {
                actor.actLeft--;
                if (actor.actLeft <= 0) { actor.actLeft += actor.actMax; ProgressTurnOrder(); }
                else { actor.StartTurn(); }
            }
        }
        public static void AddActor(ActorBase actor) { actors.Add(actor); }
        public static void RemoveActor(ActorBase actor) { actors.Remove(actor); }
    }
}
