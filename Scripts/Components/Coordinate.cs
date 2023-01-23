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
        public Vector2 vector2 { get; set; }
        public void CombineVectors(Vector2 vector2) 
        { 
            this.vector2.x += vector2.x; 
            this.vector2.y += vector2.y; 
        }
        public Coordinate(int x, int y) 
        {
            vector2 = new Vector2(x, y);
        }
        public Coordinate(Vector2 _vector2) 
        { 
            vector2 = _vector2;
        }
        public Coordinate() { }
    }
    [Serializable]
    public class Vector2 : Component
    {
        public int x { get; set; }
        public int y { get; set; }
        public Vector2(int _x, int _y) 
        { 
            x = _x; 
            y = _y; 
        }
        public Vector2(Vector2 _1, Vector2 _2)
        {
            x = _1.x + _2.x;
            y = _1.y + _2.y;
        }
        public override int GetHashCode()
        {
            return 17 + 31 * x.GetHashCode() + 31 * y.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Vector2 other = obj as Vector2;
            return other != null && x == other.x && y == other.y;
        }
        public Vector2() { }
    }
}
