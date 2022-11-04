using Lega.Core.Memory;
using Lega.Core.Monogame.Graphics;
using Lega.Core.Monogame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Lega.Monogame.Shared.Perio.Core
{
	public static class VirtualSystem
	{

		public static Color[] Colors { get; } =
		{
			//Util.FromHex("16171a"),
			Color.Transparent,
			Util.FromHex("7f0622"),
			Util.FromHex("d62411"),
			Util.FromHex("ff8426"),
			Util.FromHex("ffd100"),
			Util.FromHex("fafdff"),
			Util.FromHex("ff80a4"),
			Util.FromHex("ff2674"),
			Util.FromHex("94216a"),
			Util.FromHex("430067"),
			Util.FromHex("234975"),
			Util.FromHex("68aed4"),
			Util.FromHex("bfff3c"),
			Util.FromHex("10d275"),
			Util.FromHex("007899"),
			Util.FromHex("002859"),
		};

		public static VirtualMemory Memory;

		public static PerioDisplay Display;
		public static VirtualKeyboard Keyboard;
		
		public static VirtualMemoryRegion SpriteData;
		public static VirtualMemoryRegion UniversalData;

		static VirtualSystem()
		{
			//Memory = new VirtualMemory(131_072);
			//Memory = new VirtualMemory(8_148);
			Memory = new VirtualMemory(16_296);
			Keyboard = new VirtualKeyboard();
			Display = new PerioDisplay(98, 64);
			//Display = new PerioDisplay(192, 128);
			SpriteData = new VirtualMemoryRegion();
			UniversalData = new VirtualMemoryRegion();

			try
			{
				var displayAdress = 0x00;
				var displayLength = Display.BytesPerFrame;

				var keyboardAdress = 0x00;
				var keyboardLength = 16;

				var spriteAdress = 0x00;
				var spriteLength = 512;

				var univesalAdress = 0x00;
				var universalLength = 64;

				Display.Map(Memory, displayAdress, displayLength);
				Keyboard.Map(Memory, keyboardAdress + displayLength, keyboardLength);
				SpriteData.Map(Memory, spriteAdress + keyboardLength + displayLength, spriteLength);
				UniversalData.Map(Memory, univesalAdress + spriteLength + keyboardLength + displayLength, universalLength);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}

			SpriteData.Poke(00, 0x55, 0x55, 0x55, 0x55);
			SpriteData.Poke(04, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(08, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(12, 0x50, 0x08, 0x20, 0x05);
			SpriteData.Poke(16, 0x50, 0x00, 0x40, 0x05);
			SpriteData.Poke(20, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(24, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(28, 0x55, 0x55, 0x55, 0x55);

			SpriteData.Poke(32, 0x05, 0x00, 0x00, 0x50);
			SpriteData.Poke(36, 0x50, 0x50, 0x05, 0x05);
			SpriteData.Poke(40, 0x50, 0x05, 0x50, 0x05);
			SpriteData.Poke(44, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(48, 0x50, 0x00, 0x00, 0x05);
			SpriteData.Poke(52, 0x05, 0x00, 0x00, 0x50);
			SpriteData.Poke(56, 0x00, 0x50, 0x05, 0x00);
			SpriteData.Poke(60, 0x00, 0x05, 0x50, 0x00);

			SpriteData.Poke(064, 0x00, 0x00, 0x00, 0x00);
			SpriteData.Poke(068, 0x05, 0x55, 0x55, 0x00);
			SpriteData.Poke(072, 0x05, 0x50, 0x05, 0x50);
			SpriteData.Poke(076, 0x05, 0x50, 0x05, 0x50);
			SpriteData.Poke(080, 0x05, 0x55, 0x55, 0x00);
			SpriteData.Poke(084, 0x05, 0x50, 0x00, 0x00);
			SpriteData.Poke(088, 0x05, 0x50, 0x00, 0x00);
			SpriteData.Poke(092, 0x00, 0x00, 0x00, 0x00);

			UniversalData.Poke(0x00, 0x00, 0x00, 0x00, 0x00); //x
			UniversalData.Poke(0x04, 0x00, 0x00, 0x00, 0x00); //y
			UniversalData.Poke(0x08, 0x00, 0x00, 0x00, 0x00); //velx
			UniversalData.Poke(0x0C, 0x00, 0x00, 0x00, 0x00); //vely
		}
	}
}