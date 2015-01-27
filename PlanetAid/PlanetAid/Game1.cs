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
        private bool wasPressed = false;
        private List<Flask> flaskList;
        private int currentFlask=0;
        private GameState gameState;
        Flask flask;
        Planet planet;
        Planet targetPlanet;
        Gun gun;
        Button playButton, levelButton, quitButton;
        Song menuSong;

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
            targetPlanet = new Planet (Planet.Type.Planet2);
            gun = new Gun();
            IsMouseVisible = true;

            // Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150/2, GraphicsDevice.Viewport.Height / 2, "PlayButton");
            levelButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150 / 2, playButton.bRect.Bottom + 10, "LevelButton");
            quitButton = new Button(GraphicsDevice.Viewport.Width / 2 - 150 / 2, levelButton.bRect.Bottom + 10, "QuitButton");

            flaskList = new List<Flask>();

            Flask newFlask;
            newFlask = new Flask(Flask.Type.Flasky);
            flaskList.Add(newFlask);
            newFlask = new Flask(Flask.Type.FatoFlask);
            flaskList.Add(newFlask);
            newFlask = new Flask(Flask.Type.MonoFlask);
            flaskList.Add(newFlask);


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
            targetPlanet.Load(Content);
            endFont = Content.Load<SpriteFont>("Courier New");
            title = Content.Load<Texture2D>("Title");

            foreach (Flask flask in flaskList)
            {
                flask.Load(Content);
            }

            // Buttons
            playButton.Load(Content);
            levelButton.Load(Content);
            quitButton.Load(Content);

            //Songs
            menuSong = Content.Load<Song>("MenuSong");
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

                if (playButton.clicked && wasPressed==false)
                {
                    MediaPlayer.Stop();
                    wasPressed = true;
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
                // Music
                if (MediaPlayer.State != MediaState.Playing)
                {   
                    MediaPlayer.Volume = 0.02f;
                    MediaPlayer.IsRepeating = true;
                }

                //Shooting
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && wasPressed==false)
                {
                    if (gun.canShoot && Mouse.GetState().X > 170)
                    {
                        flask = flaskList[currentFlask];
                        flask.velocity = Vector2.Zero;
                        flask.direction = new Vector2(Mouse.GetState().X - flask.position.X - (flask.Img.Width / 2), Mouse.GetState().Y - flask.position.Y - (flask.Img.Height / 2));
                        flask.position = new Vector2(150, 290);
                        flask.direction.Normalize();
                        currentFlask++;
                        gun.canShoot = false;
                        if (currentFlask >= 3)
                        {
                            currentFlask = 0;
                        }
                    }
                    wasPressed = true;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && wasPressed == true)
                {
                    wasPressed = false;
                }

                //Collisions and Gravity Fields
                if (flask != null)
                {
                    if (GraphicsDevice.Viewport.Bounds.Contains(flask.myspace) == false)
                        gun.canShoot = true;

                    if (Vector2.Distance(flask.position, planet.position) < 200)
                    {
                        Vector2 direction = new Vector2(planet.myspace.Center.X - flask.myspace.Center.X, planet.myspace.Center.Y - flask.myspace.Center.Y);
                        direction.Normalize();
                        direction *= 5;
                        flask.velocity += direction;
                    }
                    flask.Update(gameTime.ElapsedGameTime);

                    if (Vector2.Distance(new Vector2(flask.myspace.Center.X, flask.myspace.Center.Y), new Vector2(planet.myspace.Center.X, planet.myspace.Center.Y)) < 95)
                    {
                        if (planet.isTarget == true)
                        {
                            collision = true;
                        }
                        
                    }
                }


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
                    targetPlanet.Draw(spriteBatch);
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
