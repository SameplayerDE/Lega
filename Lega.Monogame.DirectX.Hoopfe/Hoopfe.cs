using Lega.Core.Memory;
using System;

namespace Lega.Monogame.DirectX.Hoopfe
{
    public sealed class VirtualSystem
    {
        private VirtualSystem()
        {
        }

        public static VirtualSystem Instance { get { return Nested.instance; } }

        private class Nested : IVirtualMemory
        {

            static Nested()
            {
            }

            internal static readonly VirtualSystem instance = new VirtualSystem();

            public void Poke(int address, byte value)
            {
                throw new NotImplementedException();
            }

            public void Poke(int address, params byte[] value)
            {
                throw new NotImplementedException();
            }

            public byte Peek(int address)
            {
                throw new NotImplementedException();
            }

            public ReadOnlySpan<byte> Peek(int address, int bytes)
            {
                throw new NotImplementedException();
            }
        }
    }
}
