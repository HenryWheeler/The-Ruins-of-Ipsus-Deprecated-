using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class AnimationFunction: Component
    {
        public Draw[] frame;
        public int frameSelection = 0;
        public void ProgressFrame() 
        {
            entity.GetComponent<Draw>().fColor = frame[frameSelection].fColor;
            entity.GetComponent<Draw>().bColor = frame[frameSelection].bColor;
            entity.GetComponent<Draw>().character = frame[frameSelection].character;
            if (frameSelection == frame.Length - 1) { frameSelection = 0; }
            else { frameSelection++; }
        }
        public AnimationFunction(Draw[] _frame) { frame = new Draw[_frame.Length]; frame = _frame; }
        public AnimationFunction() { }
    }
}
