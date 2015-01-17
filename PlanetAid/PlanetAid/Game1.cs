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
        enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D title;
        private bool collision = false;
        private SpriteFont endFont;
        Flask flask;
        Planet planet;
        Gun gun;
        private GameState gameState;
        Button playButton, levelButton, quitButton;
        Song menuSong;
        Song playSong;

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
            gun = new Gun();
            IsMouseVisible = true;

            // Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150/2, GraphicsDevice.Viewport.Height / 2, "PlayButton");
            levelButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150 / 2, playButton.bRect.Bottom + 10, "LevelButton");
            quitButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150 / 2, levelButton.bRect.Bottom + 10, "QuitButton");

            gameState = GameState.StartMenu;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            gun.Load(Content);
            planet.Load(Content);
            endFont = Content.Load<SpriteFont>("Courier New");
            title = Content.Load<Texture2D>("Title");

            // Buttons
            playButton.Load(Content);
            levelButton.Load(Content);
            quitButton.Load(Content);

            //Songs
            menuSong = Content.Load<Song>("MenuSong");
            playSong = Content.Load<Song>("PlaySong");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameState.StartMenu)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(menuSong);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.3f;
                }
                playButton.Update();
                levelButton.Update();
                quitButton.Update();

                if (playButton.clicked)
                {
                    MediaPlayer.Stop();
                    gameState = GameState.Playing;
                }
                if (quitButton.clicked)
                    this.Exit();
            }
            if (gameState == GameState.Paused)
            {

            }

            if (gameState == GameState.Playing)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(playSong);
                    MediaPlayer.Volume = 0.02f;
                    MediaPlayer.IsRepeating = true;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (gun.canShoot && Mouse.GetState().X > 170)
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

                //Vector2 distance = new Vector2(planet.origin.X-flask.origin.X,planet.origin.Y-flask.origin.Y);
               // Vector2 length = distance;

                gun.Update(gameTime.ElapsedGameTime);
                planet.Update(gameTime.ElapsedGameTime);
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                playButton.Draw(spriteBatch);
                levelButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
                spriteBatch.Draw(title, new Rectangle(640, 200, 700, 151),null, Color.White,0,new Vector2(350, 75),SpriteEffects.None,0);
            }
            if (gameState == GameState.Paused)
            {

            }

            if (gameState == GameState.Playing)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                if (collision == false)
                {
                    gun.Draw(spriteBatch);
                    planet.Draw(spriteBatch);
                    if (flask != null) flask.Draw(spriteBatch);
                }
                else
                    spriteBatch.DrawString(endFont, "Congratulations", new Vector2(550, 300), Color.BlanchedAlmond);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
