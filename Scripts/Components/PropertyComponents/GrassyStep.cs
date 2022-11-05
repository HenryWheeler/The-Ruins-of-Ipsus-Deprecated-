using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class GrassyStep: OnMoveProperty
    {
        public override void OnMove(int x, int y)
        {
            if (CMath.random.Next(0, 100) > 50) { Map.map[x, y] = new Tile(x, y, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
            else { { Map.map[x, y] = new Tile(x, y, '"', "Grass", "Soft Green Grass.", "Green", "Black", false, 1); } }
        }
        public GrassyStep() { property = true; }
    }
}
