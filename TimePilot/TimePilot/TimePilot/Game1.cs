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
        SpriteFont debugFont;

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

            ship = new Rectangle(400, 400, 50, 80);
            rotation = 0;

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
            debugFont = this.Content.Load<SpriteFont>("SpriteFont1");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            float padRotation = (float)Math.Atan2(pad1.ThumbSticks.Left.X, pad1.ThumbSticks.Left.Y);

            if (padRotation < 0)
                padRotation += (float)Math.PI * 2;

            if (rotation > Math.PI * 2)
                rotation -= (float)Math.PI * 2;

            float degreesApart = (Math.Abs(MathHelper.ToDegrees(padRotation) - MathHelper.ToDegrees(rotation)));

            

            if ((Math.Abs(pad1.ThumbSticks.Left.X) > .5 || Math.Abs(pad1.ThumbSticks.Left.Y) > .5) && degreesApart>=10) 
            {
                rotation += MathHelper.ToRadians(8);

                if (degreesApart <= 10)
                    rotation = padRotation;
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

            spriteBatch.Draw(debug,ship,ship,Color.Red,rotation,new Vector2(25,40),new SpriteEffects(),0);
            spriteBatch.DrawString(debugFont, rotation + "", new Vector2(600, 600), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
