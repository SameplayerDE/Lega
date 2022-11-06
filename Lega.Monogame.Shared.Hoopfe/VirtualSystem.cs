using Lega.Core.Memory;
using System;

namespace Lega.Monogame.Shared.Hoopfe
{
    public sealed class VirtualSystem
    {

        private VirtualMemory _systemMemory;
        private VirtualMemoryRegion _systemData;
        private VirtualMemoryRegion _systemDisplayData;
        public int Capacity => _systemMemory.Bytes;
        public event EventHandler MemoryChange;

        private VirtualSystem()
        {
            _systemMemory = new VirtualMemory(8_192);
            _systemData = new VirtualMemoryRegion(_systemMemory, 0x00, 1_024);
            _systemDisplayData = new VirtualMemoryRegion(_systemMemory, 0x400, 4_096);

            int i = 0;
            while (i < 4_096)
            {
                _systemDisplayData.Poke(i, 0x00);
                i++;
            }
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
            MemoryChange?.Invoke(null, EventArgs.Empty);
        }

        /*public void Poke(int address, params byte[] value)
        {
            _systemMemory.Poke(address, value);
            MemoryChange?.Invoke(null, EventArgs.Empty);
        }*/

        public void Poke2(int address, ushort value)
        {
            byte upper = (byte)(value >> 8);
            byte lower = (byte)(value & 0x00FF);

            _systemMemory.Poke(address + 0, upper);
            _systemMemory.Poke(address + 1, lower);

            MemoryChange?.Invoke(null, EventArgs.Empty);
        }

        public void Poke4(int address, uint value)
        {
            ushort upper = (ushort)(value >> 16);
            ushort lower = (ushort)(value & 0x0000FFFF);

            _systemMemory.Poke(address + 0, (byte)(upper >> 8));
            _systemMemory.Poke(address + 1, (byte)(upper & 0x00FF));
            _systemMemory.Poke(address + 2, (byte)(lower >> 8));
            _systemMemory.Poke(address + 3, (byte)(lower & 0x00FF));

            MemoryChange?.Invoke(null, EventArgs.Empty);
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
