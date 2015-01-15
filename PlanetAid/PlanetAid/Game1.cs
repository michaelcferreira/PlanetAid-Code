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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D SpaceStation;
        private Texture2D flaskImg, MonoFlaskImg, FatoFlaskImg;
        private Texture2D planetImg;
        private float gunRotation;
        private bool canShoot;
        private bool collision = false;
        private SpriteFont endFont;
        Flask flask;
        private Rectangle planetbounds;
        Planet planet;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            planet = new Planet(Planet.Type.Planet1);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            SpaceStation = Content.Load<Texture2D>("SpaceStation");
            planet.Load(Content);
            endFont = Content.Load<SpriteFont>("Courier New");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // Railgun rotation and limitations
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
                    flask = new Flask(Flask.Type.Flasky);
                    flask.Load(Content);
                }
            }
            if (flask != null)
            {
                flask.Update(gameTime.ElapsedGameTime);

                if (flask.myspace.Intersects(planet.myspace))
                {
                    collision = true;
                }
            }

            planet.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            if (collision == false)
            {
                spriteBatch.Draw(SpaceStation, new Rectangle(180, 340, SpaceStation.Width, SpaceStation.Height), null, Color.White, gunRotation, new Vector2(176, 176), SpriteEffects.None, 0);
                planet.Draw(spriteBatch);
                if (flask != null) flask.Draw(spriteBatch);
            }
            else
                spriteBatch.DrawString(endFont, "Congratulations", new Vector2(550, 300), Color.BlanchedAlmond);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
