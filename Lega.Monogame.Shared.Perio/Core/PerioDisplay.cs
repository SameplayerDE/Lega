using Lega.Core.Memory;
using System;

namespace Lega.Monogame.Shared.Perio.Core
{
	public class PerioDisplay : VirtualComponent
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

		public void SetPixel(int x, int y, int id)
		{
			if (x < 0 || y < 0 || x >= _width || y >= _height)
			{
				return;
			}
			//maps x and y to the address
			var index = y * (_width / 2) + (x / 2);
			//0 if it is a most sig byte 1 if it is least sig byte
			var region = x % 2;
			//color id
			var color = (byte)(id % 16);
			//what is currently saved at this address
			var takenBy = MemoryRegion.Peek(index);
			//first nibble
			var takenFirst = takenBy >> 4;
			//last nibble
			var takenLast = takenBy & 0x0F;

			if (color != 0)
			{
				if (region == 0)
				{
					takenFirst = color;
				}
				else
				{
					takenLast = color;
				}
			}

			takenFirst <<= 4;

			//Console.WriteLine($"{(takenFirst + takenLast):X2}");

			MemoryRegion.Poke(index, (byte)(takenFirst + takenLast));
		}

		public byte GetPixel(int x, int y)
		{
			return 0;
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

		internal void Clear(byte data)
		{
			//fill
			//data %= 16;
			MemoryRegion.Poke(0, MemoryRegion.Bytes, 0x00);
		}
	}
}
