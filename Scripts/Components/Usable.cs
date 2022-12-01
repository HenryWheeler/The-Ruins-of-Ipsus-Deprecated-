using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Usable: Component
    {
        public void Use(Entity user) 
        { 
            if (user.GetComponent<PronounSet>().present) { Log.AddToStoredLog(user.GetComponent<Description>().name + " has used the " + entity.GetComponent<Description>().name + "!"); }
            { Log.AddToStoredLog(user.GetComponent<Description>().name + " have used the " + entity.GetComponent<Description>().name + "!"); }
            SpecialComponentManager.TriggerOnUse(user, entity); 
        }
        public Usable() { }
    }
}
