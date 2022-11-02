using Lega.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Monogame.Shared.Perio.Core
{
    internal class PerioDisplay : VirtualComponent
    {
        private int _width;
        private int _height;
        private int _pixelCount;

        public int Width => _width;
        public int Height => _height;
        public int PixelCount => _pixelCount;
        public int BytesPerFrame => _pixelCount / 2;
        public PerioDisplay(int width, int height)
        {
            _width = width;
            _height = height;
            _pixelCount = width * height;
        }

        public override void Map(VirtualMemory memory, int offset, int bytes)
        {
            if (bytes < BytesPerFrame)
            {
                throw new ArgumentException($"not enough bytes to store display data. Total of {BytesPerFrame} bytes has to be mapped.");
            }
            if (memory.Capacity < BytesPerFrame)
            {
                throw new ArgumentException($"memory has not enough space for the display to be stored. Total of {BytesPerFrame} bytes has to be free.");
            }
            base.Map(memory, offset, bytes);
        }

    }
}
