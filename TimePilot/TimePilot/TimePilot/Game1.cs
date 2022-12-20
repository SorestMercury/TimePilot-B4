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
        SpriteFont titleScreenFont;
        enum Status { title, ranks, play, lvl1, lvl2, lvl3, lvl5, lvl6, gameover, endscreen}
        Texture2D[] titleScreens;
        Status s;
        KeyboardState kb;
        GamePadState oldpad1 = GamePad.GetState(PlayerIndex.One);
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

            s = Status.title;
            titleScreens = new Texture2D[10];

            

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

            titleScreens[0] = this.Content.Load<Texture2D>("titlescreen");
            titleScreens[1] = this.Content.Load<Texture2D>("titlescores");
            titleScreens[2] = this.Content.Load<Texture2D>("lvl1s");
            titleScreens[3] = this.Content.Load<Texture2D>("lvl2");
            titleScreens[4] = this.Content.Load<Texture2D>("lvl3");
            titleScreens[5] = this.Content.Load<Texture2D>("lvl4");
            titleScreens[6] = this.Content.Load<Texture2D>("lvl5");
            titleScreens[7] = this.Content.Load<Texture2D>("lvl6");
            titleScreens[8] = this.Content.Load<Texture2D>("gameover");
            titleScreens[9] = this.Content.Load<Texture2D>("end");
            titleScreenFont = Content.Load<SpriteFont>("SpriteFont1");
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
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            // Allows the game to exit
            kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (pad1.Buttons.Start == ButtonState.Pressed && oldpad1.Buttons.Start != ButtonState.Pressed && (int)s < 9) 
            {
                s++;
            }
            if (kb.IsKeyDown(Keys.Space))
            {
                s++;
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            //comment
            spriteBatch.Begin();
            if (s== 0 )
            {
                spriteBatch.DrawString(titleScreenFont, "PLAY", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLAY").Length() / 2, 150), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "1 -- UP", new Vector2(0, 0), Color.Red);
                spriteBatch.DrawString(titleScreenFont, "2 -- UP", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("2 -- UP").Length(), 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "SCORE", new Vector2(GraphicsDevice.Viewport.Wid-th / 2 - titleScreenFont.MeasureString("SCORE").Length() / 2, 0), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("TIME     PILOT").Length() / 2) - 3, 202), Color.DarkOrange, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "TIME     PILOT", new Vector2((GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("TIME     PILOT").Length() / 2), 200), Color.Yellow, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "PLEASE   DEPOSIT   COIN", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("PLEASE   DEPOSIT   COIN").Length() / 2, 250), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "AND   TRY   THIS   GAME", new Vector2(GraphicsDevice.Viewport.Width / 2 - titleScreenFont.MeasureString("AND   TRY   THIS   GAME").Length() / 2, 300), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(titleScreenFont, "CREDIT  00", new Vector2(GraphicsDevice.Viewport.Width - titleScreenFont.MeasureString("CREDIT  00").Length(), GraphicsDevice.Viewport.Height - 26), Color.DeepSkyBlue, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
            if (s != 0)
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
