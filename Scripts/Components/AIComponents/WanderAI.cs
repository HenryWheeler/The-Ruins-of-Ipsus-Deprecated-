using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class WanderAI : AI
    {
        public override void Action()
        {
            entity.GetComponent<Movement>().Move(CMath.random.Next(-1, 2), CMath.random.Next(-1, 2));
        }
        public override void OnHit(Entity attacker)
        {

        }
        public WanderAI() { }
    }
}
