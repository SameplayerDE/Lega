﻿using Lega.Core.Memory;
using System;

namespace Lega.Monogame.Shared.Hoopfe
{
    public sealed class VirtualSystem
    {

        private VirtualMemory _systemMemory;

        private VirtualSystem()
        {
            _systemMemory = new VirtualMemory(512);
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
