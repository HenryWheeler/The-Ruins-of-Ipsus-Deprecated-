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
        public abstract void OnMove(Vector3 initialPosition, Vector3 finalPosition);
        public OnMoveProperty() { }
    }
}
