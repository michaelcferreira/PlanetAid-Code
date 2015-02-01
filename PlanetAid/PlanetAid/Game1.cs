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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        enum GameState                      // Game States
        {
            StartMenu,
            LevelMenu,
            Playing,
            GameOver
        }
        private GameState gameState;
        private bool isPaused = false;      // Sub-Game States
        private bool IsFinished = false;
        private Texture2D menu_background;  // Fixed Textures
        private Texture2D play_background;
        private Texture2D pause_menu;
        private Texture2D menuTitle;
        private Texture2D astronaut;
        private SpriteFont nameFont;      // Player Name
        private Vector2 fontPos;
        private string playerName;
        private Texture2D crosshair;
        private Vector2 crosshairPos;
        private int currentFlask = 0;
        public List<Level> levelList;
        public Level currentLevel;
        public int n_level = 0;
        Song menuSong;                      //Songs & Sound Effects
        SoundEffect clickSound;
        SoundEffect crashingSound;
        SoundEffect flaskSound;
        SoundEffect flaskLostSound;
        Button playButton, quitButton,              // Buttons
                pauseButton, previewsMenuButton,
                previewsButton, pauseMenuButton,
                pauseResetButton, overMenuButton,
                retryButton, nextButton,
                nivel1Button, nivel2Button,
                nivel3Button, nivel4Button,
                nivel5Button, nivel6Button;
        Flask flask;
        Gun gun;
        MouseState oldMouseState;
        KeyboardState oldKeyboardState;
     

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
            IsMouseVisible = false;
            oldKeyboardState = Keyboard.GetState();

            //Games starts on Start Menu
            gameState = GameState.StartMenu;

            // Main Menu Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2 + 100, "Buttons/Button_Play");
            quitButton = new Button(GraphicsDevice.Viewport.Width - 115, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Quit");

            // Level Select Buttons
            nivel1Button = new Button(340, 200, "Buttons/Button_Level1");
            nivel2Button = new Button(600, 200, "Buttons/Button_Level2");
            nivel3Button = new Button(860, 200, "Buttons/Button_Level3");
            nivel4Button = new Button(340, 350, "Buttons/Button_Level4");
            nivel5Button = new Button(600, 350, "Buttons/Button_Level5");
            nivel6Button = new Button(860, 350, "Buttons/Button_Level6");
            previewsMenuButton = new Button(10, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Previews");

            // Playing Buttons
            pauseButton = new Button(15, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Pause");

            // Paused Buttons
            pauseMenuButton = new Button(130, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Menu");
            pauseResetButton = new Button(245, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Retry");

            //Game Over Buttons
            retryButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2, "Buttons/Button_Retry");
            overMenuButton = new Button(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height /2, "Buttons/Button_Menu");

            // Level Passed Buttons
            nextButton = new Button(690, 320, "Buttons/Button_Next");
            previewsButton = new Button(490, 320, "Buttons/Button_Previews");

            CreateLevels();
            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            crosshair = Content.Load<Texture2D>("crosshair");

            // Menu Elements
            menuTitle = Content.Load<Texture2D>("Title");
            menu_background = Content.Load<Texture2D>("MenuBackground");
            nameFont = Content.Load<SpriteFont>("Segoi UI Symbol");
            fontPos = new Vector2(15,15);
            playButton.Load(Content);
            quitButton.Load(Content);

            // Level menu Elements
            nivel1Button.Load(Content);
            nivel2Button.Load(Content);
            nivel3Button.Load(Content);
            nivel4Button.Load(Content);
            nivel5Button.Load(Content);
            nivel6Button.Load(Content);
            previewsMenuButton.Load(Content);

            // Playing Elements
            astronaut = Content.Load<Texture2D>("Shooter/Astronaut");
            play_background = Content.Load<Texture2D>("background");
            gun.Load(Content);

            // Pause Elements
            pause_menu = Content.Load<Texture2D>("Pause_Menu");
            pauseButton.Load(Content);
            pauseMenuButton.Load(Content);
            pauseResetButton.Load(Content);

            //GameOver Elements
            retryButton.Load(Content);
            overMenuButton.Load(Content);

            // Level Completed Elements
            nextButton.Load(Content);
            previewsButton.Load(Content);

            // Songs & Effects
            menuSong = Content.Load<Song>("MenuSong");
            clickSound = Content.Load<SoundEffect>("SoundEffects/ClickSound");
            crashingSound = Content.Load<SoundEffect>("SoundEffects/CrashingSound");
            flaskSound = Content.Load<SoundEffect>("SoundEffects/FlaskSound");
            flaskLostSound = Content.Load<SoundEffect>("SoundEffects/FlaskLostSound");

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            crosshairPos = new Vector2(mouse.X-24, mouse.Y-24);

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

                // KEYBOARD MADNESS _____________
                KeyboardState newKeyboardState = Keyboard.GetState();

                if (newKeyboardState.IsKeyDown(Keys.A))
                {
                    // If not down last update, key has just been pressed.
                    if (!oldKeyboardState.IsKeyDown(Keys.A))
                    {
                        playerName = "A";
                    }
                }
                else if (oldKeyboardState.IsKeyDown(Keys.A))
                {
                    // Key was down last update, but not down now, so
                    // it has just been released.
                }
            }

            // Updating the game while in LEVEL MENU and so it means that it is testing
            // if the buttons are clicked and if so it changes the game state and the level
            if (gameState == GameState.LevelMenu)
            {
                nivel1Button.Update();
                nivel2Button.Update();
                nivel3Button.Update();
                nivel4Button.Update();
                nivel5Button.Update();
                nivel6Button.Update();
                previewsMenuButton.Update();

                if (nivel1Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 0;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel1Button.clicked = false;
                    CreateLevels();

                }
                if (nivel2Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 1;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel2Button.clicked = false;
                    CreateLevels();
                }
                if (nivel3Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 2;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel3Button.clicked = false;
                    CreateLevels();
                }
                if (nivel4Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 3;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel4Button.clicked = false;
                    CreateLevels();
                }
                if (nivel5Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 4;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel5Button.clicked = false;
                    CreateLevels();
                }
                if (nivel6Button.clicked)
                {
                    MediaPlayer.Stop();
                    clickSound.Play();
                    n_level = 5;
                    gameState = GameState.Playing;
                    oldMouseState = mouse;
                    nivel6Button.clicked = false;
                    CreateLevels();
                }
                if (previewsMenuButton.clicked)
                {
                    clickSound.Play();
                    gameState = GameState.StartMenu;
                    oldMouseState = mouse;
                    previewsMenuButton.clicked = false;
                }
            }

            // Updating the game while in Playing, Pause and GameOver
            if (gameState == GameState.Playing)
            {
                // In Pause Menu
                if (isPaused == true)
                {
                    // It checks if the pause is clicked to go back to the playing state
                    if (pauseButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = false;
                        pauseButton.clicked = false;

                    }

                    // It checks if the menu button is clicked to go back to level menu
                    if (pauseMenuButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = false;
                        gameState = GameState.LevelMenu;
                        pauseMenuButton.clicked = false;

                    }

                    // It checks if the retry button is clicked to reset the level
                    if (pauseResetButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = false;
                        pauseResetButton.clicked = false;
                        CreateLevels();

                        
                    }
                }
                else if (IsFinished == true)
                {
                    if (nextButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        n_level++;
                        CreateLevels();
                        nextButton.clicked = false;
                        IsFinished = false;
                    }
                    if (previewsButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        n_level--;
                        CreateLevels();
                        previewsButton.clicked = false;
                        IsFinished = false;
                    }
                }
                // In Playing Mode
                else
                {
                    // It checks if the pause is clicked to go to the pause menu
                    if (pauseButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        isPaused = true;
                        pauseButton.clicked = false;

                    }
                    
                    // Current level
                    currentLevel = levelList[n_level];


                    //Shooting
                    // Pressing mouse left click and verifing if some variables are true
                    // shoots the flask by calculating direction vector between mouse position and
                    // flask position and gives him a velocity by multiplying the position w/ the speed.
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    {
                        if (gun.canShoot && mouse.X > 200)
                        {
                            flaskSound.Play(1, 0.6f, 0);
                            flask = currentLevel.flaskList[currentFlask];
                            flask.position = new Vector2(70, 375);
                            Vector2 direction = new Vector2(mouse.X - flask.position.X,
                                                            mouse.Y - flask.position.Y);
                            direction.Normalize();
                            flask.velocity = direction * flask.speed;
                            currentFlask++;
                            gun.canShoot = false;
                            flask.visible = true;
                            flask.idleRotation = 2;
                            
                        }
                    }


                    //Collisions and Gravity Fields
                    if (gun.canShoot == false && flask!=null)
                    {
                        //Shoot enabled when flask reaches screen bounds
                        if (GraphicsDevice.Viewport.Bounds.Contains(flask.myspace) == false)
                        {
                            flaskLostSound.Play(1, 0.3f, 0);
                            gun.canShoot = true;
                            flask.visible = false;
                            if (currentFlask >= 4)
                            {
                                if (IsFinished == false)
                                    gameState = GameState.GameOver;

                                currentFlask = 0;
                            }
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

                                // If the fourth flask colides with any planet despite the target its GameOver
                                if (currentFlask >= 4)
                                {
                                    if (IsFinished == false)
                                        gameState = GameState.GameOver;

                                    currentFlask = 0;
                                }
                            }
                        }
                    }
                    foreach (Planet p in currentLevel.planetList)
                        p.Update(gameTime.ElapsedGameTime);
                    gun.Update(gameTime.ElapsedGameTime);
                }

               
                pauseButton.Update();
                pauseMenuButton.Update();
                pauseResetButton.Update();
                nextButton.Update();
                previewsButton.Update();
            }


            if (gameState == GameState.GameOver)
            {
                overMenuButton.Update();
                if (overMenuButton.clicked)
                {
                    gameState = GameState.LevelMenu;
                    overMenuButton.clicked = false;
                }
                retryButton.Update();
                if (retryButton.clicked)
                {
                    gameState = GameState.Playing;
                    CreateLevels();
                    retryButton.clicked = false;
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
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
                KeyboardState newKeyboardState = Keyboard.GetState();
                if (newKeyboardState.IsKeyDown(Keys.A))
                {
              
                    if (!oldKeyboardState.IsKeyDown(Keys.A))
                    {
                        string output = playerName;
                        spriteBatch.DrawString(nameFont, output, fontPos, Color.LightGreen, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                    }
                }
                else if (oldKeyboardState.IsKeyDown(Keys.A))
                {


                }
                
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
                previewsMenuButton.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
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
                    //spriteBatch.Draw(pause_menu, new Rectangle(400, 200, pause_menu.Width, pause_menu.Height), Color.White);
                    pauseMenuButton.Draw(spriteBatch);
                    pauseResetButton.Draw(spriteBatch);
                }
                if (IsFinished == true)
                {
                    nextButton.Draw(spriteBatch);
                    previewsButton.Draw(spriteBatch);
                }
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
            }
            if (gameState == GameState.GameOver)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                overMenuButton.Draw(spriteBatch);
                retryButton.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void CreateLevels()
        {
            //Adding levels
            levelList = new List<Level>();
            Level level;

            // Level 1
            level = new Level();

            // In each level the flasks position is always the same but the flask type
            // is customisable to level needs.
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 40), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 150), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 205), 20));
            // In each level the planet position, the atmosphere radius and the variable
            // that defines if the planet is the target can be modified to the level needs.
            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(640, 350), 190, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 200), 150, false, true));

            levelList.Add(level);

            // Level 2
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(1000, 290), 200, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), 100,false, true));

            levelList.Add(level);

            // Level 3
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(500, 450), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(850, 230), 200, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1100, 600), 100, false, true));

            levelList.Add(level);

            // Level 4
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 300), 250, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 300), 230, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(150, 90), 90, false, true));

            levelList.Add(level);

            // Level 5
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(450, 50), 220, true));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 250), 220, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, false, true));

            levelList.Add(level);

            // Level 6
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(10, 10), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 250), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 250), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, false, true));

            levelList.Add(level);

            // By loading level.Load we are loading both the flask and the planet load
            foreach (Level l in levelList)
                 l.Load(Content);

            currentFlask = 0;
            gun.canShoot = true;
            if (flask != null) flask.visible = false;
        }
    }
}
