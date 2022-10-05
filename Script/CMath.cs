using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeTest
{
    public static class CMath
    {
        public static int Distance(int oX, int oY, int eX, int eY) { return ((oX - eX) * (oX - eX)) + ((oY - eY) * (oY - eY)); }
    }
}
