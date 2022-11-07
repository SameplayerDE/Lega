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
            float sx = SystemMouse.Position.X - rect.X;
            float sy = SystemMouse.Position.Y - rect.Y;

            sx /= (float)rect.Width;
            sx *= (float)w;
            sx /= (rect.Width / 128f);

            sy /= (float)rect.Height;
            sy *= (float)h;
            sy /= (rect.Height / 128f);

            X = sx;
            Y = sy;
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
