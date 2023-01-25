using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    class UpdateCameraOnMove: OnMove
    {
        public override void Move(Vector2 initialPosition, Vector2 finalPosition) { Renderer.MoveCamera(entity.GetComponent<Vector2>()); }
        public UpdateCameraOnMove() { }
    }
}
