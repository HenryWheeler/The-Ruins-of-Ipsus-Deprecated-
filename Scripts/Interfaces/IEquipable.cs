using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public interface IEquipable
    {
        void Equip(ActorBase actor);
        void Unequip(ActorBase actor);
    }
}
