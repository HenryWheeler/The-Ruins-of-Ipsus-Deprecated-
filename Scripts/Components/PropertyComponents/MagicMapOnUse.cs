using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class MagicMapOnUse : OnUse
    {
        public override void Use(Entity entity, Vector2 target = null)
        {
            SpecialEffectManager.MagicMap(entity);
        }
        public MagicMapOnUse() 
        {
            strength = 0;
            singleUse = true;
            itemType = "Support";
            rangeModel = "All";
        }
    }
}
