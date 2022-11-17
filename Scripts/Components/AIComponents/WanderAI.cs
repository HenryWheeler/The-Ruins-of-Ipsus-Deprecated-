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
        public override void ExecuteAction()
        {
            switch (action)
            {
                case -1: entity.GetComponent<Movement>().Move(CMath.random.Next(-1, 2), CMath.random.Next(-1, 2)); break;
                case 0: entity.GetComponent<Movement>().Move(CMath.random.Next(-1, 2), CMath.random.Next(-1, 2)); break;
                case 1:
                    {
                        if (DijkstraMaps.maps.ContainsKey(target.GetComponent<Description>().name + target.tempID))
                        {
                            Coordinate coordinate = DijkstraMaps.PathFromMap(entity.GetComponent<Coordinate>(), target.GetComponent<Description>().name + target.tempID);
                            entity.GetComponent<Movement>().Move(coordinate.x, coordinate.y);
                        }
                        else
                        {
                            target.AddComponent(new DijkstraProperty(5));
                            DijkstraMaps.CreateMap(entity.GetComponent<Coordinate>(), target.GetComponent<Description>().name + target.tempID);
                            Coordinate coordinate = DijkstraMaps.PathFromMap(entity.GetComponent<Coordinate>(), target.GetComponent<Description>().name + target.tempID);
                            entity.GetComponent<Movement>().Move(coordinate.x, coordinate.y);
                        }
                        break;
                    }
                case 2: entity.GetComponent<TurnFunction>().EndTurn(); break;
            }
        }
        public override void OnHit(Entity attacker)
        {

        }
        public WanderAI() { }
    }
}
