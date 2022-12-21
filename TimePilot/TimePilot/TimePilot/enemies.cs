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
    class Enemy
    {
        public Texture2D tex;
        public int lives;
        public int dx;
        public int dy;
        public int points;
        public float rotation;
        Random rand;
        public Rectangle rect;
        public Rectangle hitbox;
        public Enemy(Texture2D tex, int points) 
        {
            rand = new Random();
            this.tex = tex;
            dx = rand.Next(-5, 5);
            dy = rand.Next(-5, 5);

            int direction = rand.Next(0, 1);
            rotation = (float)Math.Atan2(dy, dx) + (float)Math.PI/2;

            int startX = 0;

            int startY = 0;

            while (dx == 0 && dy == 0)
            {
                dx = rand.Next(-5, 5);
                dy = rand.Next(-5, 5);
            }

            if (dx > 0 && dy > 0)
            {
                if (direction == 1)
                {
                    startX = 0;
                    startY = rand.Next(0, 400);

                }
                else if (direction == 0)
                {
                    startY = 0;
                    startX = rand.Next(0, 400);
                }
                
            }

            if (dx < 0 && dy > 0)
            {
                if (direction == 1)
                {
                    startX = 800;
                    startY = rand.Next(0, 400);

                }
                else if (direction == 0)
                {
                    startY = 0;
                    startX = rand.Next(400, 800);

                }

            }

            if (dx > 0 && dy < 0)
            {
                if (direction == 1)
                {
                    startX = 0;
                    startY = rand.Next(400, 800);

                }
                else if (direction == 0)
                {
                    startY = 800;
                    startX = rand.Next(0, 400);
                }

            }

            if (dx < 0 && dy < 0)
            {
                if (direction == 1)
                {
                    startX = 800;
                    startY = rand.Next(0, 400);

                }
                else if (direction == 0)
                {
                    startY = 800;
                    startX = rand.Next(400, 800);
                }
            }


            

            rect = new Rectangle(startX, startY, 100, 100);
            hitbox = new Rectangle(startX-20, startY-20, 40, 40);
        }

        public void update()
        {
            rect.X += dx;
            rect.Y += dy;

            hitbox.X += dx;
            hitbox.Y += dy;

        }
    }
}
