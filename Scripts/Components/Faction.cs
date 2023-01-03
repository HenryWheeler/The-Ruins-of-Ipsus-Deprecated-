﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Faction: Component
    {
        public string faction { get; set; }
        public Faction(string _faction) 
        { 
            faction = _faction; 
        }
        public Faction() { }
    }
}
