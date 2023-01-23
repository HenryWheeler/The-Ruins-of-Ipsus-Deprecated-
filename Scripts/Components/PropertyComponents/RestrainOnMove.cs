using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class RestrainOnMove: OnMoveProperty
    {
        public override void OnMove(Vector2 initialPosition, Vector2 finalPosition)
        {
            int chance = World.random.Next(1, 100);
            if (chance > 75)
            {
                //Vector2 vector3 = entity.GetComponent<Coordinate>().vector2;
                Traversable traversable = World.GetTraversable(finalPosition);
                if (traversable.actorLayer != null)
                {
                    if (!traversable.actorLayer.GetComponent<Stats>().immunities.Contains("Restraint")) 
                    {
                        traversable.actorLayer.GetComponent<Harmable>().statusEffects.Add("Restrained");
                        if (traversable.actorLayer.GetComponent<PronounSet>().present) { Log.AddToStoredLog(traversable.actorLayer.GetComponent<Description>().name + " has been restrained in the " + entity.GetComponent<Description>().name + "."); }
                        else { Log.AddToStoredLog(traversable.actorLayer.GetComponent<Description>().name + " have been restrained in the " + entity.GetComponent<Description>().name + "."); }
                        traversable.obstacleLayer = null;
                    }                       
                }
            }
        }
        public RestrainOnMove() { }
    }
}
