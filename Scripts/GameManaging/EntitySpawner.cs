using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public class EntitySpawner
    {
        public static Entity CreateEntity(int x, int y, int uID, int type)
        {
            Entity entity = JsonDataManager.ReturnEntity(uID, type);
            entity.GetComponent<Coordinate>().x = x;
            entity.GetComponent<Coordinate>().y = y;
            switch (type)
            {
                case 0: Map.map[x, y].actor = entity; break;
                case 1: Map.map[x, y].item = entity; break;
            }
            return entity;
        }
    }
}
