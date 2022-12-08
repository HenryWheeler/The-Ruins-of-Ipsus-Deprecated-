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
        public bool equipped { get; set; }
        public string slot { get; set; }
        public bool unequipable { get; set; }
        public bool addProperties { get; set; }
        public void Equip(Entity entityRef)
        {
            entityRef.GetComponent<BodyPlot>().ReturnSlot(slot).item = entity;
            equipped = true;
            if (entity.GetComponent<Stats>() != null) { entity.GetComponent<Stats>().ModifyAllStats(entityRef, true); }
            //if (entity.GetComponent<SpecialDefences>() != null) { }
            if (addProperties) { SpecialComponentManager.AddAllToEntity(entityRef, entity); }
        }
        public void Unequip(Entity entityRef)
        {
            entityRef.GetComponent<BodyPlot>().ReturnSlot(slot).item = null;
            equipped = false;
            if (entity.GetComponent<Stats>() != null) { entity.GetComponent<Stats>().ModifyAllStats(entityRef, false); }
            //if (entity.GetComponent<SpecialDefences>() != null) { }
            if (addProperties) { SpecialComponentManager.RemoveAllFromEntity(entityRef, entity); }
        }
        public Equippable(string _slot, bool _unequipable, bool _addProperties) { equipped = false; slot = _slot; unequipable = _unequipable; addProperties = _addProperties; }
        public Equippable() { }
    }
}