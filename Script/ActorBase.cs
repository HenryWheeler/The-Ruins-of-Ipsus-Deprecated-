using System;
using System.Collections.Generic;
using RLNET;

namespace RoguelikeTest
{
    [Serializable]
    public abstract class ActorBase : IDraw
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
        public float actLeft { get; set; }
        public float actMax { get; set; }
        public void Draw(RLConsole console) { console.Set(x, y, fColor, bColor, character); }
        public void Attack(AtkData atkData, ActorBase target)
        {
            string[] dataArray = atkData.dmgData.Split('-');
            int d1 = int.Parse(dataArray[0]);
            int d2 = int.Parse(dataArray[1]);
            int d3 = int.Parse(dataArray[2]);
            int d4 = int.Parse(dataArray[3]);

            Random rand = new Random();

            if (rand.Next(0, 20) + d4 >= target.ac)
            {
                int dmg = 0;
                for (int d = 0; d < d1; d++)
                {
                    dmg += rand.Next(1, d2);
                }
                dmg += d3;

                target.OnHit(dmg);

                Log.AddToStoredLog(name + " hit " + target.name + " for " + dmg + " damage!");
            }
            else Log.AddToStoredLog(name + " missed...");

            EndTurn();
        }
        public void OnHit(int dmg)
        {
            hp -= dmg;
            if (hp <= 0) Death();
        }
        public abstract void Death();
        public abstract void StartTurn();
        public abstract void EndTurn();
    }
    public class AtkData
    {
        public string dmgData { get; set; }
        public AtkData(string _dmgData = "0-0-0-0") { dmgData = _dmgData; }
    }
}
