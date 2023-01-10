using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class PatrolFunction: OnTurnProperty
    {
        public Vector2 lastPosition { get; set; }
        public int patrolRoute { get; set; }
        public int lastPastInformation = 20;
        public override void OnTurn()
        {
            //string color = "";
            //char character = 'x';
            //switch (patrolRoute)
            //{
            //    case 0: color = "Green"; character = '0'; break;
            //    case 1: color = "Yellow"; character = '1'; break;
            //    case 2: color = "Red"; character = '2'; break;
            //    case 3: color = "Orange"; character = '3'; break;
            //    case 4: color = "Blue"; character = '4'; break;
            //    case 5: color = "Purple"; character = '5'; break;
            //    case 6: color = "Gray"; character = '6'; break;
            //    case 7: color = "White"; character = '7'; break;
            //    case 8: color = "Brown"; character = '8'; break;
            //    case 9: color = "Cyan"; character = '9'; break;
            //}
            //Vector2 vector2 = entity.GetComponent<Coordinate>().vector2;
            //World.tiles[vector2.x, vector2.y].GetComponent<Draw>().fColor = color;
            //World.tiles[vector2.x, vector2.y].GetComponent<Draw>().character = character;
        }
        public PatrolFunction() 
        {
            start = false;
            lastPosition = new Vector2(0, 0);
        }
    }
}
