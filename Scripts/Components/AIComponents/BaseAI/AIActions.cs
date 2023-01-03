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
            Traversable traversable = World.tiles[positionToMove.x, positionToMove.y].GetComponent<Traversable>();
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
