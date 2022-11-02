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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private PerioDisplay _display;
        private VirtualMemory _memory;
        private VirtualKeyboard _keyboard;

        private Texture2D _displayOutput;
        private RenderTarget2D _target;
        private int _displayOutputScale = 2;

        private SpriteFont _font;

        private Stopwatch _clock;
        private Stopwatch _debugUpdate;
        private Stopwatch _debugDraw;

        private TimeSpan _lastDebugUpdate;
        private TimeSpan _lastDebugDraw;

        private bool _clockCycle = false;
        private bool _back = false;

        public bool IsFullScreen
        {
            get => _graphics.IsFullScreen;
            set
            {
                _graphics.IsFullScreen = value;
                _graphics.ApplyChanges();
            }
        }


        private Rectangle _destination;

        private Random _random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = 256 * 4;
            _graphics.PreferredBackBufferHeight = 192 * 4;
            _graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24; // <-- set depth here

            _graphics.HardwareModeSwitch = false;
            _graphics.PreferMultiSampling = false;
            _graphics.IsFullScreen = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();

            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30f);
            IsFixedTimeStep = true;


            _memory = new VirtualMemory(16_384);
            _keyboard = new VirtualKeyboard();
            _display = new PerioDisplay(128, 128);

            try
            {
                //_keyboard.Map(_memory, 0, 16);
                _display.Map(_memory, 0, _display.BytesPerFrame);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _clock = new Stopwatch();
            _debugUpdate = new Stopwatch();
            _debugDraw = new Stopwatch();

            _display.MemoryRegion.Poke(0, _display.BytesPerFrame, 0x00);
            _display.MemoryRegion.Poke(0, 0x1F);

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;

            PerformScreenFit(_display.Width, _display.Height);

            base.Initialize();
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
        }

        protected override void Update(GameTime gameTime)
        {
            _lastDebugUpdate = _debugUpdate.Elapsed;
            _debugUpdate.Reset();
            _debugUpdate.Start();
            SystemKeyboard.Update(gameTime);
            if (SystemKeyboard.IsKeyDown(Keys.LeftAlt))
            {
                if (SystemKeyboard.IsKeyDownOnce(Keys.Enter))
                {
                    IsFullScreen = !IsFullScreen;
                }
            }

            if (!_clock.IsRunning)
            {
                _clock.Start();
            }


            _clockCycle = false;
            //if (_clock.Elapsed.TotalMilliseconds >= 1000f / 60)
           // {
                
                //_keyboard.Update(gameTime);
                var index = _random.Next(0, _display.MemoryLength);
                _display.MemoryRegion.Poke(index, (byte)_random.Next(0, 16));
                if (SystemKeyboard.IsKeyDown(Keys.Space))
                {
                    new Task(() =>
                    {
                        _displayOutput.SetData(Util.FromBufferPerio(_display.MemoryRegion.Data));
                    }).Start();
                }
                //_clockCycle = true;
                //_target.SetData(Util.FromBufferPerio(_display.MemoryRegion.Data.ToArray()));
              //  _clock.Restart();
            //}
            base.Update(gameTime);
            _debugUpdate.Stop();
            //Window.Title = $"{_debugUpdate.Elapsed.TotalMilliseconds}ms | {_debugDraw.Elapsed.TotalMilliseconds}ms";
        }

        protected override void Draw(GameTime gameTime)
        {
            _lastDebugDraw = _debugDraw.Elapsed;
            _debugDraw.Reset();
            _debugDraw.Start();
            GraphicsDevice.SetRenderTarget(_target);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_displayOutput, Vector2.Zero, Color.White);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(VirtualSystem.Colors[15]);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_target, _destination, Color.White);
            _spriteBatch.DrawString(_font, $"{_lastDebugUpdate.TotalMilliseconds}ms | {_lastDebugDraw.TotalMilliseconds}ms", Vector2.Zero, Color.White);
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
            _debugDraw.Stop();
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