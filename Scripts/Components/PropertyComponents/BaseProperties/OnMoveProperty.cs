using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public abstract class OnMoveProperty: Component
    {
        public abstract void OnMove(int x1, int y1, int x2, int y2);
        public OnMoveProperty() { }
    }
}
