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
            address += _offset;
            if (address < _offset || _offset + _bytes <= address)
            {
                throw new Exception();
            }
            _memory.Poke(address, value);
		}

		public void Poke(int address, int length, byte value)
		{
            address += _offset;
            if (address < _offset || _offset + _bytes <= address)
            {
                throw new Exception();
            }
            _memory.Poke(address, length, value);
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
            address += _offset;
            if (address < _offset || _offset + _bytes <= address)
			{
				throw new Exception();
			}
			return _memory.Peek(address);
		}

        public ReadOnlySpan<byte> Peek(int address, int bytes)
        {
            address += _offset;
            if (address < _offset || _offset + _bytes <= address)
            {
                throw new Exception();
            }
            if (address + bytes < _offset || _offset + _bytes <= address + bytes)
            {
                throw new Exception();
            }
            return _memory.Peek(address, bytes);
        }
    }
}
