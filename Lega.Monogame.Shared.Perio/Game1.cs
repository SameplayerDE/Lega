using Lega.Core.Memory;
using Lega.Core.Monogame.Graphics;
using Lega.Core.Monogame.Input;
using Lega.Monogame.Shared.Perio.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Lega.Monogame.Shared.Perio
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PerioDisplay _display;
        private VirtualMemory _memory;
        private VirtualKeyboard _keyboard;

        private Texture2D _displayOutput;
        private int _displayOutputScale = 2;

        private SpriteFont _font;

        private Stopwatch _clock;
        private Stopwatch _debugUpdate;
        private bool _clockCycle = false;

        private Random _random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            Window.AllowUserResizing = true;

            _memory = new VirtualMemory(16_384);
            _keyboard = new VirtualKeyboard();
            _display = new PerioDisplay(128, 128);

            try
            {
                //_keyboard.Map(_memory, 0, 16);
                _display.Map(_memory, 0, _display.BytesPerFrame);
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _clock = new Stopwatch();
            _debugUpdate = new Stopwatch();

            _display.MemoryRegion.Poke(0, _display.BytesPerFrame, 0x00);
            _display.MemoryRegion.Poke(0, 0x1F);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Default");
            _displayOutput = new Texture2D(GraphicsDevice, _display.Width, _display.Height);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_clock.IsRunning)
            {
                _clock.Start();
            }


            _clockCycle = false;
            if (_clock.Elapsed.TotalMilliseconds >= 1000f / 10)
            {
                _debugUpdate.Reset();
                _debugUpdate.Start();
                //_keyboard.Update(gameTime);
                var index = _random.Next(0, _display.MemoryLength);
                _display.MemoryRegion.Poke(index, (byte)_random.Next(0, 16));
                _displayOutput.SetData(Util.FromBufferPerio(_display.MemoryRegion.Data.ToArray()));
                _debugUpdate.Stop();
                _clockCycle = true;
                _clock.Restart();
            }
            Window.Title = $"{_debugUpdate.Elapsed.TotalMilliseconds}ms";
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var x = _displayOutput.Width * _displayOutputScale;
            var y = 0;
            var perRow = 32;
            for (int i = 1; i <= _memory.Capacity; i++)
            {
                _spriteBatch.DrawString(_font, $"{_memory.Peek(i - 1):X2}", new Vector2(x, y), _clockCycle ? Color.Red : Color.White);
                if (i % perRow == 0)
                {
                    x = _displayOutput.Width * _displayOutputScale;
                    y += _font.LineSpacing;
                }
                else
                {
                    x += _font.LineSpacing * 3;
                }
                
            }
            _spriteBatch.Draw(_displayOutput, new Rectangle(0, 0, _displayOutput.Width * _displayOutputScale, _displayOutput.Height * _displayOutputScale), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}