using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
    public interface IVirtualComponent
    {
        public VirtualMemory Memory
        {
            get;
        }

        public ReadOnlyMemory<byte> MappedMemory
        {
            get;
        }

        public int MemoryOffset
        {
            get;
        }

        public int MemoryLength
        {
            get;
        }

        public void Map(VirtualMemory memory, int offset, int length);

    }
}
