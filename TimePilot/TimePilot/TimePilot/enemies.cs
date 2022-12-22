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
        public int hp;
        public List<Rectangle> bullets;
        int timeToShoot;
        int time;
        public Enemy(Texture2D tex, int points) 
        {
            hp = 3;
            rand = new Random();
            this.tex = tex;
            dx = rand.Next(-5, 5);
            dy = rand.Next(-5, 5);

            bullets = new List<Rectangle>();
            int direction = rand.Next(0, 1);
            rotation = (float)Math.Atan2(dy, dx) + (float)Math.PI/2;
            timeToShoot = rand.Next(0, 180);
            int startX = 0;

            int startY = 0;

            /*while (dx == 0 && dy == 0)
            {
                dx = rand.Next(-5, 5);
                dy = rand.Next(-5, 5);
            }*/

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

            if (dx == 0 && dy == 0)
            {
                startX = -10;
                startY = -10;
            }




            rect = new Rectangle(startX, startY, 100, 100);
            hitbox = new Rectangle(startX-20, startY-20, 40, 40);
        }

        public void update()
        {
            time++;
            rect.X += (int)(dx);
            rect.Y += (int)(dy);

            hitbox.X += (int)(dx);
            hitbox.Y += (int)(dy);

            if (timeToShoot == time)
            {
                time = 0;
                timeToShoot = rand.Next(0, 120);
                bullets.Add(new Rectangle(rect.X, rect.Y, 10, 10));
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                
                int x = (int)(dx * 2); 
                int y = (int)(dy * 2);
                int currX = bullets[i].X;
                int currY = bullets[i].Y;

                bullets[i] = new Rectangle(currX + x, currY + y, 10, 10);
            }
        }
    }
}
