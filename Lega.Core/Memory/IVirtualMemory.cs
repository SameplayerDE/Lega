using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
	public interface IVirtualMemory
	{

        public int Bytes
        {
            get;
        }

        public void Poke(int address, byte value);

        public void Poke(int address, params byte[] value);

        public void Poke(int address, ReadOnlySpan<byte> value);

        public void Poke2(int address, ushort value);

        public void Poke4(int address, uint value);

        public byte Peek(int address);

        //public ReadOnlySpan<byte> Peek2(int address, int bytes);

        //public ReadOnlySpan<byte> Peek4(int address, int bytes);

        public ReadOnlySpan<byte> Peek(int address, int bytes);

        public bool Contains(int address);
        public bool Contains(int address, int bytes);
    }
}
