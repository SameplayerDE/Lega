using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lega.Core.Monogame.Input
{
    public class VirtualMouse : IUpdateable
    {

        public float X;
        public float Y;

        public void Map(Rectangle rect, int w, int h)
        {
            int offX = rect.X;
            int offY = rect.Y;
            X = SystemMouse.Position.X - offX;
            Y = SystemMouse.Position.Y - offY;
            X /= (float)rect.Width;
            Y /= (float)rect.Height;
            X *= (float)w;
            Y *= (float)h;
            // Y = (int)(Y / scaleY);
            Console.WriteLine($"X : {SystemMouse.Position.X}");
             Console.WriteLine($"OffX : {offX}");
             Console.WriteLine($"CalcX : {X}");
             Console.WriteLine($"");

        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
