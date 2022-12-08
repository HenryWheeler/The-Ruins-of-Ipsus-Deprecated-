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
        public bool turnActive = false;
        public float actionLeft { get; set; }
        public void StartTurn()
        { 
            turnActive = true;
            SpecialComponentManager.TriggerTurn(entity, true);
            if (CMath.ReturnAI(entity) != null) { CMath.ReturnAI(entity).EvaluateEnvironment(); } 
            else if (!entity.display) { EndTurn(); }
            if (entity.display) { Log.DisplayLog(); StatManager.UpdateStats(entity); } 
        }
        public void EndTurn() 
        { 
            turnActive = false;
            SpecialComponentManager.TriggerTurn(entity, false);
            if (entity.display)
            {
                Coordinate coordinate = entity.GetComponent<Coordinate>();
                ShadowcastFOV.ClearSight();
                ShadowcastFOV.Compute(coordinate.x, coordinate.y, entity.GetComponent<Stats>().sight);
            }
            TurnManager.ProgressActorTurn(this);
        }
        public TurnFunction() { }
    }
}
