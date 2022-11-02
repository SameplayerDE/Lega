using Lega.Core.Memory;
using Lega.Core.Monogame.Input;
using Lega.Monogame.Shared.Perio.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lega.Monogame.Shared.Perio
{
	public class Game1 : Game
	{

		private SpriteBatch _spriteBatch;
		private GraphicsDeviceManager _graphics;
		public bool IsFullScreen
		{
			get => _graphics.IsFullScreen;
			set
			{
				_graphics.IsFullScreen = value;
				_graphics.ApplyChanges();
			}
		}


		private PerioDisplay _display;
		private VirtualMemory _memory;
		private VirtualKeyboard _keyboard;

		private SpriteFont _font;

		private Texture2D _displayOutput;
		private RenderTarget2D _target;

		private Rectangle _destination;

		private Random _random = new Random();


		private Stopwatch _debugUpdate;
		private Stopwatch _debugDraw;
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 680;
			_graphics.PreferredBackBufferHeight = 420;
			_graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
			_graphics.PreferredDepthStencilFormat = DepthFormat.Depth24; // <-- set depth here

			_graphics.HardwareModeSwitch = false;
			_graphics.PreferMultiSampling = false;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();

			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnResize;
			IsFixedTimeStep = true;
			MaxElapsedTime = TimeSpan.FromSeconds(1);
			TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

			_memory = new VirtualMemory(16_384);
			_keyboard = new VirtualKeyboard();
			_display = new PerioDisplay(128, 128);

			try
			{
				_keyboard.Map(_memory, 0, 16);
				_display.Map(_memory, 64, _display.BytesPerFrame);
				_memory.Poke(64, 0x55);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			_display.MemoryRegion.Poke(0, _display.BytesPerFrame, 0x00);
			_display.MemoryRegion.Poke(0, 0x1F);

			_display.SetPixel(0, 1, 5);
			_display.SetPixel(1, 0, 5);
			_display.SetPixel(2, 1, 5);

			_debugDraw = new Stopwatch();
			_debugUpdate = new Stopwatch();

			base.Initialize();
			PerformScreenFit(_display.Width, _display.Height);
		}

		private void OnResize(object sender, EventArgs e)
		{
			PerformScreenFit(_display.Width, _display.Height);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Default");
			_displayOutput = new Texture2D(GraphicsDevice, _display.Width, _display.Height);
			_target = new RenderTarget2D(GraphicsDevice, _display.Width, _display.Height);
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			_debugUpdate.Restart();
			SystemKeyboard.Update(gameTime);
			if (SystemKeyboard.IsKeyDown(Keys.LeftAlt))
			{
				if (SystemKeyboard.IsKeyDownOnce(Keys.Enter))
				{
					IsFullScreen = !IsFullScreen;
				}
			}

			if (SystemKeyboard.IsKeyDownOnce(Keys.Space))
			{
				if (_display.MemoryRegion.Offset == 64)
				{
					_display.Map(_memory, 1028, _display.BytesPerFrame);
				}
				else
				{
					_display.Map(_memory, 64, _display.BytesPerFrame);
				}
			}

			new Task(() =>
			{
				for (var i = 0; i < _display.Width; i++)
				{
					//Thread.Sleep(10);
					_display.SetPixel(_random.Next(0, _display.Width), _random.Next(0, _display.Height), (byte)_random.Next(0, 16));
					//_memory.Poke(_random.Next(64, _display.MemoryLength), (byte)_random.Next(0, 255));
				}
				_displayOutput.SetData(Util.FromBufferPerio(_display.MemoryRegion.Data));
			}).Start();
			base.Update(gameTime);
			_debugUpdate.Stop();
			Window.Title = $"{_debugUpdate.Elapsed.TotalMilliseconds}";
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_target);
			_spriteBatch.Begin();
			_spriteBatch.Draw(_displayOutput, Vector2.Zero, Color.White);
			_spriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(VirtualSystem.Colors[15]);
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_target, _destination, Color.White);
			var x = 0;
			var y = 0;
			var perRow = 16;
			for (int i = 1; i <= _memory.Capacity; i++)
			{
				_spriteBatch.DrawString(_font, $"{_memory.Peek(i - 1):X2}", new Vector2(x, y), Color.White);
				if (i % perRow == 0)
				{
					x = 0;
					y += _font.LineSpacing;
				}
				else
				{
					x += _font.LineSpacing * 3;
				}

			}
			_spriteBatch.End();

			/*GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			var x = _displayOutput0.Width * _displayOutputScale;
			var y = 0;
			var perRow = 32;
			for (int i = 1; i <= _memory.Capacity; i++)
			{
				_spriteBatch.DrawString(_font, $"{_memory.Peek(i - 1):X2}", new Vector2(x, y), _clockCycle ? Color.Red : Color.White);
				if (i % perRow == 0)
				{
					x = _displayOutput0.Width * _displayOutputScale;
					y += _font.LineSpacing;
				}
				else
				{
					x += _font.LineSpacing * 3;
				}
				
			}
			_spriteBatch.Draw(_target, new Rectangle(0, 0, _target.Width * _displayOutputScale, _target.Height * _displayOutputScale), Color.White);

			_spriteBatch.End();*/

			base.Draw(gameTime);
		}

		private void PerformScreenFit(int scrW, int scrH)
		{
			var (_, _, width, height) = GraphicsDevice.Viewport.Bounds;
			var aspectRatioViewport = GraphicsDevice.Viewport.AspectRatio;
			var aspectRatioVirtualDisplay = (float)scrW / scrH;

			var rx = 0f;
			var ry = 0f;
			float rw = width;
			float rh = height;

			if (aspectRatioViewport > aspectRatioVirtualDisplay)
			{
				rw = rh * aspectRatioVirtualDisplay;
				rx = (width - rw) / 2f;
			}
			else if (aspectRatioViewport < aspectRatioVirtualDisplay)
			{
				rh = rw / aspectRatioVirtualDisplay;
				ry = (height - rh) / 2f;
#if __MOBILE__
				ry = 0;
#endif
			}

			_destination = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
		}

	}
}