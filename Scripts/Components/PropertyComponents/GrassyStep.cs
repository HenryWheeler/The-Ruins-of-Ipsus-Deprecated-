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
        public override void OnMove(int x1, int y1, int x2, int y2)
        {
            if (CMath.random.Next(0, 100) > 50) { Map.map[x2, y2] = new Tile(x2, y2, '`', "Grass", "Soft Green Grass.", "Light_Green", "Black", false, 1); }
            else { { Map.map[x2, y2] = new Tile(x2, y2, '"', "Grass", "Soft Green Grass.", "Green", "Black", false, 1); } }
        }
        public GrassyStep() { special = true; }
    }
}
