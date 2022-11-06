using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.SFML.Demo
{
    internal class Game
    {

        private GameWindow _window;
        private Image _image;
        private Texture _texture;
        private Sprite _sprite;
        private uint _x = 0;
        private uint _y = 64;
        public Clock Timer { get; set; }

        public void Run()
        {
            _window = new GameWindow();
            _window.Tick += Tick;
            _window.Render += Render;
            _window.Closed += OnClose;
            _window.KeyPressed += OnKeyPress;

            _image = new Image(128, 128);
            _texture = new Texture(128, 128);
            _sprite = new Sprite();
            Timer = new Clock();
            Timer.Restart();
            _window.Run();
        }

        private void OnKeyPress(object? sender, KeyEventArgs e)
        {

        }

        private void OnClose(object? sender, EventArgs e)
        {
            _window.Close();
        }

        public void Tick(object sender, EventArgs eventArgs)
        {
            _y = _y + 1;
            if (_y >= 128)
            {
                Console.WriteLine(Timer.ElapsedTime.AsSeconds());
                _y = 64;
                Timer.Restart();
            }
        }

        public void Render(object sender, EventArgs eventArgs)
        {
            var window = (RenderWindow)sender;



            window.Draw(_sprite, RenderStates.Default);
        }
    }
}
