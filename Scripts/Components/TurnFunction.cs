﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class TurnFunction: Component
    {
        public List<OnTurn> onTurnComponents = new List<OnTurn>();
        public bool turnActive = false;
        public float actionLeft { get; set; }
        public void StartTurn()
        { 
            if (entity != null)
            {
                turnActive = true;
                TriggerTurnComponents(true);

                if (CMath.ReturnAI(entity) != null) 
                {
                    CMath.ReturnAI(entity).Process();
                }
                else if (entity.GetComponent<PlayerComponent>() != null)
                {
                    Log.DisplayLog();
                    StatManager.UpdateStats(entity);
                }
                else
                {
                    EndTurn();
                }
            }
            else 
            { 
                TurnManager.ProgressTurnOrder();
            }
            entity.ClearCollections();

            Renderer.DrawToScreen();
        }
        public void EndTurn() 
        { 
            turnActive = false;
            TriggerTurnComponents(false);
            if (entity.GetComponent<PlayerComponent>() != null)
            {
                Vector2 vector3 = entity.GetComponent<Vector2>();
                ShadowcastFOV.ClearSight();
                ShadowcastFOV.Compute(vector3, entity.GetComponent<Stats>().sight);
            }
            TurnManager.ProgressActorTurn(this);

            Renderer.DrawToScreen();
        }
        public void TriggerTurnComponents(bool start)
        {
            foreach (OnTurn component in onTurnComponents)
            {
                if (component != null && start && component.start)
                {
                    component.Turn();
                }
                else if (component != null && !start && !component.start)
                {
                    component.Turn();
                }
            }
        }
        public TurnFunction() { }
    }
}
