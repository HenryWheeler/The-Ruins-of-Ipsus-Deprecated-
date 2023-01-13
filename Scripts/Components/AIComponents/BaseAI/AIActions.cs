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
            if (detail.target != null)
            {
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
            else
            {
                AI.GetComponent<TurnFunction>().EndTurn();
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
        public static void TestMimicWait(Entity AI)
        {
            AI detail = CMath.ReturnAI(AI);

            if (!AI.GetComponent<Mimicry>().disguised)
            {
                if (!AI.GetComponent<Mimicry>().CaptureGuise())
                {
                    if (DijkstraMaps.maps.ContainsKey("Obstacles"))
                    {
                        Vector2 vector2 = DijkstraMaps.PathFromMap(AI, "Obstacles");
                        AI.GetComponent<Movement>().Move(vector2);
                    }
                    else if (DijkstraMaps.maps.ContainsKey("Items"))
                    {
                        Vector2 vector2 = DijkstraMaps.PathFromMap(AI, "Items");
                        AI.GetComponent<Movement>().Move(vector2);
                    }
                    else
                    {
                        AI.GetComponent<TurnFunction>().EndTurn();
                    }
                }
            }
            else
            {
                detail.interest--;
                if (detail.interest <= 0)
                {
                    Vector2 vector2 = AI.GetComponent<Coordinate>().vector2;
                    AI.GetComponent<Movement>().Move(new Vector2(vector2.x + World.random.Next(-1, 2), vector2.y + World.random.Next(-1, 2)));
                    AI.GetComponent<Mimicry>().disguised = false;
                    detail.interest = detail.baseInterest;
                }
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
