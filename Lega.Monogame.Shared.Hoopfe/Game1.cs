using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lega.Monogame.Shared.Hoopfe
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private Effect _effect;
		private Texture2D _texture;

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

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = Content.Load<SpriteFont>("Default");
			_texture = new Texture2D(GraphicsDevice, 3, 3);
        }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


            
            base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);



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

			 _spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
