using System;
using System.Collections.Generic;
using RLNET;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class ActorBase : IDraw, IDescription
    {
        public bool turnActive = false;
        public int x { get; set; }
        public int y { get; set; }
        public int hp { get; set; }
        public int hpCap { get; set; }
        public int ac { get; set; }
        public int sight { get; set; }
        public char character { get; set; }
        public RLColor fColor { get; set; }
        public RLColor bColor { get; set; }
        public bool opaque { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float actLeft { get; set; }
        public float actMax { get; set; }
        public string spacer { get; set; }
        public List<Item> inventory { get; set; }
        public List<AtkData> attacks { get; set; }
        public EquipmentSlot[] bodyPlot { get; set; }
        public string bodyPlotName { get; set; }
        public abstract string Describe();
        public void Draw(RLConsole console) { console.Set(x, y, fColor, bColor, character); }
        public void Attack(ActorBase target, ActorBase attacker, int type)
        {
            List<AtkData> atkDataTotal = new List<AtkData>();
            foreach(AtkData atkData in attacker.attacks) { atkDataTotal.Add(atkData); }
            if (atkDataTotal.Count == 0) { atkDataTotal.Add(new AtkData("Basic", 0, "1-1-0-0")); }
            int totalDmg = 0;
            int numberOfHits = 0;
            foreach (AtkData atkData in atkDataTotal)
            {
                string[] dataArray = atkData.dmgData.Split('-');
                int d1 = int.Parse(dataArray[0]);
                int d2 = int.Parse(dataArray[1]);
                int d3 = int.Parse(dataArray[2]);
                int d4 = int.Parse(dataArray[3]);

                if (CMath.random.Next(0, 20) + d4 >= target.ac)
                {
                    int dmg = 0;
                    for (int d = 0; d < d1; d++)
                    {
                        dmg += CMath.random.Next(1, d2);
                    }
                    dmg += d3;

                    target.OnHit(dmg, attacker);

                    totalDmg += dmg; numberOfHits++;
                }
            }
            if (numberOfHits != 0)
            {
                if (numberOfHits == 1) { Log.AddToStoredLog(name + " hit " + target.name + " " + numberOfHits + " time for " + totalDmg + " damage!"); }
                else { Log.AddToStoredLog(name + " hit " + target.name + " " + numberOfHits + " times for " + totalDmg + " damage!"); }
            }
            EndTurn();
        }
        public abstract void OnHit(int dmg, ActorBase attacker);
        public abstract void Death();
        public abstract void StartTurn();
        public abstract void EndTurn();
    }
    [Serializable]
    public class AtkData
    {
        public string dmgData { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public AtkData(string _name = "Basic", int _type = 0, string _dmgData = "1-1-0-0") { dmgData = _dmgData; name = _name; type = _type; }
        public AtkData() { }
    }
}
