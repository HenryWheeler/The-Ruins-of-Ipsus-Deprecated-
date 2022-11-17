using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class AI : Component
    {
        public int action { get; set; }
        public int memory { get; set; }
        public int maxMemory { get; set; }
        public Entity target { get; set; }
        public List<string> favoredEntities = new List<string>();
        public List<string> hatedEntities = new List<string>();
        public abstract void OnHit(Entity attacker);
        public void EvaluateEnvironment()
        {
            if (target == null)
            {
                Coordinate coordinate = entity.GetComponent<Coordinate>();
                ShadowcastFOV.Compute(coordinate.x, coordinate.y, entity.GetComponent<Stats>().sight, this, true);
                if (target != null)
                {
                    memory = maxMemory;
                    if (ReturnHatred(entity) > Math.Abs(ReturnConviction(entity, -1))) { action = 1; }
                    else { action = 1; }
                }
                else { action = CMath.random.Next(-1, 1); }
                ExecuteAction();
            }
            else if (CMath.Sight(entity, target)) { memory = maxMemory; ExecuteAction(); }
            else if (memory > 0) { memory--; ExecuteAction(); }
            else { target = null; EvaluateEnvironment(); }
            if (target != null)
            {
                Coordinate coordinate = entity.GetComponent<Coordinate>();
                Coordinate targetCoordinate = target.GetComponent<Coordinate>();
                if (targetCoordinate == coordinate) { target = null; }
            }
        }
        public abstract void ExecuteAction();
        public int ReturnHatred(Entity entity)
        {
            if (this.entity.GetComponent<Faction>() != null && entity.GetComponent<Faction>() != null)
            {
                if (favoredEntities.Contains(entity.GetComponent<Faction>().faction)
                    || favoredEntities.Contains(entity.GetComponent<Description>().name)) { return 0; }
                else if (hatedEntities.Contains(entity.GetComponent<Faction>().faction)
                    || hatedEntities.Contains(entity.GetComponent<Description>().name)) { return 1000; }
                else if (entity.GetComponent<Commerce>() != null) { return 500 - entity.GetComponent<Commerce>().value; }
                else { return 0; }
            }
            else
            {
                if (favoredEntities.Contains(entity.GetComponent<Description>().name)) { return 0; }
                else if (hatedEntities.Contains(entity.GetComponent<Description>().name)) { return 1000; }
                else if (entity.GetComponent<Commerce>() != null) { return 500 - entity.GetComponent<Commerce>().value; }
                else { return 0; }
            }
        }
        public int ReturnConviction(Entity entity, int hatred)
        {
            int acuity = entity.GetComponent<Stats>().acuity;
            if (acuity <= 0) { return 0; }
            else if (acuity >= 1 && acuity <= 10)
            {
                return (1000 * hatred * (PercievedStrength(entity) 
                    / PercievedStrength(this.entity))) / (int)CMath.Distance(this.entity.GetComponent<Coordinate>(), entity.GetComponent<Coordinate>());
            }
            else
            {
                return (500 * hatred * (PercievedStrength(entity) / PercievedStrength(this.entity) + 500 * hatred * (PercievedDanger(entity)
                 / PercievedDanger(this.entity)))) / (int)CMath.Distance(this.entity.GetComponent<Coordinate>(), entity.GetComponent<Coordinate>());
            }
        }
        public int PercievedStrength(Entity entity)
        {
            Stats stats = entity.GetComponent<Stats>();
            int value = (stats.strength + stats.acuity + (stats.hp / 10)) * (int)Math.Max(stats.maxAction, 1);
            if (value == 0) { value = 1; }
            return value;
        }
        public int PercievedDanger(Entity entity)
        {
            if (entity.GetComponent<BodyPlot>() != null)
            {
                int dangerValue = 1;
                foreach (EquipmentSlot slot in entity.GetComponent<BodyPlot>().bodyPlot)
                {
                    if (slot.item != null && slot.item.GetComponent<Commerce>() != null) { dangerValue *= slot.item.GetComponent<Commerce>().value; }
                }
                return dangerValue;
            }
            else { return 1; }
        }
        public AI() { }
    }
}
