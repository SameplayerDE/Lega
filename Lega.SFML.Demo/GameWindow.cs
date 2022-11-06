using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lega.SFML.Demo
{
    internal class GameWindow
    {

        private VideoMode _videoMode;
        private RenderWindow _window;
        private Clock _timer;

        public event EventHandler Render;
        public event EventHandler Tick;

        public event EventHandler<KeyEventArgs> KeyPressed;
        public event EventHandler<SizeEventArgs> Resized;
        public event EventHandler<TextEventArgs> TextEntered;
        public event EventHandler<EventArgs> Closed;

        public void Init()
        {
            _videoMode = new VideoMode(800, 600);
            _window = new RenderWindow(_videoMode, "SFML works!");

            _window.Closed += OnClosed;
            _window.KeyPressed += OnKeyPressed;
            _window.Resized += OnResized;
            _window.TextEntered += OnTextEntered;
            _timer = new Clock();
        }

        private void OnTextEntered(object? sender, TextEventArgs e)
        {
            TextEntered?.Invoke(sender, e);
        }


        private void OnResized(object? sender, SizeEventArgs e)
        {
            Resized?.Invoke(sender, e);
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            KeyPressed?.Invoke(sender, e);
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            Closed?.Invoke(sender, e);
        }

        public void Run()
        {
            Init();
            _timer.Restart();

            int lastTime = _timer.ElapsedTime.AsMilliseconds();
            int lastTimer = _timer.ElapsedTime.AsMilliseconds(); //to output the ticks and fps in the console
            double unprocessed = 0; //counts unprocessed ticks to compensate
            double msPerTick = 1000 / 60d; //frames per seconds
            int frames = 0;
            int ticks = 0;

            while (_window.IsOpen)
            {
                int now = _timer.ElapsedTime.AsMilliseconds();
                unprocessed += (now - lastTime) / msPerTick; //calculates unprocessed time
                lastTime = now;
                bool shouldRender = true;

                //if game skipped updates, compensate for that 
                while (unprocessed >= 1)
                {
                    ticks++;
                    Update();
                    unprocessed -= 1;
                    shouldRender = true;
                }

                if (shouldRender)
                {
                    frames++;
                    Draw();
                }

                if (_timer.ElapsedTime.AsMilliseconds() - lastTimer > 1000)
                {
                    lastTimer += 1000;
                    Console.WriteLine(ticks + " ticks, " + frames + " f/s");
                    frames = 0;
                    ticks = 0;
                }

            }
        }

        public void Close()
        {
            _window.Close();
        }

        void Update()
        {
            _window.DispatchEvents();
            Tick?.Invoke(null, EventArgs.Empty);
        }

        void Draw()
        {
            Clear();
            //Begin


            Render?.Invoke(_window, EventArgs.Empty);

            //End
            _window.Display();
        }

        void Clear()
        {
            _window.Clear();
        }


    }
}
