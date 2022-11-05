using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRuinsOfIpsus
{
    [Serializable]
    public class Memory: Component
    {
        public int memory { get; set; }
        public int memoryMax { get; set; }
        public Entity memorizedEntity { get; set; }
        public bool TickMemory()
        {
            if (memory > 0) { memory--; return true; }
            else { return false; }
        }
        public void SetMemoryMax() { memory = memoryMax; }
        public Memory(int _memoryMax, Entity entity = null) { memory = 0; memoryMax = _memoryMax; memorizedEntity = entity; }
        public Memory() { }
    }
}
