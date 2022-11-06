using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
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
        private uint _y = 0;

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
            _image.SetPixel(_x, _y, Color.White);
            _texture.Update(_image);
            _sprite.Texture = _texture;
        }

        public void Render(object sender, EventArgs eventArgs)
        {
            var window = (RenderWindow)sender;



            window.Draw(_sprite, RenderStates.Default);
        }
    }
}
