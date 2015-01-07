using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PlanetAid.Entities;

namespace PlanetAid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D railgun;
        private Texture2D flaskImg;
        private Texture2D planetImg;
        private float gunRotation;
        private bool canShoot;
        private bool collision = false;
        private SpriteFont font1;
        Flask flask;
        private Rectangle planetbounds;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
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
            background = Content.Load<Texture2D>("background");
            railgun = Content.Load<Texture2D>("railgun");
            flaskImg = Content.Load<Texture2D>("Flask");
            planetImg = Content.Load<Texture2D>("Planet-1");
            font1 = Content.Load<SpriteFont>("Courier New");
            planetbounds = new Rectangle(700, 200, planetImg.Width, planetImg.Height);
            // TODO: use this.Content to load your game content here
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
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            //Railgun rotation and limitations
            gunRotation = (float)Math.Atan2(Mouse.GetState().Y - 220, Mouse.GetState().X - 10);
            canShoot = true;
            if (gunRotation <= -.523)
            {
                gunRotation = -.523f;
                canShoot = false;
            }


            if (gunRotation >= .174)
            {
                gunRotation = .174f;
                canShoot = false;
            }

            IsMouseVisible = true;
            

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (canShoot && Mouse.GetState().X > 170)
                {
                   
                    flask = new Flask(new Vector2(70, 240), flaskImg);
                }
                
            }
            if (flask != null)
            {
                flask.update();

                // TODO: Add your update logic here
                if (flask.myspace.Intersects(planetbounds))
                {
                    collision = true;
                }
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
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if (collision == false)
            {
                spriteBatch.Draw(railgun, new Rectangle(70, 240, railgun.Width / 4, railgun.Height / 4), null, Color.White, gunRotation, new Vector2(195, 235), SpriteEffects.None, 0);
                spriteBatch.Draw(planetImg, new Rectangle(700, 200, planetImg.Width, planetImg.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                if (flask != null) flask.Draw(spriteBatch);
            }

            else
              spriteBatch.DrawString(font1,"Congratulations",new Vector2(550, 300),Color.BlanchedAlmond);

            
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

    }
}
