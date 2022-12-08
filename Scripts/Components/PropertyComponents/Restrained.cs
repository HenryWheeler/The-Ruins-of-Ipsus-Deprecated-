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
        public override void OnMove(Vector3 initialPosition, Vector3 finalPosition)
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
                Vector3 vector3 = entity.GetComponent<Coordinate>().vector3;
                int nX = vector3.x - initialPosition.x;
                int nY = vector3.y - initialPosition.y;
                int nZ = vector3.z - initialPosition.z;
                if (CMath.CheckBounds(vector3.x - nX, vector3.y - nY))
                {
                    PronounSet pronouns = entity.GetComponent<PronounSet>();
                    if (pronouns.present) { Log.AddToStoredLog(entity.GetComponent<Description>().name + " has failed to free " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
                    else { Log.AddToStoredLog(entity.GetComponent<Description>().name + " have failed to free " + pronouns.reflexive + " from " + pronouns.possesive + " restraints."); }
                    //World.tiles[vector3.x, vector3.y, vector3.z].actor = null;
                    vector3.x -= nX; vector3.y -= nY; vector3.z -= nZ;
                    //Map.map[coordinate.x, coordinate.y].actor = entity;
                }
            }
        }
        public Restrained() { special = true; componentName = "Restrained"; }
    }
}
