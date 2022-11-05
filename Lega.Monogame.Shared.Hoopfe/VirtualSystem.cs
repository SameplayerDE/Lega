using Lega.Core.Memory;
using System;

namespace Lega.Monogame.Shared.Hoopfe
{
    public sealed class VirtualSystem
    {

        private VirtualMemory _systemMemory;
        public int Capacity => _systemMemory.Capacity;

        private VirtualSystem()
        {
            _systemMemory = new VirtualMemory(4_096);
            int i = 0;
            while (i < 4_096)
            {
                Poke(i, 0x00);
                i++;
            }
            Poke(0x00, 0b11001001);
        }

        public static VirtualSystem Instance { get { return Nested.instance; } }

        private class Nested
        {

            static Nested()
            {
            }

            internal static readonly VirtualSystem instance = new VirtualSystem();

        }

        public void Poke(int address, byte value)
        {
            _systemMemory.Poke(address, value);
        }

        public void Poke(int address, params byte[] value)
        {
            _systemMemory.Poke(address, value);
        }

        public byte Peek(int address)
        {
            return _systemMemory.Peek(address);
        }

        public ReadOnlySpan<byte> Peek(int address, int bytes)
        {
            return _systemMemory.Peek(address, bytes);
        }

    }
}
