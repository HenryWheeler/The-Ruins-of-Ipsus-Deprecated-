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
        public void Move(Vector2 newPosition)
        {
            if (entity.GetComponent<OnHit>().statusEffects.Contains("Restrained"))
            {
                PronounSet pronouns = entity.GetComponent<PronounSet>();
                if (World.random.Next(1, 21) + ((entity.GetComponent<Stats>().strength - 10) / 2) > 10)
                {
                    if (pronouns.present)
                    { 
                        Log.Add($"{entity.GetComponent<Description>().name} has freed {pronouns.reflexive} from {pronouns.possesive} restraints."); 
                    }
                    else 
                    { 
                        Log.Add($"{entity.GetComponent<Description>().name} have freed {pronouns.reflexive} from {pronouns.possesive} restraints."); 
                    }
                    entity.GetComponent<OnHit>().statusEffects.Remove("Restrained");
                }
                else
                {
                    if (pronouns.present) 
                    { 
                        Log.Add($"{entity.GetComponent<Description>().name} struggles in {pronouns.possesive} restraints."); 
                    }
                    else
                    { 
                        Log.Add($"{ entity.GetComponent<Description>().name} struggle in {pronouns.possesive} restraints."); 
                    }
                }
                entity.GetComponent<TurnFunction>().EndTurn();
            }
            else
            {
                Vector2 originalPosition = entity.GetComponent<Coordinate>().vector2;
                Traversable newTraversable = World.GetTraversable(newPosition);
                if (CMath.CheckBounds(newPosition.x, newPosition.y) && moveTypes.Contains(newTraversable.terrainType))
                {
                    if (newTraversable.actorLayer == null)
                    {
                        World.GetTraversable(originalPosition).actorLayer = null;
                        entity.GetComponent<Coordinate>().vector2 = newPosition;
                        newTraversable.actorLayer = entity;
                        SpecialComponentManager.TriggerOnMove(entity, originalPosition, newPosition);
                        if (newTraversable.obstacleLayer != null)
                        { 
                            SpecialComponentManager.TriggerOnMove(newTraversable.obstacleLayer, originalPosition, newPosition);
                        }
                        entity.GetComponent<TurnFunction>().EndTurn();
                        EntityManager.UpdateMap(entity);
                    }
                    else if (CMath.ReturnAI(newTraversable.actorLayer) != null && !CMath.ReturnAI(newTraversable.actorLayer).hatedEntities.Contains(entity.GetComponent<Faction>().faction))
                    {
                        World.GetTraversable(originalPosition).actorLayer = newTraversable.actorLayer;
                        newTraversable.actorLayer.GetComponent<Coordinate>().vector2 = entity.GetComponent<Coordinate>().vector2;
                        EntityManager.UpdateMap(newTraversable.actorLayer);
                        entity.GetComponent<Coordinate>().vector2 = newPosition;
                        newTraversable.actorLayer = entity;
                        SpecialComponentManager.TriggerOnMove(entity, originalPosition, newPosition);
                        if (newTraversable.obstacleLayer != null)
                        {
                            SpecialComponentManager.TriggerOnMove(newTraversable.obstacleLayer, originalPosition, newPosition);
                        }
                        entity.GetComponent<TurnFunction>().EndTurn();
                        EntityManager.UpdateMap(entity);
                    }
                    else if (entity.display) 
                    { 
                        AttackManager.MeleeAllStrike(entity, newTraversable.actorLayer); 
                    }
                    else 
                    {
                        entity.GetComponent<TurnFunction>().EndTurn();
                    }
                }
                else if (entity.display) 
                { 
                    Log.Add("You cannot move there.");
                    Log.DisplayLog();
                }
                else 
                {
                    entity.GetComponent<TurnFunction>().EndTurn(); 
                }
            }
        }
        public Movement(List<int> _moveTypes) 
        {
            moveTypes = _moveTypes;
        }
        public Movement() { }
    }
}
