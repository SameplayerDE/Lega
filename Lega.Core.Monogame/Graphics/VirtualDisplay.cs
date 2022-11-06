using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System;
using Lega.Core.Memory;

namespace Lega.Core.Monogame.Graphics
{
    public class VirtualDisplay : VirtualComponent
    {
        private int _width;
        private int _height;
        private int _bytesPerPixel;
        private int _pixelCount;

        public int Width => _width;
        public int Height => _height;
        public int BytesPerPixel => _bytesPerPixel;
        public int PixelCount => _pixelCount;
        public int BytesPerFrame => _pixelCount * _bytesPerPixel;
        public VirtualDisplay(int width, int height, int bytes = 1)
        {
            _width = width;
            _height = height;
            _bytesPerPixel = bytes;
            _pixelCount = width * height;
        }

        public override void Map(VirtualMemory memory, int offset, int bytes)
        {
            if (bytes < BytesPerFrame)
            {
                throw new ArgumentOutOfRangeException($"not enough bytes to store display data. Display is using {_bytesPerPixel} bytes per pixel and has {_pixelCount}. Total of {BytesPerFrame} bytes has to be mapped.");
            }
            if (memory.Bytes < BytesPerFrame)
            {
                throw new ArgumentException($"memory has not enough space for the display to be stored. Total of {BytesPerFrame} bytes has to be free.");
            }
            base.Map(memory, offset, bytes);
        }
    }
}
