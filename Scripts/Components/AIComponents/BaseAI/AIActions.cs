using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    class AIActions
    {
        public static void TestHuntAction(Entity AI)
        {
            AI detail = CMath.ReturnAI(AI);
            Vector2 positionToMove = DijkstraMaps.PathFromMap(AI, detail.target.GetComponent<Faction>().faction);
            Traversable traversable = World.tiles[positionToMove.x, positionToMove.y];
            if (traversable.actorLayer != null && detail.hatedEntities.Contains(traversable.actorLayer.GetComponent<Faction>().faction))
            {
                AttackManager.MeleeAllStrike(AI, traversable.actorLayer);
            }
            else
            {
                AI.GetComponent<Movement>().Move(new Vector2(positionToMove.x, positionToMove.y));
                detail.interest--;
                //Log.Add($"{AI.GetComponent<Description>().name}'s current interest is {detail.interest}");
                //if (detail.interest <= 0)
                //{
                //    CMath.ReturnAI(AI).currentInput = TheRuinsOfIpsus.AI.Input.Tired;
                //}
            }
        }
        public static void TestPatrol(Entity AI)
        {
            PatrolFunction patrol = AI.GetComponent<PatrolFunction>();
            patrol.lastPastInformation--;
            Vector2 positionToMove = DijkstraMaps.PathFromMap(AI, $"Patrol{patrol.patrolRoute}");
            AI.GetComponent<Movement>().Move(new Vector2(positionToMove.x, positionToMove.y));
            if (positionToMove.x == patrol.lastPosition.x && positionToMove.y == patrol.lastPosition.y)
            {
                patrol.patrolRoute = World.seed.Next(0, 20);
            }
            else if (World.tiles[positionToMove.x, positionToMove.y].actorLayer != null && World.tiles[positionToMove.x, positionToMove.y].actorLayer != AI && CMath.ReturnAI(World.tiles[positionToMove.x, positionToMove.y].actorLayer) != null)
            {
                if (CMath.ReturnAI(AI).hatedEntities.Count != 0 && CMath.ReturnAI(AI).favoredEntities.Contains(World.tiles[positionToMove.x, positionToMove.y].actorLayer.GetComponent<Faction>().faction))
                {
                    foreach (string hated in CMath.ReturnAI(AI).hatedEntities)
                    {
                        CMath.ReturnAI(World.tiles[positionToMove.x, positionToMove.y].actorLayer).hatedEntities.Add(hated);
                    }
                    Log.Add("Someone spoke hatred");
                }
            }
            patrol.lastPosition = positionToMove;
        }
        public static void TestSleep(Entity AI)
        {
            AI detail = CMath.ReturnAI(AI);
            detail.interest--;

            if (detail.interest <= 0)
            {
                detail.interest = detail.baseInterest;
                detail.currentInput = TheRuinsOfIpsus.AI.Input.Noise;
            }
            AI.GetComponent<TurnFunction>().EndTurn();
        }
        public static void TestAwake(Entity AI)
        {
            AI detail = CMath.ReturnAI(AI);
            detail.interest--;

            if (detail.interest <= 0)
            {
                detail.interest = detail.baseInterest;
                detail.currentInput = TheRuinsOfIpsus.AI.Input.Tired;
            }
            AI.GetComponent<TurnFunction>().EndTurn();
        }
    }
}
