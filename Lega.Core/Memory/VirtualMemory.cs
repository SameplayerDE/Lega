namespace Lega.Core.Memory
{
	/// <summary>
	/// represents memory for the virtual system
	/// </summary>
	public class VirtualMemory : IVirtualMemory
	{
        /*
		/// <summary>
		/// data used by the virtual memory
		/// </summary>
		private byte[] _data;

		/// <summary>
		/// number of bytes the memory can hold
		/// </summary>
		private int _capacity;

		/// <summary>
		/// number of bytes the memory can hold
		/// </summary>
		public int Capacity => _capacity;

		/// <summary>
		/// span of data
		/// </summary>
		public ReadOnlySpan<byte> Data => Peek(0x00, Capacity);

		/// <summary>
		/// allocates the space used by the memory
		/// </summary>
		/// <param name="capacity">number of bytes saved into memory</param>
		public VirtualMemory(int capacity)
		{
			_capacity = capacity;
			_data = new byte[capacity];
		}
		
		/// <summary>
		/// write value in memory
		/// </summary>
		/// <param name="address">start address in memory</param>
		/// <param name="value">value to poke</param>
		public void Poke(int address, byte value)
		{
			_data[address] = value;
		}

		/// <summary>
		/// write values in memory
		/// </summary>
		/// <param name="address">start address in memory</param>
		/// <param name="length">number of bytes to poke</param>
		/// <param name="value">value to poke</param>
		public void Poke(int address, int length, byte value)
		{
			_data.AsSpan().Slice(address, length).Fill(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="adress"></param>
		/// <param name="data"></param>
		public void Poke(int adress, params byte[] data)
		{
			int offset = 0;
			while (offset < data.Length)
			{
				Poke(adress + offset, data[offset]);
				offset++;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <param name="value"></param>
		public void Poke(int address, float value)
		{
			Poke(address, BitConverter.GetBytes(value));
		}

        //public void Poke(int address, int value)
        //{
        //    Poke(address, BitConverter.GetBytes(value));
        //}

        /// <summary>
        /// reads value in memory
        /// </summary>
        /// <param name="address">start address in memory</param>
        /// <returns>byte at address</returns>
        public byte Peek(int address)
		{
			return _data[address];
		}

		/// <summary>
		/// reads values in memory
		/// </summary>
		/// <param name="address">start address in memory</param>
		/// <param name="length">number of bytes to peek</param>
		/// <returns>Span of the requested area</returns>
		public ReadOnlySpan<byte> Peek(int address, int length)
		{
			return (ReadOnlySpan<byte>)_data.AsSpan().Slice(address, length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public Memory<byte> PeekMemory(int address, int length)
		{
			return _data.AsMemory().Slice(address, length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public Span<byte> PeekSpan(int address, int length)
		{
			return _data.AsSpan().Slice(address, length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public ReadOnlyMemory<byte> PeekReadOnlyMemory(int address, int length)
		{
			return (ReadOnlyMemory<byte>)_data.AsMemory().Slice(address, length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public float PeekSingle(int address)
		{
			return BitConverter.ToSingle(Peek(address, 4));
		}
		*/

        private byte[] _data;
        private int _bytes;

        public int Bytes => _bytes;

        public VirtualMemory(int bytes)
        {
            _bytes = bytes;
            _data = new byte[bytes];
        }

        public bool Contains(int address)
        {
            return address >= 0 && address < _bytes;
        }

        public bool Contains(int address, int bytes)
        {
			bytes -= 1;
            return Contains(address) && address + bytes >= 0 && address + bytes < _bytes;
        }

        public byte Peek(int address)
		{
			if (!Contains(address))
			{
				throw new Exception();
			}
			return _data[address];
		}


        public ReadOnlySpan<byte> Peek(int address, int bytes)
        {
            if (!Contains(address, bytes))
            {
                throw new Exception();
            }
            return (ReadOnlySpan<byte>)_data.AsSpan().Slice(address, bytes);
        }

        public void Poke(int address, byte value)
		{
            if (!Contains(address))
            {
                throw new Exception();
            }
            _data[address] = value;
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

        public void Poke2(int address, ushort value)
		{
            if (!Contains(address, 2))
            {
                throw new Exception();
            }
			_data[address + 0] = (byte)(value >> 8);
			_data[address + 1] = (byte)(value & 0x00FF);
        }

		public void Poke4(int address, uint value)
		{
            if (!Contains(address, 4))
            {
                throw new Exception();
            }
            _data[address + 0] = (byte)(value >> 24);
            _data[address + 1] = (byte)(value >> 16 & 0x00FF);
            _data[address + 2] = (byte)(value & 0x0000FF00 >> 8);
            _data[address + 3] = (byte)(value & 0x000000FF);
        }
    }
}
