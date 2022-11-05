using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class TurnFunction: Component
    {
        public bool turnActive { get; set; }
        public float actionLeft { get; set; }
        public bool display { get; set; }
        public void StartTurn()
        { 
            turnActive = true;
            if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().TriggerTurnStart(); }
            if (CMath.ReturnAI(entity) != null) { CMath.ReturnAI(entity).Action(); } 
            else if (!display) { EndTurn(); }
            if (display) { Log.DisplayLog(); } 
        }
        public void EndTurn() 
        { 
            turnActive = false;
            if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().TriggerTurnEnd(); }
            if (display)
            {
                Coordinate coordinate = entity.GetComponent<Coordinate>();
                ShadowcastFOV.ClearSight();
                ShadowcastFOV.Compute(coordinate.x, coordinate.y, entity.GetComponent<Stats>().sight);
            }
            TurnManager.ProgressActorTurn(this); 
        }
        public TurnFunction(float _actionLeft, bool _display = false) { turnActive = false; actionLeft = actionLeft; display = _display; }
        public TurnFunction() { }
    }
}
