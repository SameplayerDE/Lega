using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
    public abstract class VirtualComponent : IVirtualComponent
    {
        private VirtualMemory _memory;
        public VirtualMemory Memory => _memory;

        public ReadOnlyMemory<byte> MappedMemory => _memory.PeekReadOnlyMemory(_offset, _length);

        private int _offset = 0;
        public int MemoryOffset => _offset;

        private int _length = 1;
        public int MemoryLength => _length;

        public void Map(VirtualMemory memory, int offset, int length)
        {
            _memory = memory;
            _offset = offset;
            _length = length;
        }
    }
}
