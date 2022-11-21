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
        public string mood { get; set; }
        public int memory { get; set; }
        public int maxMemory { get; set; }
        public string target { get; set; }
        public Entity referenceTarget { get; set; }
        public List<string> favoredEntities = new List<string>();
        public List<string> hatedEntities = new List<string>();
        public void OnHit(Entity attacker)
        {
            if (attacker.GetComponent<Faction>() != null && !hatedEntities.Contains(attacker.GetComponent<Faction>().faction)) 
            { hatedEntities.Add(attacker.GetComponent<Faction>().faction); target = attacker.GetComponent<Faction>().faction; }
            referenceTarget = attacker;
            mood = "Angry"; memory = maxMemory;
        }
        public void EvaluateEnvironment()
        {
            Coordinate coordinate = entity.GetComponent<Coordinate>();
            ShadowcastFOV.Compute(coordinate.x, coordinate.y, entity.GetComponent<Stats>().sight, this, true);
            if (referenceTarget != null && mood == "Angry")
            {
                if (target == null) { target = referenceTarget.GetComponent<Faction>().faction; memory = maxMemory; }
                else if (target == referenceTarget.GetComponent<Faction>().faction) { memory = maxMemory; }
                referenceTarget = null;
            }
            else if (memory > 0) { memory--; }
            else { target = null; mood = "Uncertain"; }
            ExecuteAction();
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
