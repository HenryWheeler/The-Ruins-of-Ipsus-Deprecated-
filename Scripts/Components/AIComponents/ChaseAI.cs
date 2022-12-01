using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class ChaseAI : AI
    {
        public override void ExecuteAction()
        {
            switch (mood)
            {
                case "Uncertain": entity.GetComponent<Movement>().Move(CMath.random.Next(-1, 2), CMath.random.Next(-1, 2)); break;
                case "Red*Angry": { HuntAndAttack(); break; }
                case "Fearful": entity.GetComponent<TurnFunction>().EndTurn(); break;
            }
        }
        public ChaseAI(int _maxMemory) { maxMemory = _maxMemory; }
        public ChaseAI() { }
    }
}
