using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    class UpdateCameraOnMove: OnMoveProperty
    {
        public override void OnMove(Vector2 initialPosition, Vector2 finalPosition) { Renderer.MoveCamera(entity.GetComponent<Coordinate>().vector2); }
        public UpdateCameraOnMove() { }
    }
}
