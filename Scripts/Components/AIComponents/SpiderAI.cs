﻿using System;
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
            switch (action)
            {
                case -1:
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
                        break;
                    }
                case 0: 
                    {
                        Coordinate coordinate = entity.GetComponent<Coordinate>();
                        int x = CMath.random.Next(-1, 2);
                        int y = CMath.random.Next(-1, 2);
                        if (Map.map[coordinate.x + x, coordinate.y + y].terrain != null && Map.map[coordinate.x + x, coordinate.y + y].terrain.GetComponent<Description>().name != "Web")
                        { entity.GetComponent<Movement>().Move(x, y); }
                        else if (CMath.random.Next(1, 100) > 90) { entity.GetComponent<Movement>().Move(x, y); }
                        else { entity.GetComponent<TurnFunction>().EndTurn(); }
                        break; 
                    }
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
            action = 1;
            target = attacker; memory = maxMemory;
        }
        public SpiderAI(int _maxMemory) { maxMemory = _maxMemory; }
        public SpiderAI() { }
    }
}