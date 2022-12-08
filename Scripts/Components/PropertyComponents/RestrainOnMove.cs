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
        public override void OnMove(Vector3 initialPosition, Vector3 finalPosition)
        {
            int chance = CMath.random.Next(1, 100);
            if (chance > 75)
            {
                Vector3 vector3 = entity.GetComponent<Coordinate>().vector3;
                if (Map.map[coordinate.x, coordinate.y].actor != null)
                {
                    if (!Map.map[coordinate.x, coordinate.y].actor.GetComponent<Stats>().immunities.Contains("Restraint")) 
                    {
                        Map.map[coordinate.x, coordinate.y].actor.AddComponent(new Restrained());
                        if (Map.map[coordinate.x, coordinate.y].actor.GetComponent<PronounSet>().present) { Log.AddToStoredLog(Map.map[coordinate.x, coordinate.y].actor.GetComponent<Description>().name + " has been restrained in the " + entity.GetComponent<Description>().name + "."); }
                        else { Log.AddToStoredLog(Map.map[coordinate.x, coordinate.y].actor.GetComponent<Description>().name + " have been restrained in the " + entity.GetComponent<Description>().name + "."); }
                        Map.map[coordinate.x, coordinate.y].terrain = null;
                    }                       
                }
            }
        }
        public RestrainOnMove() { special = true; }
    }
}
