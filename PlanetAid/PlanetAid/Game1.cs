using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PlanetAid.Entities;
using System.Collections.Generic;

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
        public static string checkedPlayerName = "";
        public List<Level> levelList;
        public Level currentLevel;
        GameState gameState;
        bool isPaused = false;      // Sub-Game States
        Texture2D menu_background;  // Fixed Textures
        Texture2D play_background;
        Texture2D pause_menu;
        Texture2D menuTitle;
        Texture2D astronaut;
        SpriteFont font;
        string playerName = "";       // Player Name
        Texture2D crosshair;
        Vector2 crosshairPos;       // Levels
        int currentFlask = 0;
        Song menuSong;                      //Songs & Sound Effects
        SoundEffect clickSound;
        SoundEffect crashingSound;
        SoundEffect flaskSound;
        SoundEffect flaskLostSound;
        Button playButton, quitButton, checkButton,              // Buttons
                pauseButton, previousMenuButton,
                pauseMenuButton, finalRetryButton, gameoverRetryButton,
                pauseResetButton, gameoverMenuButton,
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

            //Games starts on Start Menu
            gameState = GameState.StartMenu;

            // Main Menu Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2 + 100, "Buttons/Button_Play");
            quitButton = new Button(GraphicsDevice.Viewport.Width - 115, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Quit");
            checkButton = new Button(100, 10, "Buttons/Button_Play");

            // Level Select Buttons
            nivel1Button = new Button(340, 200, "Buttons/Button_Level1");
            nivel2Button = new Button(600, 200, "Buttons/Button_Level2");
            nivel3Button = new Button(860, 200, "Buttons/Button_Level3");
            nivel4Button = new Button(340, 350, "Buttons/Button_Level4");
            nivel5Button = new Button(600, 350, "Buttons/Button_Level5");
            nivel6Button = new Button(860, 350, "Buttons/Button_Level6");
            previousMenuButton = new Button(10, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Previews");

            // Playing Buttons
            pauseButton = new Button(15, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Pause");

            // Paused Buttons
            pauseMenuButton = new Button(130, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Menu");
            pauseResetButton = new Button(245, GraphicsDevice.Viewport.Height - 115, "Buttons/Button_Retry");

            // Level finish Buttons
            finalRetryButton = new Button(590, 510, "Buttons/Button_Retry");

            //Game Over Buttons
            gameoverRetryButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2, "Buttons/Button_Retry");
            gameoverMenuButton = new Button(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2, "Buttons/Button_Menu");

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
            font = Content.Load<SpriteFont>("Segoi UI Symbol");
            playButton.Load(Content);
            quitButton.Load(Content);
            checkButton.Load(Content);

            // Level menu Elements
            nivel1Button.Load(Content);
            nivel2Button.Load(Content);
            nivel3Button.Load(Content);
            nivel4Button.Load(Content);
            nivel5Button.Load(Content);
            nivel6Button.Load(Content);
            previousMenuButton.Load(Content);

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
            gameoverRetryButton.Load(Content);
            gameoverMenuButton.Load(Content);

            // Finish Elements
            finalRetryButton.Load(Content);

            // Songs & Effects
            menuSong = Content.Load<Song>("Music/MenuMusic");
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
            KeyboardState newKeyboardState = Keyboard.GetState();
            crosshairPos = new Vector2(mouse.X - 24, mouse.Y - 24);

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
                    MediaPlayer.Volume = 0.1f;
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

                if (checkButton.clicked && oldMouseState.LeftButton == ButtonState.Released)
                {
                    clickSound.Play();
                    checkedPlayerName = playerName;
                    checkButton.clicked = false;
                }

                playButton.Update();
                quitButton.Update();
                checkButton.Update();

                // Player name input
                foreach (Keys k in newKeyboardState.GetPressedKeys())
                {
                    if ((k >= Keys.A && k <= Keys.Z) && (oldKeyboardState.IsKeyUp(k)))
                        playerName += k.ToString();
                    if ((k == Keys.Enter) && (oldKeyboardState.IsKeyUp(k)))
                        checkedPlayerName = playerName;

                    if ((k == Keys.Back) && (oldKeyboardState.IsKeyUp(k)))
                        playerName = playerName.Remove(playerName.Length - 1);
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
                previousMenuButton.Update();

                if (nivel1Button.clicked)
                {
                    InitializeLevel(0);
                    nivel1Button.clicked = false;
                }

                if (nivel2Button.clicked)
                {
                    InitializeLevel(1);
                    nivel2Button.clicked = false;
                }

                if (nivel3Button.clicked)
                {
                    InitializeLevel(2);
                    nivel3Button.clicked = false;
                }

                if (nivel4Button.clicked)
                {
                    InitializeLevel(3);
                    nivel4Button.clicked = false;
                }

                if (nivel5Button.clicked)
                {
                    InitializeLevel(4);
                    nivel5Button.clicked = false;
                }

                if (nivel6Button.clicked)
                {
                    InitializeLevel(5);
                    nivel6Button.clicked = false;
                }

                if (previousMenuButton.clicked)
                {
                    clickSound.Play();
                    gameState = GameState.StartMenu;
                    oldMouseState = mouse;
                    previousMenuButton.clicked = false;
                }
            }

            // Updating the game while in Playing, Pause and GameOver
            if (gameState == GameState.Playing)
            {
                // In Pause Menu
                if (isPaused == true)
                {
                    // Click Pause Button to go back to the playing state
                    if ((pauseButton.clicked) && (oldMouseState.LeftButton == ButtonState.Released))
                    {
                        isPaused = false;
                        pauseButton.clicked = false;

                    }

                    // Click Menu Button to go back to level menu
                    if ((pauseMenuButton.clicked) && (oldMouseState.LeftButton == ButtonState.Released))
                    {
                        isPaused = false;
                        gameState = GameState.LevelMenu;
                        pauseMenuButton.clicked = false;

                    }

                    // It checks if the retry button is clicked to reset the level
                    if ((pauseResetButton.clicked) && (oldMouseState.LeftButton == ButtonState.Released))
                    {
                        isPaused = false;
                        pauseResetButton.clicked = false;
                        InitializeLevel(Level.n_level);
                    }
                }
                else if (currentLevel.IsFinished == true)
                {
                    finalRetryButton.Update();
                    currentLevel.UpdateFinish();

                    if ((Level.n_level < 0) || (Level.n_level > levelList.Count-1))
                    {
                        gameState = GameState.LevelMenu;
                        MediaPlayer.Play(menuSong);
                    }
                    else if (currentLevel.IsFinished == false)
                            InitializeLevel(Level.n_level);

                    // It checks if the retry button is clicked to reset the level
                    if (finalRetryButton.clicked)
                    {
                        isPaused = false;
                        finalRetryButton.clicked = false;
                        InitializeLevel(Level.n_level);
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


                    //Shooting
                    // Pressing mouse left click and verifing if some variables are true
                    // shoots the flask by calculating direction vector between mouse position and
                    // flask position and gives him a velocity by multiplying the position w/ the speed.
                    if ((mouse.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton == ButtonState.Released))
                    {
                        if ((gun.canShoot) && (mouse.X > 200))
                        {
                            flaskSound.Play(0.3f, 0.6f, 0);
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
                            currentLevel.attemptScore -= 10000;
                        }
                    }


                    //Collisions and Gravity Fields
                    if ((gun.canShoot == false) && (flask != null))
                    {
                        //Shoot enabled when flask reaches screen bounds
                        if (GraphicsDevice.Viewport.Bounds.Contains(flask.myspace) == false)
                        {
                            flaskLostSound.Play(0.8f, 0.3f, 0);
                            currentLevel.orbitingScore = 0;
                            gun.canShoot = true;
                            flask.visible = false;
                            if (currentFlask >= 4)
                            {
                                if (currentLevel.IsFinished == false)
                                    gameState = GameState.GameOver;

                                currentFlask = 0;
                            }
                        }

                        //For each flask in the list it calculates its update
                        foreach (Flask f in currentLevel.flaskList)
                        {
                            f.Update(gameTime.ElapsedGameTime);
                        }
                        //For each planet in the list
                        foreach (Planet p in currentLevel.planetList)
                        {
                            //Calculates flask gravity when entering planet atmosphere
                            if (flask.calculateGravity(p))
                            {
                                currentLevel.orbitingScore += 100;
                            }

                            //When flask colide with any planet
                            if (Vector2.Distance(new Vector2(flask.myspace.Center.X, flask.myspace.Center.Y),
                                                    new Vector2(p.myspace.Center.X, p.myspace.Center.Y)) < p.radius + 15)
                            {
                                if (p.isTarget == true)
                                    currentLevel.IsFinished = true;
                                else
                                {
                                    currentLevel.orbitingScore = 0;
                                    flask.visible = false;
                                    gun.canShoot = true;
                                    crashingSound.Play();
                                }
                                // If the fourth flask colides with any planet, despite the target, its GameOver
                                if (currentFlask >= 4)
                                {
                                    if (currentLevel.IsFinished == false)
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
            }


            if (gameState == GameState.GameOver)
            {
                gameoverMenuButton.Update();
                if (gameoverMenuButton.clicked)
                {
                    gameState = GameState.LevelMenu;
                    gameoverMenuButton.clicked = false;
                }
                gameoverRetryButton.Update();
                if (gameoverRetryButton.clicked)
                {
                    gameState = GameState.Playing;
                    InitializeLevel(Level.n_level);
                    gameoverRetryButton.clicked = false;
                }
            }
            oldMouseState = mouse;
            oldKeyboardState = Keyboard.GetState();
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
                checkButton.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);

                if (checkedPlayerName == "")
                    spriteBatch.DrawString(font, playerName, new Vector2(15, 15), Color.LightGreen);
                else spriteBatch.DrawString(font, checkedPlayerName, new Vector2(1000, 15), Color.LightGreen);
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
                previousMenuButton.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
            }

            if (gameState == GameState.Playing)
            {
                spriteBatch.Draw(play_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                pauseButton.Draw(spriteBatch);

                // Draws score
                //string Score= attemptScore;

                spriteBatch.DrawString(font, "Score: " + currentLevel.orbitingScore.ToString(), new Vector2(15, 500), Color.White);
                // Draws planets on screeen
                foreach (Planet p in currentLevel.planetList)
                    p.Draw(spriteBatch);

                // Draws flasks on screeen
                foreach (Flask f in currentLevel.flaskList)
                    f.Draw(spriteBatch);

                spriteBatch.Draw(astronaut, new Rectangle(-5, 5, 230, 480), Color.White);

                // Draws shooted flask
                if (flask != null)
                    flask.Draw(spriteBatch);

                gun.Draw(spriteBatch);

                if (isPaused == true)
                {
                    //spriteBatch.Draw(pause_menu, new Rectangle(400, 200, pause_menu.Width, pause_menu.Height), Color.White);
                    pauseMenuButton.Draw(spriteBatch);
                    pauseResetButton.Draw(spriteBatch);
                }
                if (currentLevel.IsFinished == true)
                {
                    currentLevel.DrawFinish(spriteBatch);
                    finalRetryButton.Draw(spriteBatch);
                }
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
            }
            if (gameState == GameState.GameOver)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                gameoverMenuButton.Draw(spriteBatch);
                gameoverRetryButton.Draw(spriteBatch);
                spriteBatch.Draw(crosshair, crosshairPos, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void InitializeLevel(int lvl)
        {
            levelList = Level.CreateLevelsList();

            // By loading level.Load we are loading both the flask and the planet load
            foreach (Level l in levelList)
                l.Load(Content);

            currentFlask = 0;
            gun.canShoot = true;
            isPaused = false;

            currentLevel = levelList[lvl];
            Level.n_level = lvl;
            if (flask != null) flask.visible = false;

            MediaPlayer.Stop();
            clickSound.Play();
            oldMouseState = Mouse.GetState();
            gameState = GameState.Playing;
        }
    }
}
