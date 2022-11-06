using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Monogame.Input
{
    public static class SystemMouse
    {

        private static MouseState _curr;
        private static MouseState _prev;
        private static bool _stateChange;

        public static bool StateChange => _stateChange;
        public static Point Position => _curr.Position;

        public static void Update(GameTime gameTime)
        {
            _prev = _curr;
            _curr = Mouse.GetState();
            _stateChange = !_curr.Equals(_prev);
        }
    }
}
