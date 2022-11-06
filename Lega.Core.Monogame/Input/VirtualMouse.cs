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
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
