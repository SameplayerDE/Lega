using Lega.Core.Memory;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Lega.Core.Monogame.Input
{
    public class VirtualKeyboard : VirtualComponent, IUpdateable
    {

        private KeyboardState _curr;
        private KeyboardState _prev;

        public void Update(GameTime gameTime)
        {


            _prev = _curr;
            _curr = Keyboard.GetState();

            var currLeft = _curr.IsKeyDown(Keys.Left);
            var currRight = _curr.IsKeyDown(Keys.Right);

            var prevLeft = _prev.IsKeyDown(Keys.Left);
            var prevRight = _prev.IsKeyDown(Keys.Right);

            MemoryRegion.Poke(0x00, (byte)(currLeft ? 1 : 0));
            MemoryRegion.Poke(0x01, (byte)(currRight ? 1 : 0));

            MemoryRegion.Poke(0x00 + 0x06, (byte)(prevLeft ? 1 : 0));
            MemoryRegion.Poke(0x01 + 0x06, (byte)(prevRight ? 1 : 0));

            /*var memState = MemoryRegion.Peek(0x00);
            var sysState = 0b0000_0000;
            
            if (left)
            {
                sysState |= 0b0000_0001;
            }
            else
            {

            }
            if (right)
            {
                sysState |= 0b0000_0010;
            }*/



        }
    }
}
