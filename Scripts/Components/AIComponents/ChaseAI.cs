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
        public override void Action()
        {
            entity.GetComponent<Memory>().memorizedEntity = Program.player;
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            Coordinate playerCoordinate = entity.GetComponent<Memory>().memorizedEntity.GetComponent<Coordinate>();
            if (CMath.Sight(entity, coordinate.x, coordinate.y, playerCoordinate.x, playerCoordinate.y)) { entity.GetComponent<Memory>().SetMemoryMax(); }
            if (entity.GetComponent<Memory>().TickMemory())
            {
                Coordinate coordinate2 = DijkstraMaps.PathFromMap(entity, coordinate.x, coordinate.y, "You0");
                entity.GetComponent<Movement>().Move(coordinate2.x, coordinate2.y);
            } 
            else { entity.GetComponent<TurnFunction>().EndTurn(); }
        }
        public override void OnHit(Entity attacker)
        {
            entity.GetComponent<Memory>().memorizedEntity = attacker;
        }
        public ChaseAI() { }
    }
}
