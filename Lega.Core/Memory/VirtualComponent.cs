using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
    public abstract class VirtualComponent : IVirtualComponent
    {
        private VirtualMemoryRegion _memoryRegion;
        public VirtualMemoryRegion MemoryRegion => _memoryRegion;

        public int MemoryOffset => _memoryRegion.Offset;
        public int MemoryLength => _memoryRegion.Bytes;

        public virtual void Map(VirtualMemory memory, int offset, int bytes)
        {
            if (offset + bytes > memory.Bytes)
            {
                throw new ArgumentOutOfRangeException("mapped outside of virtual memory bounds");
            }
            _memoryRegion = new VirtualMemoryRegion(memory, offset, bytes);
        }
    }
}
