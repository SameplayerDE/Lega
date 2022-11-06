using Lega.Core.Memory;
using Lega.Core.Monogame;
using Lega.Core.Monogame.Input;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Net;

namespace Lega.Monogame.Shared.Hoopfe
{
    public sealed class VirtualSystem : IVirtualMemory, IUpdateable
    {

        private VirtualMemory _systemMemory;
        private VirtualMemoryRegion _systemData;
        private VirtualMemoryRegion _systemDisplayData;

        //private VirtualKeyboard _keyboard;
        public VirtualMouse _mouse;

        private const int _bytes = 8_192;

        public int Bytes => _bytes;
        public event EventHandler MemoryChange;

        private VirtualSystem()
        {
            _systemMemory = new VirtualMemory(_bytes);
            _systemData = new VirtualMemoryRegion(_systemMemory, 0x00, 1_024);
            _systemDisplayData = new VirtualMemoryRegion(_systemMemory, 0x400, 4_096);

            //_keyboard = new VirtualKeyboard();
            _mouse = new VirtualMouse();

            int i = 0;
            while (i < _systemDisplayData.Bytes)
            {
                _systemDisplayData.Poke(i, 0x00);
                i++;
            }

            _systemData.Poke2(0x0000, 0b_10_10_00_00_00_00_00_00);
            _systemData.Poke2(0x0002, 0b_10_11_10_00_00_00_00_00);
            _systemData.Poke2(0x0004, 0b_10_11_11_10_00_00_00_00);
            _systemData.Poke2(0x0006, 0b_10_11_11_11_10_00_00_00);
            _systemData.Poke2(0x0008, 0b_10_11_11_11_11_10_00_00);
            _systemData.Poke2(0x000A, 0b_10_11_11_11_11_11_10_00);
            _systemData.Poke2(0x000C, 0b_10_11_11_11_10_10_00_00);
            _systemData.Poke2(0x000E, 0b_00_10_10_11_11_10_00_00);
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

        public void Poke(int address, params byte[] value)
        {
            _systemMemory.Poke(address, value);
            MemoryChange?.Invoke(null, EventArgs.Empty);
        }

        public void Poke(int address, ReadOnlySpan<byte> value)
        {
            _systemMemory.Poke(address, value);
            MemoryChange?.Invoke(null, EventArgs.Empty);
        }

        public void Poke2(int address, ushort value)
        {
            /*
            byte upper = (byte)(value >> 8);
            byte lower = (byte)(value & 0x00FF);

            _systemMemory.Poke(address + 0, upper);
            _systemMemory.Poke(address + 1, lower);
            */

            _systemMemory.Poke2(address, value);
            MemoryChange?.Invoke(null, EventArgs.Empty);
        }

        public void Poke4(int address, uint value)
        {
            /*
            ushort upper = (ushort)(value >> 16);
            ushort lower = (ushort)(value & 0x0000FFFF);

            _systemMemory.Poke(address + 0, (byte)(upper >> 8));
            _systemMemory.Poke(address + 1, (byte)(upper & 0x00FF));
            _systemMemory.Poke(address + 2, (byte)(lower >> 8));
            _systemMemory.Poke(address + 3, (byte)(lower & 0x00FF));
            */

            _systemMemory.Poke4(address, value);
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

        public bool Contains(int address)
        {
            return address >= 0 && address < _bytes;
        }

        public bool Contains(int address, int bytes)
        {
            bytes -= 1;
            return Contains(address) && address + bytes >= 0 && address + bytes < _bytes;
        }

        public void SetPixel(int x, int y, byte id, bool transparent = false)
        {
            
            if (x < 0 || y < 0 || x >= 128 || y >= 128)
            {
                return;
            }

            //maps x and y to the address
            var index = y * 32 + (x / 4);
            //0 if it is a most sig byte 1 if it is least sig byte
            var region = x % 4;
            //color id 
            var color = (byte)(id % 4);
            //what is currently saved at this address
            var takenBy = _systemDisplayData.Peek(index);

            byte aa = (byte)(takenBy >> 6);
            byte ab = (byte)(takenBy >> 4 & 0x03);
            byte ba = (byte)(takenBy >> 2 & 0x03);
            byte bb = (byte)(takenBy >> 0 & 0x03);

            if (color == 0 && transparent)
            {
                return;
            }

            switch (region)
            {
                case 0:
                    aa = color;
                    break;
                case 1:
                    ab = color;
                    break;
                case 2:
                    ba = color;
                    break;
                case 3:
                    bb = color;
                    break;
            }

            aa = (byte)(aa << 6);
            ab = (byte)(ab << 4);
            ba = (byte)(ba << 2);

            Poke(_systemDisplayData.Offset + index, (byte)(aa + ab + ba + bb));
        }

        public void DrawSprite(int x, int y, int id)
        {
            var data = Peek(16 * id, 16);
            for (var col = 0; col < data.Length / 2; col++)
            {
                for (var row = 0; row < data.Length / 8; row++)
                {
                    var index = col * (2) + row;
                    var @byte = data[index];

                    byte aa = (byte)(@byte >> 6);
                    byte ab = (byte)(@byte >> 4 & 0x03);
                    byte ba = (byte)(@byte >> 2 & 0x03);
                    byte bb = (byte)(@byte >> 0 & 0x03);
                    var dstX = x + (row * 4);
                    var dstY = y + (col);
                    SetPixel(dstX + 0, dstY, aa, true);
                    SetPixel(dstX + 1, dstY, ab, true);
                    SetPixel(dstX + 2, dstY, ba, true);
                    SetPixel(dstX + 3, dstY, bb, true);
                }
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //_keyboard.Update(gameTime);
            _mouse.Update(gameTime);
        }
    }
}
