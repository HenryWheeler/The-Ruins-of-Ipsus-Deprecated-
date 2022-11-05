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
        public bool display { get; set; }
        public void Move(int _x, int _y)
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();

            if (CMath.CheckBounds(coordinate.x + _x, coordinate.y + _y) && Map.map[coordinate.x + _x, coordinate.y + _y].moveType != 0)
            {
                if (Map.map[coordinate.x + _x, coordinate.y + _y].actor == null)
                {
                    Map.map[coordinate.x, coordinate.y].actor = null;
                    coordinate.x += _x; coordinate.y += _y;
                    Map.map[coordinate.x, coordinate.y].actor = entity;
                    if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().TriggerOnMove(coordinate.x, coordinate.y); }
                    entity.GetComponent<TurnFunction>().EndTurn();
                }
                else if (display) { AttackManager.MeleeAllStrike(entity, Map.map[coordinate.x + _x, coordinate.y + _y].actor); }
                else { entity.GetComponent<TurnFunction>().EndTurn(); }
            }
            else if (display) { Log.AddToStoredLog("You cannot move there.", true); }
            else { entity.GetComponent<TurnFunction>().EndTurn(); }
        }
        public Movement(bool _display = false) { display = _display; }
        public Movement() { }
    }
}
