using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System;
using System.Threading.Tasks;
using Lega.Monogame.Shared.Hoopfe.Core;
using Lega.Core.Monogame.Input;

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
        }

		protected override void Update(GameTime gameTime)
		{
            SystemKeyboard.Update(gameTime);
            if (SystemKeyboard.IsKeyDown(Keys.LeftAlt))
            {
                if (SystemKeyboard.IsKeyDownOnce(Keys.Enter))
                {
                    IsFullScreen = !IsFullScreen;
                }
            }

            Task.Run(() =>
            {
                _output.SetData(Util.FromBuffer(VirtualSystem.Instance.Peek(0x00, 4_096)));
            });

            base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(_target);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_output, Vector2.Zero, Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(Color.Black);

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
#if __MOBILE__
				ry = 0;
#endif
            }
            _destination = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
        }
    }
}
