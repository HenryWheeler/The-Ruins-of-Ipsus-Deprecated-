using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class SpiderAI : AI
    {
        public override void ExecuteAction()
        {
            switch (mood)
            {
                case "Uncertain": 
                    {
                        if (CMath.random.Next(1, 100) > 50)
                        {
                            Coordinate coordinate = entity.GetComponent<Coordinate>();
                            int x = CMath.random.Next(-1, 2);
                            int y = CMath.random.Next(-1, 2);
                            if (Map.map[coordinate.x + x, coordinate.y + y].terrain != null && Map.map[coordinate.x + x, coordinate.y + y].terrain.GetComponent<Description>().name != "Web")
                            { entity.GetComponent<Movement>().Move(x, y); }
                            else if (CMath.random.Next(1, 100) > 90) { entity.GetComponent<Movement>().Move(x, y); }
                            else { entity.GetComponent<TurnFunction>().EndTurn(); }
                        }
                        else
                        {
                            if (CMath.random.Next(1, 100) > 90)
                            {
                                Coordinate coordinate = entity.GetComponent<Coordinate>();
                                if (Map.map[coordinate.x, coordinate.y].terrain == null)
                                {
                                    Entity webEntity = new Entity();
                                    webEntity.AddComponent(new Coordinate(coordinate.x, coordinate.y));
                                    webEntity.AddComponent(new Description("Web", "A sticky white web"));
                                    webEntity.AddComponent(new Draw("White", "Black", (char)15));
                                    webEntity.AddComponent(new Visibility(false, false, false));
                                    webEntity.AddComponent(new RestrainOnMove());
                                    Map.map[coordinate.x, coordinate.y].terrain = webEntity;
                                }
                                entity.GetComponent<TurnFunction>().EndTurn();
                            }
                            else { entity.GetComponent<TurnFunction>().EndTurn(); }
                        }
                        break; 
                    }
                case "Angry":
                    {
                        Coordinate coordinate = entity.GetComponent<Coordinate>();
                        Coordinate targetCoordinate = DijkstraMaps.PathFromMap(entity, target);
                        if (Map.map[coordinate.x + targetCoordinate.x, coordinate.y + targetCoordinate.y].actor != null && 
                            Map.map[coordinate.x + targetCoordinate.x, coordinate.y + targetCoordinate.y].actor != entity)
                        { AttackManager.MeleeAllStrike(entity, Map.map[coordinate.x, coordinate.y].actor); }
                        else { entity.GetComponent<Movement>().Move(targetCoordinate.x, targetCoordinate.y); }
                        break;
                    }
                case "Fearful": entity.GetComponent<TurnFunction>().EndTurn(); break;
            }
        }
        public SpiderAI(int _maxMemory) { maxMemory = _maxMemory; }
        public SpiderAI() { }
    }
}
