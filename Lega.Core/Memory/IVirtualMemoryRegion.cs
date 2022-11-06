using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
    public interface IVirtualMemoryRegion : IVirtualMemory
    {
        public int Offset { get; }

        public void Map(VirtualMemory memory, int offset, int bytes);
    }
}
