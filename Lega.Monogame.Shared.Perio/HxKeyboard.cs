using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Calendula.Framework.Input
{
    public static class HxKeyboard
    {
        private static KeyboardState _curr;
        private static KeyboardState _prev;
        private static bool _stateChange;

        public static bool StateChange => _stateChange;
        
        public static void Update(GameTime gameTime)
        {
            _prev = _curr;
            _curr = Keyboard.GetState();
            _stateChange = !_curr.Equals(_prev);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _curr.IsKeyDown(key);
        }
        
        public static bool WasKeyDown(Keys key)
        {
            return _prev.IsKeyDown(key);
        }
        
        public static bool IsKeyUp(Keys key)
        {
            return _curr.IsKeyUp(key);
        }
        
        public static bool WasKeyUp(Keys key)
        {
            return _prev.IsKeyUp(key);
        }

        public static bool IsKeyDownOnce(Keys key)
        {
            return !WasKeyDown(key) && IsKeyDown(key);
        }
        
        public static bool IsKeyUpOnce(Keys key)
        {
            return !WasKeyUp(key) && IsKeyUp(key);
        }
        
    }
}