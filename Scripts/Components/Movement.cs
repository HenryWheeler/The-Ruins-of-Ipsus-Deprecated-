using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Movement: Component
    {
        public List<int> moveTypes = new List<int>();
        public void Move(int _x, int _y)
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();

            if (CMath.CheckBounds(coordinate.x + _x, coordinate.y + _y) && moveTypes.Contains(Map.map[coordinate.x + _x, coordinate.y + _y].moveType))
            {
                if (Map.map[coordinate.x + _x, coordinate.y + _y].actor == null)
                {
                    Map.map[coordinate.x, coordinate.y].actor = null;
                    coordinate.x += _x; coordinate.y += _y;
                    Map.map[coordinate.x, coordinate.y].actor = entity;
                    SpecialComponentManager.TriggerOnMove(entity, coordinate.x - _x, coordinate.y - _y, coordinate.x, coordinate.y);
                    if (Map.map[coordinate.x, coordinate.y].terrain != null) { SpecialComponentManager.TriggerOnMove(Map.map[coordinate.x, coordinate.y].terrain, coordinate.x - _x, coordinate.y - _y, coordinate.x, coordinate.y); }
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
                else if (entity.display) { AttackManager.MeleeAllStrike(entity, Map.map[coordinate.x + _x, coordinate.y + _y].actor); }
                else { entity.GetComponent<TurnFunction>().EndTurn(); }
            }
            else if (entity.display) { Log.AddToStoredLog("You cannot move there.", true); }
            else { entity.GetComponent<TurnFunction>().EndTurn(); }
        }
        public Movement(bool canWalk = false, bool canSwim = false, bool canPhase = false) 
        {
            if (canWalk) { moveTypes.Add(1); }
            if (canSwim) { moveTypes.Add(2); }
            if (canPhase) { moveTypes.Add(0); }
        }
        public Movement() { }
    }
}
