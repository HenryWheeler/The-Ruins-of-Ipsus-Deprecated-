using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Coordinate: Component
    {
        public Vector3 vector3 { get; set; }
        public void CombineVectors(Vector3 vector3) 
        { 
            this.vector3.x += vector3.x; 
            this.vector3.y += vector3.y; 
            this.vector3.z += vector3.z; 
        }
        public Coordinate(int x, int y, int z) { vector3 = new Vector3(x, y, z); }
        public Coordinate(Vector3 _vector3) { vector3 = _vector3; }
        public Coordinate() { }
    }
    [Serializable]
    public class Vector3
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public Vector3(int _x, int _y, int _z) { x = _x; y = _y; z = _z; }
        public Vector3() { }
    }
}
