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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle ship;
        Texture2D debug;
        float rotation;
        List<Bullet> bullets;
        Texture2D bulletTex;
        int timer;
        List<Enemy> enemies;
        Texture2D plane;
        Texture2D spriteSheet1;
        Rectangle[] shipSource;
        Rectangle currentSprite;
        float[] rotations;
        int score = 0;
        Rectangle shipHitBox;
        Texture2D cloud1;
        List<Rectangle> topClouds;
        int cloud1vel = 5;
        Texture2D cloud2;
        List<Rectangle> bottomClouds;
        SpriteFont titleScreenFont;
        enum Status { title, playLoad, play, lostLife, gameover}
        Status gameState;
        KeyboardState kb;
        GamePadState oldpad1 = GamePad.GetState(PlayerIndex.One);
        SpriteFont bigFont;
        int loadTime=0;
        Color background = Color.Black;
        Rectangle[] lives;
        int hp = 3;
        int lostLife = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            ship = new Rectangle(400, 400, 60, 65);
            rotation = 0;
            bullets = new List<Bullet>();

            enemies = new List<Enemy>();
            
            shipSource = new Rectangle[32];

            Vector2 temp = new Vector2(0, 0);
            for(int x=0;x<shipSource.Length;x++)
            {
                shipSource[x] = new Rectangle((int)temp.X,(int)temp.Y,82,85);

                if (temp.Y == 0 || temp.Y==180)
                    temp.X += 88;

                else
                    temp.X += 90;

                if ((x + 1) % 8 == 0)
                {
                    temp.Y += 90;
                    temp.X = 0;
                }
            }

            Rectangle[] tempArray = new Rectangle[shipSource.Length];
            for (int x = 0; x < 17; x++)
                tempArray[x] = shipSource[x];

            int flip = 17;
            for (int x = shipSource.Length-1; x > 16; x--)
            {
                tempArray[flip] = shipSource[x];
                flip++;
            }

            tempArray[16].Y-=10;
            shipSource=tempArray;

            currentSprite = shipSource[0];

            rotations = new float[33];

            int count = 0;
            for (double x = 0; x > -(Math.PI); x -= (Math.PI / 16))
            {
                rotations[count] = (float)x;
                count++;
            }

            for (double x = Math.PI; x > 0; x -= (Math.PI / 16))
            {
                rotations[count] = (float)x;
                count++;
            }

            rotations[32] = rotations[0];

            shipHitBox = new Rectangle(ship.X-ship.Width/2, ship.Y-ship.Height/2, ship.Width-8, ship.Height-8);

            topClouds = new List<Rectangle>();
            bottomClouds = new List<Rectangle>();

            topClouds.Add(new Rectangle(0, 0, 800, 800));

            //topClouds.Add(new Rectangle(0, -800, 800, 800));

            for (int y = ship.Y - 1200; y < 2400; y += 800)
                for (int x = ship.X - 1200; x < 2400; x += 800)
                    topClouds.Add(new Rectangle(x, y, 800, 800));

            for (int y = ship.Y - 1200; y < 2400; y += 800)
                for (int x = ship.X - 1200; x < 2400; x += 800)
                    bottomClouds.Add(new Rectangle(x, y, 800, 800));


            gameState = Status.play;

            lives = new Rectangle[3];
            lives[0] = new Rectangle(20, 20, 60, 65);
            lives[1] = new Rectangle(80, 20, 60, 65);
            lives[2] = new Rectangle(140, 20, 60, 65);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            plane = this.Content.Load<Texture2D>("plane");
            debug = this.Content.Load<Texture2D>("debug");
            bulletTex = this.Content.Load<Texture2D>("square");


            enemies.Add(new Enemy(plane, 1000));
            enemies.Add(new Enemy(plane, 1000));
            enemies.Add(new Enemy(plane, 1000));

            spriteSheet1 = this.Content.Load<Texture2D>("improved spritesheet1");

            cloud1 = this.Content.Load<Texture2D>("clouds plate 1");
            cloud2 = this.Content.Load<Texture2D>("cloud plate 2");

            titleScreenFont = Content.Load<SpriteFont>("SpriteFont1");
            bigFont = this.Content.Load<SpriteFont>("SpriteFont2");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timer++;
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            // Allows the game to exit
            kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            List<Enemy> eToRmove = new List<Enemy>();
            List<Bullet> bToRmove = new List<Bullet>();

            // TODO: Add your update logic here

            float joyRotation = (float)Math.Atan2(pad1.ThumbSticks.Left.X, pad1.ThumbSticks.Left.Y);

            if (Math.Abs(pad1.ThumbSticks.Left.X) >= .5 || Math.Abs(pad1.ThumbSticks.Left.Y) >= .5)
            {
                float distance = Math.Abs(rotation - joyRotation);

                if (distance > Math.PI)
                    joyRotation += (float)(2 * Math.PI * Math.Sign(rotation - joyRotation));

                rotation = MathHelper.Lerp(rotation, joyRotation, 0.2f);

                rotation = MathHelper.WrapAngle(rotation);
            }

            if (pad1.IsButtonDown(Buttons.RightTrigger))
            {
                if (timer % 5 == 0)
                {
                    bullets.Add(new Bullet(rotation));
                    if (timer % 10 == 0)
                    {
                        bullets.Add(new Bullet(rotation));
                    }
                }
            }


            for (var i = 0; i < bullets.Count; i++)
            {

                bullets[i].update();
                if (bullets[i].rect.X > 800 || bullets[i].rect.X < 0 || bullets[i].rect.Y > 800 || bullets[i].rect.Y < 0)
                {
                    bToRmove.Add(bullets[i]);
                }


            }

            

            
            if (pad1.Buttons.Start == ButtonState.Pressed && oldpad1.Buttons.Start != ButtonState.Pressed && gameState==Status.title) 
            {
                gameState=Status.playLoad;
                loadTime = timer;
            }

            if (timer-loadTime == 60 && loadTime!=0 && gameState!=Status.gameover)
            {
                gameState = Status.play;
                background = new Color(8, 84, 100);
            }

            for (int x = 0; x < rotations.Length - 1; x++)
            {
                if (rotation <= rotations[x] && rotation > rotations[x + 1])
                    currentSprite = shipSource[x];
            }

            /*if(Vector2.Distance(lastTile,new Vector2(ship.X,ship.Y))>=800)
            {
                topClouds.Clear();

                for (int y = ship.Y - 1200; y < 2400; y += 800)
                    for (int x = ship.X - 1200; x < 2400; x += 800)
                        topClouds.Add(new Rectangle(x, y, 800, 800));

                lastTile = new Vector2(ship.X, ship.Y);
            }*/
            double dx = Math.Sin(rotation + -Math.PI);
            double dy = Math.Cos(rotation + -Math.PI);

            for (int x=0;x<topClouds.Count;x++)
            {
                topClouds[x] = new Rectangle(topClouds[x].X + (int)(cloud1vel*dx), topClouds[x].Y - (int)(cloud1vel*dy), topClouds[x].Width, topClouds[x].Height);

                if (topClouds[x].Left < -800)
                    topClouds[x] = new Rectangle(800, topClouds[x].Y, topClouds[x].Width, topClouds[x].Height);

                if (topClouds[x].Right > 1600)
                    topClouds[x] = new Rectangle(-800, topClouds[x].Y, topClouds[x].Width, topClouds[x].Height);

                if (topClouds[x].Top < -800)
                    topClouds[x] = new Rectangle(topClouds[x].X, 800, topClouds[x].Width, topClouds[x].Height);

                if (topClouds[x].Bottom > 1600)
                    topClouds[x] = new Rectangle(topClouds[x].X, -800, topClouds[x].Width, topClouds[x].Height);
            }

            for (int x = 0; x < bottomClouds.Count; x++)
            {
                bottomClouds[x] = new Rectangle(bottomClouds[x].X + (int)(3 * dx), bottomClouds[x].Y - (int)(3 * dy), bottomClouds[x].Width, bottomClouds[x].Height);

                if (bottomClouds[x].Left < -800)
                    bottomClouds[x] = new Rectangle(800, bottomClouds[x].Y, bottomClouds[x].Width, bottomClouds[x].Height);

                if (bottomClouds[x].Right > 1600)
                    bottomClouds[x] = new Rectangle(-800, bottomClouds[x].Y, bottomClouds[x].Width, bottomClouds[x].Height);

                if (bottomClouds[x].Top < -800)
                    bottomClouds[x] = new Rectangle(bottomClouds[x].X, 800, bottomClouds[x].Width, bottomClouds[x].Height);

                if (bottomClouds[x].Bottom > 1600)
                    bottomClouds[x] = new Rectangle(bottomClouds[x].X, -800, bottomClouds[x].Width, bottomClouds[x].Height);
            }


            for (int i = 0; i < enemies.Count; i++)
            {
                /*enemies[i].rect.X += (int)(3 * dx);
                enemies[i].rect.Y += (int)(3 * dy);

                enemies[i].hitbox.X += (int)(3 * dx);
                enemies[i].hitbox.Y += (int)(3 * dy);*/
                enemies[i].update();
                
                if (enemies[i].rect.X < 0 || enemies[i].rect.Y < 0 || enemies[i].rect.Y > 800 || enemies[i].rect.Y > 800)
                {
                    eToRmove.Add(enemies[i]);

                }

                if (enemies[i].hitbox.Intersects(shipHitBox) && gameState!=Status.lostLife && gameState!=Status.title && gameState!=Status.gameover)
                {
                    eToRmove.Add(enemies[i]);

                    hp--;

                    if (hp != 0)
                    {
                        gameState = Status.lostLife;
                        lostLife = timer;
                    }


                    //lose a life
                }
                foreach (Bullet bullet in bullets)
                {

                    if (enemies[i].hitbox.Intersects(bullet.rect) && gameState==Status.play)
                    {
                        bToRmove.Add(bullet);
                        enemies[i].hp--;

                        if(enemies[i].hp<=0)
                        {
                            eToRmove.Add(enemies[i]);
                            score += 100;
                        }
                    }
                }

            }

            foreach (Enemy en in eToRmove)
            {
                enemies.Remove(en);
            }

            foreach (Bullet en in bToRmove)
            {
                bullets.Remove(en);
            }

            if (hp == 0)
                gameState = Status.gameover;

            if (timer - lostLife == 60 && lostLife!=0 && gameState!=Status.gameover)
            {
                gameState = Status.play;
                enemies.Clear();
            }

            if (enemies.Count < 5)
            {
                enemies.Add(new Enemy(plane, 1000));
            }


            oldpad1 = pad1;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if(gameState==Status.title)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    //spriteBatch.Draw(enemies[i].tex, enemies[i].rect, Color.White);
                    //spriteBatch.Draw(debug, enemies[i].hitbox, Color.Red);
                    spriteBatch.Draw(plane, enemies[i].rect, null, Color.Red, enemies[i].rotation, new Vector2(240, 260), new SpriteEffects(), 0);

                }

                foreach (Rectangle clouds in bottomClouds)
                {
                    spriteBatch.Draw(cloud2, clouds, Color.White);
                }

                //spriteBatch.Draw(debug, shipHitBox, Color.Red);
                //spriteBatch.Draw(spriteSheet1, ship, currentSprite, Color.White, 0, new Vector2(41, 42), new SpriteEffects(), 0);

                /*for (var i = 0; i < bullets.Count; i++)
                {
                    //spriteBatch.Draw(projectile, rockets[i], Color.White);
                    spriteBatch.Draw(bulletTex, new Vector2(bullets[i].rect.X, bullets[i].rect.Y), null, Color.White,
                    (float)bullets[i].rotation,
                    new Vector2(12, 12), 0.2f,
                    SpriteEffects.None, 0);
                }*/

                foreach (Rectangle clouds in topClouds)
                {
                    spriteBatch.Draw(cloud1, clouds, Color.White);
                }
            }

            if(gameState==Status.play)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    //spriteBatch.Draw(enemies[i].tex, enemies[i].rect, Color.White);
                    //spriteBatch.Draw(debug, enemies[i].hitbox, Color.Red);
                    spriteBatch.Draw(plane, enemies[i].rect, null, Color.Red, enemies[i].rotation, new Vector2(240, 260), new SpriteEffects(), 0);

                    for (int j = 0; j < enemies[i].bullets.Count; j++)
                    {
                        spriteBatch.Draw(debug, enemies[i].bullets[j], null, Color.White, enemies[i].rotation, new Vector2(240, 260), new SpriteEffects(), 0);

                    }

                }

                foreach (Rectangle clouds in bottomClouds)
                {
                    spriteBatch.Draw(cloud2, clouds, Color.White);
                }

                //spriteBatch.Draw(debug, shipHitBox, Color.Red);
                spriteBatch.Draw(spriteSheet1, ship, currentSprite, Color.White, 0, new Vector2(41, 42), new SpriteEffects(), 0);

                for (var i = 0; i < bullets.Count; i++)
                {
                    //spriteBatch.Draw(projectile, rockets[i], Color.White);
                    spriteBatch.Draw(bulletTex, new Vector2(bullets[i].rect.X, bullets[i].rect.Y), null, Color.White,
                    (float)bullets[i].rotation,
                    new Vector2(12, 12), 0.2f,
                    SpriteEffects.None, 0);
                }

                foreach (Rectangle clouds in topClouds)
                {
                    spriteBatch.Draw(cloud1, clouds, Color.White);
                }
            }

            spriteBatch.End();
            //comment
            spriteBatch.Begin();
            if (gameState == Status.title)
            {
                spriteBatch.DrawString(titleScreenFont, "PLAY", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLAY").Length() / 2, 150), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "1 -- UP", new Vector2(0, 0), Color.Red);
                spriteBatch.DrawString(titleScreenFont, "2 -- UP", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("2 -- UP").Length(), 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "SCORE "+score, new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("SCORE "+score).Length() / 2, 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width / 2 - bigFont.MeasureString("TIME     PILOT").Length() / 2) - 3, 202), Color.DarkOrange, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width / 2 - bigFont.MeasureString("TIME     PILOT").Length() / 2), 200), Color.Yellow, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "PRESS   START   TO   PLAY", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PRESS   START   TO   PLAY").Length() / 2, 300), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "AND   TRY   THIS   GAME", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("AND   TRY   THIS   GAME").Length() / 2, 350), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "CREDIT  00", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("CREDIT  00").Length(), GraphicsDevice.Viewport.Height - 26), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }

            if(gameState==Status.play)
            {
                spriteBatch.Draw(debug, new Rectangle(0, 0, 800, 100), Color.Black);

                spriteBatch.DrawString(titleScreenFont, "SCORE " + score, new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("SCORE " + score).Length() / 2, 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                for (int x = 0; x < hp; x++)
                        spriteBatch.Draw(spriteSheet1, lives[x], shipSource[0], Color.White);
            }

            if (gameState == Status.gameover)
            {
                spriteBatch.DrawString(titleScreenFont, "PLAYER  1", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLAYER  1").Length() / 2, 250), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "GAME  OVER", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("GAME  OVER").Length() / 2, 350), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }

            //spriteBatch.DrawString(titleScreenFont, "PLAY", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLAY").Length() / 2, 150), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "1 -- UP", new Vector2(0, 0), Color.Red);
            //spriteBatch.DrawString(titleScreenFont, "2 -- UP", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("2 -- UP").Length(), 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "SCORE", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("SCORE").Length() / 2, 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("TIME     PILOT").Length() / 2) - 3, 202), Color.DarkOrange, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width  / 2 - titleScreenFont.MeasureString("TIME     PILOT").Length() / 2), 200), Color.Yellow, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "PLEASE   DEPOSIT   COIN", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLEASE   DEPOSIT   COIN").Length() / 2, 250), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "AND   TRY   THIS   GAME", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("AND   TRY   THIS   GAME").Length() / 2, 300), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(titleScreenFont, "CREDIT  00", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("CREDIT  00").Length(), GraphicsDevice.Viewport.Height - 26), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);



            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
