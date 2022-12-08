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
                            if (CMath.CheckBounds(coordinate.vector3.x + x, coordinate.vector3.y + y) && Map.map[coordinate.vector3.x + x, coordinate.vector3.y + y].terrain != null && Map.map[coordinate.x + x, coordinate.y + y].terrain.GetComponent<Description>().name != "Web")
                            { entity.GetComponent<Movement>().Move(x, y); }
                            else if (CMath.random.Next(1, 100) > 90) { entity.GetComponent<Movement>().Move(x, y); }
                            else { entity.GetComponent<TurnFunction>().EndTurn(); }
                        }
                        else
                        {
                            if (CMath.random.Next(1, 100) > 90)
                            {
                                Coordinate coordinate = entity.GetComponent<Coordinate>();
                                if (Map.map[coordinate.vector3.x, coordinate.vector3.y].terrain == null)
                                {
                                    Entity webEntity = new Entity();
                                    webEntity.AddComponent(new Coordinate(coordinate.vector3));
                                    webEntity.AddComponent(new Description("Web", "A sticky white web"));
                                    webEntity.AddComponent(new Draw("White", "Black", (char)15));
                                    webEntity.AddComponent(new Visibility(false, false, false));
                                    webEntity.AddComponent(new RestrainOnMove());
                                    //Map.map[coordinate.x, coordinate.y].terrain = webEntity;
                                }
                                entity.GetComponent<TurnFunction>().EndTurn();
                            }
                            else { entity.GetComponent<TurnFunction>().EndTurn(); }
                        }
                        break; 
                    }
                case "Red*Angry": { HuntAndAttack(); break; }
                case "Fearful": entity.GetComponent<TurnFunction>().EndTurn(); break;
            }
        }
        public SpiderAI(int _maxMemory) { maxMemory = _maxMemory; }
        public SpiderAI() { }
    }
}
