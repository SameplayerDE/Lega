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

        public int Width => _width;
        public int Height => _height;

        public VirtualDisplay(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
