using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System;
using System.Threading.Tasks;
using Lega.Monogame.Shared.Hoopfe.Core;
using Lega.Core.Monogame.Input;
using System.Diagnostics;
using System.Threading;
using System.Security.AccessControl;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;

namespace Lega.Monogame.Shared.Hoopfe
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private SpriteFont _font;

		private Texture2D _output;
		private RenderTarget2D _target;
		private Rectangle _destination;
		public bool IsFullScreen
		{
			get => _graphics.IsFullScreen;
			set
			{
				_graphics.IsFullScreen = value;
				_graphics.ApplyChanges();
			}
		}

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
		}


		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 720;
			_graphics.PreferredBackBufferHeight = 720;
			_graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
			_graphics.PreferredDepthStencilFormat = DepthFormat.Depth24; // <-- set depth here

			_graphics.HardwareModeSwitch = false;
			_graphics.PreferMultiSampling = false;
			_graphics.IsFullScreen = false;
			_graphics.SynchronizeWithVerticalRetrace = false;
			_graphics.ApplyChanges();

			Window.ClientSizeChanged += OnResize;
			IsFixedTimeStep = true;
			MaxElapsedTime = TimeSpan.FromSeconds(1);
			TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
			Window.AllowUserResizing = true;

			PerformScreenFit(128, 128);

			base.Initialize();
		}

		private void OnResize(object sender, EventArgs e)
		{
			PerformScreenFit(128, 128);
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Default");

			_output = new Texture2D(GraphicsDevice, 128, 128);
			_target = new RenderTarget2D(GraphicsDevice, 128, 128);

			/*
			VirtualSystem.Instance.Poke4(0x400, 0b00_00_00_00_00_00_00_00_00_00_00_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x420, 0b00_00_00_00_00_00_00_00_00_00_00_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x440, 0b00_00_00_11_11_11_00_00_00_00_11_11_11_00_00_00);
			VirtualSystem.Instance.Poke4(0x460, 0b00_00_11_00_00_00_11_00_00_11_00_00_00_11_00_00);
			VirtualSystem.Instance.Poke4(0x480, 0b00_11_00_00_00_00_00_11_11_00_00_00_00_00_11_00);
			VirtualSystem.Instance.Poke4(0x4A0, 0b00_11_00_00_00_00_00_00_00_00_00_00_00_00_11_00);
			VirtualSystem.Instance.Poke4(0x4C0, 0b00_11_00_00_00_00_00_00_00_00_00_00_00_00_11_00);
			VirtualSystem.Instance.Poke4(0x4E0, 0b00_11_00_00_00_00_00_00_00_00_00_00_00_00_11_00);
			VirtualSystem.Instance.Poke4(0x500, 0b00_00_11_00_00_00_00_00_00_00_00_00_00_11_00_00);
			VirtualSystem.Instance.Poke4(0x520, 0b00_00_00_11_00_00_00_00_00_00_00_00_11_00_00_00);
			VirtualSystem.Instance.Poke4(0x540, 0b00_00_00_00_11_00_00_00_00_00_00_11_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x560, 0b00_00_00_00_00_11_00_00_00_00_11_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x580, 0b00_00_00_00_00_00_11_00_00_11_00_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x5A0, 0b00_00_00_00_00_00_00_11_11_00_00_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x5C0, 0b00_00_00_00_00_00_00_00_00_00_00_00_00_00_00_00);
			VirtualSystem.Instance.Poke4(0x5E0, 0b00_00_00_00_00_00_00_00_00_00_00_00_00_00_00_00);
			*/
		}

		protected override void Update(GameTime gameTime)
		{
			SystemKeyboard.Update(gameTime);
			SystemMouse.Update(gameTime);
			VirtualSystem.Instance.Update(gameTime);

			VirtualSystem.Instance._mouse.Map(_destination, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

			if (SystemKeyboard.IsKeyDown(Keys.LeftAlt))
			{
				if (SystemKeyboard.IsKeyDownOnce(Keys.Enter))
				{
					IsFullScreen = !IsFullScreen;
				}
			}

			if (!VirtualSystem.Instance.AudioDriver.IsPlaying)
			{
				VirtualSystem.Instance.AudioDriver.Play();
			}

			VirtualSystem.Instance.AudioDriver[0].Gain = 0.1;
			VirtualSystem.Instance.AudioDriver[0].Frequency = 500 + Math.Sin(gameTime.TotalGameTime.TotalMilliseconds) * 250;

			VirtualSystem.Instance.AudioDriver.Pause();

			VirtualSystem.Instance.Clear(0x400, 4096);
			VirtualSystem.Instance.DrawSprite((int)VirtualSystem.Instance._mouse.X, (int)VirtualSystem.Instance._mouse.Y, 00);
			_output.SetData(Util.FromBuffer(VirtualSystem.Instance.Peek(0x400, 4_096)));

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{

            GraphicsDevice.SetRenderTarget(_target);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_output, Vector2.Zero, Color.White);
			_spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);

			//GraphicsDevice.Clear(Color.Black);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			/*
			var x = 0;
			var y = 0;
			var perRow = 32;

			for (int i = 1; i <= 4_096; i++)
			{
				_spriteBatch.DrawString(_font, $"{VirtualSystem.Instance.Peek(i - 1):X2}", new Vector2(x, y), Color.White);
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
			*/

			_spriteBatch.Draw(_target, _destination, Color.White);
			_spriteBatch.DrawString(_font, "" + 1 / gameTime.ElapsedGameTime.TotalSeconds, Vector2.Zero, Color.Yellow);
			_spriteBatch.End();

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
			}
			_destination = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
		}
	}
}
