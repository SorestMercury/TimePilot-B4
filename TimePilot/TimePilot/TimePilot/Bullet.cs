using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimePilot
{

    
    class Bullet
    {

        public Rectangle rect;
        public double rotation;


        public Bullet(double rotate)
        {
            rotation = rotate - Math.PI/2;
            rect = new Rectangle(400, 400, 10, 10);
        }

        public void update()
        {
            rect.X += (int)(Math.Cos(rotation) * 10);
            rect.Y += (int)(Math.Sin(rotation) * 10);

        }
    


    }
}
