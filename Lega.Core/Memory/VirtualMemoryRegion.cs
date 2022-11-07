namespace Lega.Core.Memory
{
    public class VirtualMemoryRegion : IVirtualMemoryRegion
    {
        /*
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
        */

        private VirtualMemory _memory;
        private int _offset = 0;
        private int _bytes = 1;

        public int Bytes => _bytes;

        public int Offset => _offset;

        public VirtualMemoryRegion() { }

        public VirtualMemoryRegion(VirtualMemory memory, int offset, int bytes)
        {
            Map(memory, offset, bytes);
        }

        public bool Contains(int address)
        {
            return address >= _offset && address < _offset + _bytes;
        }

        public bool Contains(int address, int bytes)
        {
            bytes -= 1;
            return Contains(address) && address + bytes >= _offset && address + bytes < _offset + _bytes;
        }

        public byte Peek(int address)
        {
            address += _offset;
            if (!Contains(address))
            {
                throw new Exception();
            }
            return _memory.Peek(address);
        }

        public ReadOnlySpan<byte> Peek(int address, int bytes)
        {
            address += _offset;
            if (!Contains(address, bytes))
            {
                throw new Exception();
            }
            return _memory.Peek(address, bytes);
        }

        public void Poke(int address, byte value)
        {
            address += _offset;
            if (!Contains(address))
            {
                throw new Exception();
            }
            _memory.Poke(address, value);
        }

        public void Poke(int adress, params byte[] value)
        {
            int offset = 0;
            while (offset < value.Length)
            {
                Poke(adress + offset, value[offset]);
                offset++;
            }
        }

        public void Poke(int adress, ReadOnlySpan<byte> value)
        {
            int offset = 0;
            while (offset < value.Length)
            {
                Poke(adress + offset, value[offset]);
                offset++;
            }
        }

        public void Poke2(int address, ushort value)
        {
            address += _offset;
            if (!Contains(address))
            {
                throw new Exception();
            }
            _memory.Poke2(address, value);
        }

        public void Poke4(int address, uint value)
        {
            address += _offset;
            if (!Contains(address))
            {
                throw new Exception();
            }
            _memory.Poke4(address, value);
        }

        public void Map(VirtualMemory memory, int offset, int bytes)
        {
            if (!memory.Contains(offset, bytes))
            {
                throw new Exception();
            }
            _memory = memory;
            _offset = offset;
            _bytes = bytes;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Clear(int address)
        {
            throw new NotImplementedException();
        }

        public void Clear(int address, int bytes)
        {
            throw new NotImplementedException();
        }
    }
}
