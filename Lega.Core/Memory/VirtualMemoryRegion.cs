using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lega.Core.Memory
{
    public class VirtualMemoryRegion
    {
        private VirtualMemory _memory;
        private int _offset = 0;
        private int _bytes = 1;

        /// <summary>
        /// 
        /// </summary>
        public int Offset => _offset;

        /// <summary>
        /// 
        /// </summary>
        public int Bytes => _bytes;

        /// <summary>
        /// span of data
        /// </summary>
        public ReadOnlySpan<byte> Data => _memory.Peek(Offset, Bytes);

        public VirtualMemoryRegion(VirtualMemory memory, int offset, int bytes)
        {
            _memory = memory;
            _offset = offset;
            _bytes = bytes;
        }

        public void Poke(int address, byte value)
        {
            _memory.Poke(address + _offset, value);
        }

        public void Poke(int address, int length, byte value)
        {
            _memory.Poke(address + _offset, length, value);
        }

        public byte Peek(int address)
        {
            return _memory.Peek(address + _offset);
        }
    }
}
