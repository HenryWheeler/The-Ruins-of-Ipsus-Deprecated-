﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Inventory: Component
    {
        public List<Entity> inventory = new List<Entity>();
        public Inventory() { }
    }
}
