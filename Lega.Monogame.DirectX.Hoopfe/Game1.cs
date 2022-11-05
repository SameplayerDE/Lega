using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Mime;

namespace Lega.Monogame.Shared.Hoopfe
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            int i = 0;
            while (i < 512)
            {
                VirtualSystem.Instance.Poke(i, 0xFF);
                i++;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Default");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
             _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
             var x = _displayOutput0.Width * _displayOutputScale;
             var y = 0;
             var perRow = 32;
             for (int i = 1; i <= 512; i++)
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
