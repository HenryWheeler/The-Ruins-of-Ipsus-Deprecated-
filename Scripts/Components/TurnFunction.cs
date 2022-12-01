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
            SpecialComponentManager.TriggerTurn(entity, true);
            if (CMath.ReturnAI(entity) != null) { CMath.ReturnAI(entity).EvaluateEnvironment(); } 
            else if (!display) { EndTurn(); }
            if (display) { Log.DisplayLog(); StatManager.UpdateStats(entity); } 
        }
        public void EndTurn() 
        { 
            turnActive = false;
            SpecialComponentManager.TriggerTurn(entity, false);
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
