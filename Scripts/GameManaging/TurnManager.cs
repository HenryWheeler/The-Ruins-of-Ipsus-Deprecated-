using System;
using System.Collections.Generic;
using RLNET;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class TurnManager
    {
        private static List<TurnFunction> entities = new List<TurnFunction>();
        private static int turn = 0;
        private static int entityTurn = 0;

        public static void ProgressTurnOrder()
        {
            turn++;
            if (entityTurn >= entities.Count - 1) entityTurn = 0;
            else entityTurn++;
            //if (entities[entityTurn].actionLeft <= 0) Task.WaitAll(ProgressActorTurn(entities[entityTurn]));
            if (entities[entityTurn].actionLeft <= 0) ProgressActorTurn(entities[entityTurn]);
            else entities[entityTurn].StartTurn();
        }
        public static void ProgressActorTurn(TurnFunction entity)
        {
            //await Task.Delay(1);
            if (entity.actionLeft <= 0) { entity.actionLeft += entity.entity.GetComponent<Stats>().maxAction; ProgressTurnOrder(); }
            else
            {
                entity.actionLeft--;
                if (entity.actionLeft <= 0) { entity.actionLeft += entity.entity.GetComponent<Stats>().maxAction; ProgressTurnOrder(); }
                else { entity.StartTurn(); }
            }
        }
        public static void AddActor(TurnFunction entity)
        {
            if (entity.entity != null && !entities.Contains(entity)) { entities.Add(entity); }
        }
        public static void RemoveActor(TurnFunction entity) { entities.Remove(entity); }
    }
}
