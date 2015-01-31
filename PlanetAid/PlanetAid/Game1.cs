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
using PlanetAid.Interface;

namespace PlanetAid
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState
        {
            StartMenu,
            LevelMenu,
            Playing,
            GameOver
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameState gameState;
        private Texture2D menu_background;
        private Texture2D play_background;
        private Texture2D pause_menu;
        private bool isPaused = false;
        private bool IsFinished = false;
        private Texture2D menuTitle;
        private Texture2D astronaut;
        private SpriteFont endFont;
        private int currentFlask = 0;
        public List<Level> levelList;
        public Level currentLevel;
        public int n_level = 0;
        Flask flask;
        Gun gun;
        Button playButton, quitButton, pauseButton,
                menuButton, retryButton, nextButton,
                nivel1Button, nivel2Button, nivel3Button,
                nivel4Button, nivel5Button, nivel6Button;
        Song menuSong;
        SoundEffect clickSound;
        SoundEffect crashingSound;
        MouseState oldMouseState;


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
            gun = new Gun();
            IsMouseVisible = true;

            // Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2 + 100, "Buttons/Button_Play");
            quitButton = new Button(GraphicsDevice.Viewport.Width - 115, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Quit");
            pauseButton = new Button(15, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Pause");
            menuButton = new Button(130, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Menu");
            retryButton = new Button(245, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Retry");
            nextButton = new Button(600, 320, "Buttons/Buttons_Next");
            nivel1Button = new Button(340, 200, "Buttons/Button_Level1");
            nivel2Button = new Button(600, 200, "Buttons/Button_Level2");
            nivel3Button = new Button(860, 200, "Buttons/Button_Level3");
            nivel4Button = new Button(340, 350, "Buttons/Button_Level4");
            nivel5Button = new Button(600, 350, "Buttons/Button_Level5");
            nivel6Button = new Button(860, 350, "Buttons/Button_Level6");

            //Adding levels
            levelList = new List<Level>();
            Level level;

            // Level 1
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 40), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 150), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 205), 20));
            //In each level the planet position, the atmosphere radius and the variable
            //that defines if the planet is the target can be modified to the level needs.
            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(640, 350), 190, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 200), 150, true, Color.Blue)); 
                                                                                                                  
            levelList.Add(level);

            // Level 2
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(1000, 290), 200, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), 100, true, Color.Orange));

            levelList.Add(level);

            // Level 3
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, true, Color.Green));

            levelList.Add(level);

            // Level 4
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 300), 250, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 300), 230, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(150, 90), 90, true, Color.Orange));

            levelList.Add(level);

            // Level 5
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, true, Color.White));

            levelList.Add(level);

            // Level 6
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 250), 220, false, Color.White));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, true, Color.White));

            levelList.Add(level);

            //Games starts on Start Menu
            gameState = GameState.StartMenu;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Menu Elements
            menuTitle = Content.Load<Texture2D>("Title");
            menu_background = Content.Load<Texture2D>("MenuBackground");
            playButton.Load(Content);
            quitButton.Load(Content);

            // Level menu Elements
            nivel1Button.Load(Content);
            nivel2Button.Load(Content);
            nivel3Button.Load(Content);
            nivel4Button.Load(Content);
            nivel5Button.Load(Content);
            nivel6Button.Load(Content);

            // Playing Elements
            astronaut = Content.Load<Texture2D>("Shooter/Astronaut");
            play_background = Content.Load<Texture2D>("background");
            gun.Load(Content);
            foreach (Level level in levelList)
                // By loading level.Load we are loading both the flask and the planet load
                level.Load(Content);

            // Pause Elements
            pause_menu = Content.Load<Texture2D>("Pause_Menu");
            pauseButton.Load(Content);

            //GameOver Elements
            endFont = Content.Load<SpriteFont>("Courier New");

            // Songs
            menuSong = Content.Load<Song>("MenuSong");
            clickSound = Content.Load<SoundEffect>("SoundEffects/ClickSound");
            crashingSound = Content.Load<SoundEffect>("SoundEffects/CrashingSound");

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            // Updating the game while in Start Menu
            // When game is at start menu it adds music and checks if we press
            // the buttons. If we do press the buttons it changes the game state
            // and changes other variables.
            if (gameState == GameState.StartMenu)                                                               
            {                                                                                                   
                // Music Properties                                                                             
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(menuSong);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.3f;
                }
                // When pressing the Play Button
                if (playButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                {
                    clickSound.Play();
                    gameState = GameState.LevelMenu;
                    playButton.clicked = false;
                }

                // When pressing the Quit Button
                if (quitButton.clicked)
                    this.Exit();

                playButton.Update();
                quitButton.Update();
            }

            //Updating the game while in Level Menu
            if (gameState == GameState.LevelMenu)
            {
                nivel1Button.Update();
                if (nivel1Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 0;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;

                }
                nivel2Button.Update();
                if (nivel2Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 1;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                }
                nivel3Button.Update();
                if (nivel3Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 2;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                }
                nivel4Button.Update();
                if (nivel4Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 3;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                }
                nivel5Button.Update();
                if (nivel5Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 4;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                }
                nivel6Button.Update();
                if (nivel6Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 5;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                }
            }

            // Updating the game while in Playing, Pause and GameOver
            if (gameState == GameState.Playing)
            {
                if (isPaused == true)
                {
                    if (pauseButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = false;
                        pauseButton.clicked = false;
                    }
                }
                else
                {
                    if (pauseButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = true;
                        pauseButton.clicked = false;
                    }

                    gun.Update(gameTime.ElapsedGameTime);
                    // Current level
                    currentLevel = levelList[n_level];

                    //Shooting
                    // Pressing mouse left click and verifing if some variables are true
                    // shoots the flask by calculating direction vector between mouse position and
                    // flask position and gives him a velocity by multiplying the position w/ the speed.
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        if (gun.canShoot && mouse.X > 0)                                                 
                        {                                                                                           
                            flask = currentLevel.flaskList[currentFlask];
                            flask.position = new Vector2(70, 375);
                            Vector2 direction = new Vector2(mouse.X - flask.position.X,
                                                            mouse.Y - flask.position.Y);
                            direction.Normalize();
                            flask.velocity = direction * flask.speed;
                            currentFlask++;
                            gun.canShoot = false;
                            flask.visible = true;
                            flask.rotatito = 2;
                            if (currentFlask >= 3)
                            {
                                currentFlask = 0;
                            }
                        }
                    }
                    

                    //Collisions and Gravity Fields
                    if (gun.canShoot == false)
                    {
                        //Shoot enabled when flask reaches screen bounds
                        if (GraphicsDevice.Viewport.Bounds.Contains(flask.myspace) == false)
                        {
                            gun.canShoot = true;
                            flask.visible = false;
                        }

                        //For each flask in the list it calculates its update
                        foreach (Flask f in currentLevel.flaskList)
                        {
                            f.Update(gameTime.ElapsedGameTime);
                        }

                        foreach (Planet p in currentLevel.planetList)
                        {
                            //Calculates flask gravity when entering planet atmosphere
                            flask.calculateGravity(p);

                            //When flask colide with any planet
                            if (Vector2.Distance(new Vector2(flask.myspace.Center.X, flask.myspace.Center.Y),
                                                    new Vector2(p.myspace.Center.X, p.myspace.Center.Y)) < p.radius + 15)
                            {
                                flask.visible = false;
                                if (p.isTarget == true) IsFinished = true;
                                gun.canShoot = true;
                                crashingSound.Play();
                            }
                        }
                    }
                    foreach (Planet p in currentLevel.planetList)
                        p.Update(gameTime.ElapsedGameTime);
                }
                if (IsFinished == true)
                {

                }
                pauseButton.Update();

            }


            if (gameState == GameState.GameOver)
            {
                menuButton.Update();
                if (menuButton.clicked)
                {
                    gameState = GameState.LevelMenu;
                }
                retryButton.Update();
                if (retryButton.clicked)
                {

                }
            }
            oldMouseState = mouse;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.Draw(menuTitle, new Rectangle(290, 125, menuTitle.Width
                                                                    , menuTitle.Height), null, Color.White);
                playButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
            }

            if (gameState == GameState.LevelMenu)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                nivel1Button.Draw(spriteBatch);
                nivel2Button.Draw(spriteBatch);
                nivel3Button.Draw(spriteBatch);
                nivel4Button.Draw(spriteBatch);
                nivel5Button.Draw(spriteBatch);
                nivel6Button.Draw(spriteBatch);
            }

            if (gameState == GameState.Playing)
            {
                spriteBatch.Draw(play_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                pauseButton.Draw(spriteBatch);

                foreach (Planet p in currentLevel.planetList)
                    p.Draw(spriteBatch);
                foreach (Flask f in currentLevel.flaskList)
                    f.Draw(spriteBatch);
                if (flask != null) flask.Draw(spriteBatch);
                spriteBatch.Draw(astronaut, new Rectangle(-5, 5, 230, 480), Color.White);
                gun.Draw(spriteBatch);
                if (isPaused == true)
                {
                    spriteBatch.Draw(pause_menu, new Rectangle(400, 200, pause_menu.Width, pause_menu.Height), Color.White);
                }

            }
            if (gameState == GameState.GameOver)
            {

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
