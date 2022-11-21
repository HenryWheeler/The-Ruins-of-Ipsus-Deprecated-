using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class ChaseAI : AI
    {
        public override void ExecuteAction()
        {
            switch (mood)
            {
                case "Uncertain": entity.GetComponent<Movement>().Move(CMath.random.Next(-1, 2), CMath.random.Next(-1, 2)); break;
                case "Angry":
                    {
                        Coordinate coordinate = entity.GetComponent<Coordinate>();
                        Coordinate targetCoordinate = DijkstraMaps.PathFromMap(entity, target);
                        if (Map.map[coordinate.x + targetCoordinate.x, coordinate.y + targetCoordinate.y].actor != null &&
                            Map.map[coordinate.x + targetCoordinate.x, coordinate.y + targetCoordinate.y].actor != entity)
                        { AttackManager.MeleeAllStrike(entity, Map.map[coordinate.x + targetCoordinate.x, coordinate.y + targetCoordinate.y].actor); }
                        else { entity.GetComponent<Movement>().Move(targetCoordinate.x, targetCoordinate.y); }
                        break;
                    }
                case "Fearful": entity.GetComponent<TurnFunction>().EndTurn(); break;
            }
        }
        public ChaseAI(int _maxMemory) { maxMemory = _maxMemory; }
        public ChaseAI() { }
    }
}
