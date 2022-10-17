using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    public interface IDescription
    {
        string spacer { get; set; }
        string description { get; set; }
        string Describe();
    }
}
