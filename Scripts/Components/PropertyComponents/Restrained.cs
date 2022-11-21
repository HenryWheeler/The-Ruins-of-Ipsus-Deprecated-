using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Restrained: OnMoveProperty
    {
        public override void OnMove(int x1, int y1, int x2, int y2)
        {
            int random = CMath.random.Next(1, 100);
            if (random > 80) 
            { 
                entity.collectionsToRemove.Add(this);
                PronounSet pronouns = entity.GetComponent<PronounSet>();
                if (pronouns.present) { Log.AddToStoredLog(entity.GetComponent<Description>().name + " has freed " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
                else { Log.AddToStoredLog(entity.GetComponent<Description>().name + " have freed " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
            }
            else 
            {
                Coordinate coordinate = entity.GetComponent<Coordinate>();
                int nX = coordinate.x - x1;
                int nY = coordinate.y - y1;
                if (CMath.CheckBounds(coordinate.x - nX, coordinate.y - nY))
                {
                    PronounSet pronouns = entity.GetComponent<PronounSet>();
                    if (pronouns.present) { Log.AddToStoredLog(entity.GetComponent<Description>().name + " has failed to free " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
                    else { Log.AddToStoredLog(entity.GetComponent<Description>().name + " have failed to free " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
                    Map.map[coordinate.x, coordinate.y].actor = null;
                    coordinate.x -= nX; coordinate.y -= nY;
                    Map.map[coordinate.x, coordinate.y].actor = entity;
                }
            }
        }
        public Restrained() { special = true; componentName = "Restrained"; }
    }
}
