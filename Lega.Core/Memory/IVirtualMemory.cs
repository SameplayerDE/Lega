using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Memory
{
	public interface IVirtualMemory
	{

		public void Poke(int address, byte value);

		public void Poke(int address, params byte[] value);

		//public void Poke(int address, float value);

		public byte Peek(int address);

		public ReadOnlySpan<byte> Peek(int address, int bytes);

		//public float PeekFloat(int address, int bytes);

	}
}
