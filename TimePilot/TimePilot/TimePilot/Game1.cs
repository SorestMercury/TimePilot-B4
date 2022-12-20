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
        Texture2D spriteSheet1;
        Rectangle[] shipSource;
        Rectangle currentSprite;
        float[] rotations;
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
            debug = this.Content.Load<Texture2D>("debug");
            bulletTex = this.Content.Load<Texture2D>("square");
            spriteSheet1 = this.Content.Load<Texture2D>("improved spritesheet1");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);

            float joyRotation = (float)Math.Atan2(pad1.ThumbSticks.Left.X, pad1.ThumbSticks.Left.Y);

            if(Math.Abs(pad1.ThumbSticks.Left.X )>= .5 || Math.Abs(pad1.ThumbSticks.Left.Y )>= .5)
            {
                float distance = Math.Abs(rotation - joyRotation);

                if (distance > Math.PI)
                    joyRotation += (float)(2 * Math.PI * Math.Sign(rotation - joyRotation));

                rotation = MathHelper.Lerp(rotation, joyRotation, 0.2f);

                rotation = MathHelper.WrapAngle(rotation);
            }

            if (pad1.IsButtonDown(Buttons.RightTrigger))
            {
                if (timer % 10 == 0)
                { 
                    bullets.Add(new Bullet(rotation)); 
                }
            }


            for (var i = 0; i < bullets.Count; i++)
            {

                bullets[i].update();
                if (bullets[i].rect.X > 800 || bullets[i].rect.X < 0 || bullets[i].rect.Y > 800 || bullets[i].rect.Y < 0)
                {
                    bullets.Remove(bullets[i]);
                }


            }

            for (int x = 0; x < rotations.Length-1; x++)
            {
                if (rotation <= rotations[x] && rotation > rotations[x + 1])
                    currentSprite = shipSource[x];
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(spriteSheet1,ship,currentSprite,Color.White,0,new Vector2(25,40),new SpriteEffects(),0);

            for (var i = 0; i < bullets.Count; i++)
            {
                //spriteBatch.Draw(projectile, rockets[i], Color.White);
                spriteBatch.Draw(bulletTex, new Vector2(bullets[i].rect.X, bullets[i].rect.Y), null, Color.White,
                (float)bullets[i].rotation,
                new Vector2(5, 5), 0.2f,
                SpriteEffects.None, 0);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
