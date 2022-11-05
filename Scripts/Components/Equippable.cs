using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Equippable: Component
    {
        public int type { get; set; }
        public bool equipped { get; set; }
        public string slot { get; set; }
        public void Equip(Entity entityRef)
        {
            entityRef.GetComponent<BodyPlot>().ReturnSlot(slot).item = entity;
            equipped = true;
            entity.GetComponent<Stats>().ModifyAllStats(entityRef, true);
            if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().AddAllToEntity(entityRef); }
        }
        public void Unequip(Entity entityRef)
        {
            entityRef.GetComponent<BodyPlot>().ReturnSlot(slot).item = null;
            equipped = false;
            entity.GetComponent<Stats>().ModifyAllStats(entityRef, false);
            if (entity.GetComponent<PropertyFunction>() != null) { entity.GetComponent<PropertyFunction>().RemoveAllFromEntity(entityRef); }
        }
        public Equippable(int _type, string _slot) { type = _type; equipped = false; slot = _slot; }
        public Equippable() { }
    }
}