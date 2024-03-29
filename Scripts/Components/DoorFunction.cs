﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class DoorFunction: OnMove
    {
        public bool open = false;

        public override void Move(Vector2 initialPosition, Vector2 finalPosition)
        {
            if (!open)
            {
                Open();
            }
        }
        public void Open()
        {
            open = true;
            entity.GetComponent<Draw>().character = '-';
            entity.GetComponent<Visibility>().opaque = false;
        }
        public void Close()
        {
            open = false;
            entity.GetComponent<Draw>().character = '+';
            entity.GetComponent<Visibility>().opaque = true;
        }
        public DoorFunction() { }
    }
}
