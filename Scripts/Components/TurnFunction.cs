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
            if (entity != null)
            {
                if (CMath.ReturnAI(entity) != null) 
                {
                    CMath.ReturnAI(entity).Process();
                }
                else if (!entity.display) 
                {
                    EndTurn();
                }
                if (entity.display) 
                {
                    Log.DisplayLog();
                    StatManager.UpdateStats(entity); 
                }
            }
            else 
            { 
                TurnManager.ProgressTurnOrder();
            }
        }
        public void EndTurn() 
        { 
            turnActive = false;
            SpecialComponentManager.TriggerTurn(entity, false);
            if (entity.display)
            {
                Vector2 vector3 = entity.GetComponent<Coordinate>().vector2;
                ShadowcastFOV.ClearSight();
                ShadowcastFOV.Compute(vector3, entity.GetComponent<Stats>().sight);
            }
            TurnManager.ProgressActorTurn(this);
        }
        public TurnFunction() { }
    }
}
