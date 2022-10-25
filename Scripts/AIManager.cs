using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class AIManager
    {
        public static Dictionary<string, AI> aiDictionary = new Dictionary<string, AI>();
        public AIManager()
        {
            aiDictionary.Add("Chase_AI", new ChaseAI());
            aiDictionary.Add("Wander_AI", new WanderAI());
        }
    }
    [Serializable]
    public abstract class AI
    {
        public int action;
        public abstract void Action(Monster actor);
        public abstract void OnHit(Monster actor, ActorBase attacker);
        public Node Path(Monster actor, string map) { return DijkstraMaps.PathFromMap(actor.x, actor.y, map); }
        public AI() { }
    }
    [Serializable]
    public class ChaseAI : AI
    {
        public ActorBase target;
        public override void Action(Monster actor)
        {
            target = Program.player;
            if (target != null)
            {
                if (CMath.Sight(actor.x, actor.y, target.x, target.y, actor.sight)) { actor.memory = actor.maxMemory; }
                else { if (actor.memory > 0) { actor.memory--; } }
                if (actor.memory > 0)
                {
                    Node targetNode = Path(actor, target.name);
                    if (targetNode != null)
                    {
                        if (CMath.Distance(target.x, target.y, targetNode.x, targetNode.y) == 0)
                        {
                            actor.Attack(target, actor, 0);
                        }
                        else actor.Move(targetNode.x, targetNode.y);
                    }
                    else actor.EndTurn();
                }
                else actor.EndTurn();
            }
            else actor.EndTurn();
        }
        public override void OnHit(Monster actor, ActorBase attacker) { target = attacker; }
        public ChaseAI() { }
    }
    [Serializable]
    public class WanderAI : AI
    {
        public ActorBase target;
        public override void Action(Monster actor)
        {
            actor.Move(CMath.random.Next(-1, 2) + actor.x, CMath.random.Next(-1, 2) + actor.y);
        }
        public override void OnHit(Monster actor, ActorBase attacker) { target = attacker; }
        public WanderAI() { }
    }
}
