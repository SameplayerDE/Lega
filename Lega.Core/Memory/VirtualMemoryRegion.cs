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

        public VirtualMemoryRegion() {}

        public VirtualMemoryRegion(VirtualMemory memory, int offset, int bytes)
		{
			_memory = memory;
			_offset = offset;
			_bytes = bytes;
		}

        public virtual void Map(VirtualMemory memory, int offset, int bytes)
        {
            if (offset + bytes > memory.Capacity)
            {
                throw new ArgumentOutOfRangeException("mapped outside of virtual memory bounds");
            }
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

        public void Poke(int adress, params byte[] data)
        {
            int offset = 0;
            while (offset < data.Length)
            {
                Poke(adress + offset, data[offset]);
                offset++;
            }
        }

        public byte Peek(int address)
		{
			return _memory.Peek(address + _offset);
		}

        public ReadOnlySpan<byte> Peek(int address, int bytes)
        {
            return _memory.Peek(address + _offset, bytes);
        }
    }
}
