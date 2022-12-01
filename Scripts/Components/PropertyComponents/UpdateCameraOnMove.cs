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
        public override void OnMove(int x1, int y1, int x2, int y2) { Renderer.MoveCamera(entity.GetComponent<Coordinate>()); }
        public UpdateCameraOnMove() { special = true; }
    }
}
