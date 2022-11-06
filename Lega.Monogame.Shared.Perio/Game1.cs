using Lega.Core.Memory;
using Lega.Core.Monogame.Input;
using Lega.Monogame.Shared.Perio.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading;
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

		private SpriteFont _font;

		private Texture2D _displayOutput;
		private Texture2D _pixel;
		private RenderTarget2D _target;

		private Rectangle _destination;

		private Random _random = new Random();

		private Vector2 _vel = Vector2.Zero;
		private Vector2 _pos = Vector2.Zero;

		private Stopwatch _debugUpdate;
		private Stopwatch _debugDraw;
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 720;
			_graphics.PreferredBackBufferHeight = 480;
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

			_debugDraw = new Stopwatch();
			_debugUpdate = new Stopwatch();

			base.Initialize();
			PerformScreenFit(VirtualSystem.Display.Width, VirtualSystem.Display.Height);
		}

		private void OnResize(object sender, EventArgs e)
		{
			PerformScreenFit(VirtualSystem.Display.Width, VirtualSystem.Display.Height);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Default");
			_displayOutput = new Texture2D(GraphicsDevice, VirtualSystem.Display.Width, VirtualSystem.Display.Height);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
			_pixel.SetData(new Color[] { Color.White });
			_target = new RenderTarget2D(GraphicsDevice, VirtualSystem.Display.Width, VirtualSystem.Display.Height);
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

            var velXSpan = VirtualSystem.UniversalData.Peek(0x08, 4);
            var velYSpan = VirtualSystem.UniversalData.Peek(0x0C, 4);
            var velXSingle = BitConverter.ToSingle(velXSpan);
            var velYSingle = BitConverter.ToSingle(velYSpan);

            if (SystemKeyboard.IsKeyDown(Keys.Right))
			{
                velXSingle += 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
				
			}
			if (SystemKeyboard.IsKeyDown(Keys.Down))
			{
				velYSingle += 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

			if (SystemKeyboard.IsKeyDown(Keys.Left))
			{
                velXSingle -= 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
			if (SystemKeyboard.IsKeyDown(Keys.Up))
			{
                velYSingle -= 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            velXSingle = (float)Math.Clamp(velXSingle, -5, 5);
            velYSingle = (float)Math.Clamp(velYSingle, -5, 5);

            VirtualSystem.UniversalData.Poke(0x08, BitConverter.GetBytes(velXSingle));
            //VirtualSystem.UniversalData.Poke4(0x08, BitConverter.GetBytes(velXSingle));
            VirtualSystem.UniversalData.Poke(0x0C, BitConverter.GetBytes(velYSingle));

			_pos.X += velXSingle;
			_pos.Y += velYSingle;
           // _pos += _vel;

			_vel *= 0.9f;
			//_pos.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 5;

			//_pos.X = (float)Math.Clamp(_pos.X, 0, VirtualSystem.Display.Width - 8);
			//_pos.Y = (float)Math.Clamp(_pos.Y, 0, VirtualSystem.Display.Height - 8);

			VirtualSystem.Display.Clear(0x00);

            DrawSprite((int)_pos.X, (int)_pos.Y, 2);
            DrawSprite(0, 0, 1);
            DrawSprite(8, 0, 1);
            DrawSprite(128, 0, 1);

            Task.Run(() =>
			{
				_displayOutput.SetData(Util.FromBufferPerio(VirtualSystem.Display.MemoryRegion.Peek(0x00, VirtualSystem.Display.BytesPerFrame)));
			});

			base.Update(gameTime);
			_debugUpdate.Stop();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_target);
			_spriteBatch.Begin();
			_spriteBatch.Draw(_displayOutput, Vector2.Zero, Color.White);
			_spriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(VirtualSystem.Colors[0]);
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_target, _destination, Color.White);

			_spriteBatch.Draw(_pixel, new Rectangle(0, 0, 80, 8), Color.Black);
			_spriteBatch.Draw(_pixel, new Rectangle(0, 0, 80, 8), Color.Black);
            _spriteBatch.DrawString(_font, $"{_debugUpdate.Elapsed.TotalMilliseconds}", Vector2.Zero, Color.Yellow);
            _spriteBatch.DrawString(_font, $"{_debugUpdate.Elapsed.TotalMilliseconds}", Vector2.Zero, Color.Yellow);

            var x = 0;
			var y = 0;
			var perRow = 16;
			for (int i = 1; i <= VirtualSystem.UniversalData.Bytes; i++)
			{
				var value = VirtualSystem.UniversalData.Peek(i - 1);

				_spriteBatch.DrawString(_font,$"{value:X2}", new Vector2(x, y), value > 0 ? Color.Red : Color.DarkGray);
				if (i % perRow == 0)
				{
					x = 0;
					y += _font.LineSpacing;
				}
				else
				{
					x += _font.LineSpacing * 2;
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

		public void DrawSprite(int x, int y, int id)
		{
			var data = VirtualSystem.SpriteData.Peek(32 * id, 32);
			for (var col = 0; col < data.Length / 4; col++)
			{
				for (var row = 0; row < data.Length / 8; row++)
				{
					var index = col * (4) + row;
					var @byte = data[index];
					//first nibble
					var takenFirst = @byte >> 4;
					//last nibble
					var takenLast = @byte & 0x0F;
					var dstX = x + (row * 2);
					var dstY = y + (col);
					VirtualSystem.Display.SetPixel(dstX, dstY, (byte)takenFirst);
					VirtualSystem.Display.SetPixel(dstX + 1, dstY, (byte)takenLast);
					//Console.WriteLine($"{row} : {col}");
				}
			}
			/*for (var row = 0; row < data.Length / 8; row++)
			{
				var @byte = data[i];
				//first nibble
				var takenFirst = @byte >> 4;
				//last nibble
				var takenLast = @byte & 0x0F;
				var dstX = x + i;
				var dstY = y;
				_display.SetPixel(dstX, dstY, (byte)takenFirst);
				_display.SetPixel(dstX, dstY, (byte)takenLast);
			}
			for (var i = 0; i < data.Length ; i++)
			{
				var @byte = data[i];
				//first nibble
				var takenFirst = @byte >> 4;
				//last nibble
				var takenLast = @byte & 0x0F;
				var dstX = x + i;
				var dstY = y;
				_display.SetPixel(dstX, dstY, (byte)takenFirst);
				_display.SetPixel(dstX, dstY, (byte)takenLast);
			}*/
		}
	}
}