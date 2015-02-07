using InputManager;
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
        enum GameState
        {
            StartMenu,
            LevelMenu,
            Playing,
            GameOver
        }
        public static string checkedPlayerName = "";
        public List<Level> levelList;
        public Level currentLevel;  
        string playerName = "";     
        // Sub-Game States
        bool isPlaying = false;
        bool isPaused = false;
        bool levelMenu1 = false;
        bool levelMenu2= false;
        int currentFlask = 0;
        GameState gameState;
        Texture2D menu_background, play_background;
        Texture2D menuPlanetBackground;
        Texture2D menuTitle;
        Texture2D astronaut;
        Texture2D playerNameHUD, pauseHUD,gameoverHUD, finishHUD;
        Texture2D purpleDot;
        Texture2D cursor;
        Vector2 cursorPos;          
        // Display of current level while playing
        Texture2D   lvl1, lvl2, lvl3, 
                    lvl4, lvl5, lvl6,
                    lvl7, lvl8, lvl9,
                    lvl10, lvl11, lvl12;
        SpriteFont font;
        Song menuSong;                      
        SoundEffect clickSound;
        SoundEffect crashingSound;
        SoundEffect flaskSound;
        SoundEffect flaskLostSound;
        Button playButton, quitButton, checkButton,
                pauseButton, previousMenuButton, nextMenuButton,
                pauseMenuButton, finalRetryButton, gameoverRetryButton,
                pauseRestartButton, pauseResumeButton, gameoverMenuButton,
                nivel1Button, nivel2Button,
                nivel3Button, nivel4Button,
                nivel5Button, nivel6Button,
                nivel7Button, nivel8Button,
                nivel9Button, nivel10Button,
                nivel11Button, nivel12Button;
        Flask flask;
        Gun gun;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            Components.Add(new Input(this));
        }

        protected override void Initialize()
        {
            gun = new Gun();
            IsMouseVisible = false;

            //Games starts on Start Menu
            gameState = GameState.StartMenu;

            // Main Menu Buttons
            playButton = new Button(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2 + 100, 100, 100, "Buttons/Button_Next");
            quitButton = new Button(GraphicsDevice.Viewport.Width - 115, GraphicsDevice.Viewport.Height - 115, 100, 100, "Buttons/Button_Quit");
            checkButton = new Button(195, 328, 40, 40, "Buttons/Button_Check");

            // Level Select Buttons
            nivel1Button = new Button(340, 200, 100, 100, "Buttons/Button_Level1");
            nivel2Button = new Button(600, 200, 100, 100, "Buttons/Button_Level2");
            nivel3Button = new Button(860, 200, 100, 100, "Buttons/Button_Level3");
            nivel4Button = new Button(340, 350, 100, 100, "Buttons/Button_Level4");
            nivel5Button = new Button(600, 350, 100, 100, "Buttons/Button_Level5");
            nivel6Button = new Button(860, 350, 100, 100, "Buttons/Button_Level6");
            nivel7Button = new Button(340, 200, 100, 100, "Buttons/Button_Level7");
            nivel8Button = new Button(600, 200, 100, 100, "Buttons/Button_Level8");
            nivel9Button = new Button(860, 200, 100, 100, "Buttons/Button_Level9");
            nivel10Button = new Button(340, 350, 100, 100, "Buttons/Button_Level10");
            nivel11Button = new Button(600, 350, 100, 100, "Buttons/Button_Level11");
            nivel12Button = new Button(860, 350, 100, 100, "Buttons/Button_Level12");
            previousMenuButton = new Button(10, GraphicsDevice.Viewport.Height - 115, 100, 100, "Buttons/Button_PreviewMenu");
            nextMenuButton = new Button(GraphicsDevice.Viewport.Width - 110, GraphicsDevice.Viewport.Height - 115, 100, 100, "Buttons/Button_Next");

            // Playing Buttons
            pauseButton = new Button(15, GraphicsDevice.Viewport.Height - 115, 100, 100, "Buttons/Button_Pause");

            // Paused Buttons
            pauseResumeButton = new Button(470, 260, 350, 100, "Buttons/Button_Resume");
            pauseRestartButton = new Button(470, 380, 350, 100, "Buttons/Button_Restart");
            pauseMenuButton = new Button(470, 500, 350, 100, "Buttons/Button_LevelMenu");

            // Level finish Buttons
            finalRetryButton = new Button(590, 610, 100, 100, "Buttons/Button_Retry");

            //Game Over Buttons
            gameoverRetryButton = new Button(470, 260, 350, 100, "Buttons/Button_GameOverRetry");
            gameoverMenuButton = new Button(470, 500, 350, 100, "Buttons/Button_LevelMenu");

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cursor = Content.Load<Texture2D>("cursor");

            // Menu Elements
            menuTitle = Content.Load<Texture2D>("Title");
            menu_background = Content.Load<Texture2D>("MenuBackground");
            menuPlanetBackground = Content.Load<Texture2D>("MenuPlanetBackground");
            playerNameHUD = Content.Load<Texture2D>("HUD/HUD_PlayerName");
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
            nivel7Button.Load(Content);
            nivel8Button.Load(Content);
            nivel9Button.Load(Content);
            nivel10Button.Load(Content);
            nivel11Button.Load(Content);
            nivel12Button.Load(Content);
            previousMenuButton.Load(Content);
            nextMenuButton.Load(Content);

            // Playing Elements
            play_background = Content.Load<Texture2D>("background");
            purpleDot = Content.Load<Texture2D>("purpledot");
            astronaut = Content.Load<Texture2D>("Shooter/Astronaut");
            gun.Load(Content);
            lvl1 = Content.Load<Texture2D>("HUD/Level1");
            lvl2 = Content.Load<Texture2D>("HUD/Level2");
            lvl3 = Content.Load<Texture2D>("HUD/Level3");
            lvl4 = Content.Load<Texture2D>("HUD/Level4");
            lvl5 = Content.Load<Texture2D>("HUD/Level5");
            lvl6 = Content.Load<Texture2D>("HUD/Level6");


            // Pause Elements
            pauseHUD = Content.Load<Texture2D>("HUD/HUD_Pause");
            pauseButton.Load(Content);
            pauseResumeButton.Load(Content);
            pauseMenuButton.Load(Content);
            pauseRestartButton.Load(Content);

            //GameOver Elements
            gameoverHUD = Content.Load<Texture2D>("HUD/HUD_GameOver");
            gameoverRetryButton.Load(Content);
            gameoverMenuButton.Load(Content);

            // Finish Elements
            finalRetryButton.Load(Content);
            finishHUD = Content.Load<Texture2D>("HUD/HUD_Finish");

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
            cursorPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

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
                if (playButton.clicked)
                {
                    clickSound.Play();
                    levelMenu1=true;
                    gameState = GameState.LevelMenu;
                    playButton.clicked = false;
                }

                // When pressing the Quit Button
                if (quitButton.clicked)
                    this.Exit();

                // When pressing name check button
                if (checkButton.clicked)
                {
                    clickSound.Play();
                    checkedPlayerName = playerName;
                    checkButton.clicked = false;
                }

                playButton.Update();
                quitButton.Update();
                checkButton.Update();

                // Player name input
                foreach (Keys k in Keyboard.GetState().GetPressedKeys())
                {
                    if ((k >= Keys.A && k <= Keys.Z) && Input.IsKeyReleased(k))
                        playerName += k.ToString();
                    if ((k == Keys.Enter) && Input.IsKeyReleased(k))
                        checkedPlayerName = playerName;

                    if ((k == Keys.Back) && Input.IsKeyReleased(k))
                        {
                            if (playerName.Length < 0)
                                playerName += "";
                            else playerName = playerName.Remove(playerName.Length - 1);
                        }
                }
            }

            // Updating the game while in LEVEL MENU and so it means that it is testing
            // if the buttons are clicked and if so it changes the game state and the level.
            if (gameState == GameState.LevelMenu)
            {
                if (levelMenu1==true)
                {

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
                        levelMenu1 = false;
                        levelMenu2 = false;
                        gameState = GameState.StartMenu;
                        previousMenuButton.clicked = false;
                    }

                    if (nextMenuButton.clicked)
                    {
                        clickSound.Play();
                        levelMenu1 = false;
                        levelMenu2 = true;
                        nextMenuButton.clicked = false;
                    }
                    nivel1Button.Update();
                    nivel2Button.Update();
                    nivel3Button.Update();
                    nivel4Button.Update();
                    nivel5Button.Update();
                    nivel6Button.Update();
                    previousMenuButton.Update();
                    nextMenuButton.Update();
                }
                if (levelMenu2==true)
                {
                    if (nivel7Button.clicked)
                    {
                        InitializeLevel(6);
                        nivel7Button.clicked = false;
                    }
                    if (nivel8Button.clicked)
                    {
                        InitializeLevel(7);
                        nivel8Button.clicked = false;
                    }
                    if (nivel9Button.clicked)
                    {
                        InitializeLevel(8);
                        nivel9Button.clicked = false;
                    }
                    if (nivel10Button.clicked)
                    {
                        InitializeLevel(9);
                        nivel10Button.clicked = false;
                    }
                    if (nivel11Button.clicked)
                    {
                        InitializeLevel(10);
                        nivel11Button.clicked = false;
                    }
                    if (nivel12Button.clicked)
                    {
                        InitializeLevel(11);
                        nivel12Button.clicked = false;
                    }
                    if (previousMenuButton.clicked)
                    {
                        clickSound.Play();
                        levelMenu1 = true;
                        levelMenu2 = false;
                        previousMenuButton.clicked = false;
                    }
                    nivel7Button.Update();
                    nivel8Button.Update();
                    nivel9Button.Update();
                    nivel10Button.Update();
                    nivel11Button.Update();
                    nivel12Button.Update();
                    previousMenuButton.Update();
                }
            }

            // Updating the game while in Playing, Pause and GameOver
            if (gameState == GameState.Playing)
            {
                // In Playing Mode.
                if (isPlaying==true)
                {
                    // It checks if the pause is clicked to go to the pause menu.
                    if (pauseButton.clicked)
                    {
                        isPaused = true;
                        pauseButton.clicked = false;
                    }

                    // Shooting
                    // Pressing mouse left click and verifing if some variables are true
                    // shoots the flask by calculating direction vector between mouse position and
                    // flask position and gives him a velocity by multiplying the position w/ the speed.
                    if (Input.IsMousePressed())
                    {
                        if ((gun.canShoot) && (Mouse.GetState().X > 200))
                        {
                            flaskSound.Play(0.3f, 0.6f, 0);
                            flask = currentLevel.flaskList[currentFlask];
                            flask.position = new Vector2(70, 375);
                            Vector2 direction = new Vector2(Mouse.GetState().X - flask.position.X, Mouse.GetState().Y - flask.position.Y);
                            direction.Normalize();
                            flask.velocity = direction * flask.speed;
                            currentFlask++;
                            gun.canShoot = false;
                            flask.visible = true;
                            flask.idleRotation = 2;
                            currentLevel.attemptScore -= 10000;
                        }
                    }

                    // Collisions and Gravity Fields
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

                        // For each flask in the list it calculates its update
                        foreach (Flask f in currentLevel.flaskList)
                            f.Update(gameTime.ElapsedGameTime);
                        // For each planet in the list
                        foreach (Planet p in currentLevel.planetList)
                        {
                            // Calculates flask gravity when entering planet atmosphere
                            if (flask.calculateGravity(p))
                                currentLevel.orbitingScore += 105;

                            // When flask colide with any planet
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
                    pauseButton.Update();
                }
                // In Pause Menu
                if (isPaused == true)
                {
                    isPlaying = false;
                    // Click Resume Button to go back to the playing state
                    if (pauseResumeButton.clicked)
                    {
                        isPaused = false;
                        isPlaying = true;
                        pauseResumeButton.clicked = false;
                    }

                    // Click Menu Button to go back to level menu
                    if ((pauseMenuButton.clicked))
                    {
                        isPaused = false;
                        isPlaying = false;
                        gameState = GameState.LevelMenu;
                        pauseMenuButton.clicked = false;

                    }

                    // It checks if the retry button is clicked to reset the level.
                    if (pauseRestartButton.clicked)
                    {
                        isPaused = false;
                        pauseRestartButton.clicked = false;
                        InitializeLevel(Level.n_level);
                    }
                    pauseResumeButton.Update();
                    pauseMenuButton.Update();
                    pauseRestartButton.Update();
                }
                // In Finish Mode
                if (currentLevel.IsFinished == true)
                {
                    isPlaying = false;
                    finalRetryButton.Update();
                    currentLevel.UpdateFinish();
                    if ((Level.n_level < 0) || (Level.n_level > levelList.Count - 1))
                    {
                        gameState = GameState.LevelMenu;
                        MediaPlayer.Play(menuSong);
                    }
                    else if (currentLevel.IsFinished == false)
                        InitializeLevel(Level.n_level);

                    // It checks if the retry button is clicked to reset the level.
                    if (finalRetryButton.clicked)
                    {
                        isPaused = false;
                        finalRetryButton.clicked = false;
                        InitializeLevel(Level.n_level);
                    }
                }
                
            }

            if (gameState == GameState.GameOver)
            {
                if (gameoverMenuButton.clicked)
                {
                    gameState = GameState.LevelMenu;
                    gameoverMenuButton.clicked = false;
                }
                if (gameoverRetryButton.clicked)
                {
                    gameState = GameState.Playing;
                    InitializeLevel(Level.n_level);
                    gameoverRetryButton.clicked = false;
                }
                gameoverMenuButton.Update();
                gameoverRetryButton.Update();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.Draw(menuTitle, new Rectangle((GraphicsDevice.Viewport.Width/2)-(menuTitle.Width/2), 20, menuTitle.Width, menuTitle.Height), null, Color.White);
                spriteBatch.Draw(menuPlanetBackground, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (menuPlanetBackground.Width / 2), 200,
                                                                        menuPlanetBackground.Width / (6 / 5), menuPlanetBackground.Height / (6 / 5)), Color.White);
                spriteBatch.Draw(playerNameHUD, new Rectangle(0, 300,playerNameHUD.Width,playerNameHUD.Height), Color.White);
                playButton.Draw(spriteBatch);
                quitButton.Draw(spriteBatch);
                checkButton.Draw(spriteBatch);
                
                if (checkedPlayerName == "")
                    spriteBatch.DrawString(font, playerName, new Vector2(25, 335), Color.Black);
                else spriteBatch.DrawString(font, checkedPlayerName, new Vector2(25, 335), Color.Black);
            }

            if (gameState == GameState.LevelMenu)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                if (levelMenu1 == true)
                {
                    nivel1Button.Draw(spriteBatch);
                    nivel2Button.Draw(spriteBatch);
                    nivel3Button.Draw(spriteBatch);
                    nivel4Button.Draw(spriteBatch);
                    nivel5Button.Draw(spriteBatch);
                    nivel6Button.Draw(spriteBatch);
                    previousMenuButton.Draw(spriteBatch);
                    nextMenuButton.Draw(spriteBatch);
                }
                if (levelMenu2 == true)
                {
                    nivel7Button.Draw(spriteBatch);
                    nivel8Button.Draw(spriteBatch);
                    nivel9Button.Draw(spriteBatch);
                    nivel10Button.Draw(spriteBatch);
                    nivel11Button.Draw(spriteBatch);
                    nivel12Button.Draw(spriteBatch);
                    previousMenuButton.Draw(spriteBatch);
                }
            }

            if (gameState == GameState.Playing)
            {
                spriteBatch.Draw(play_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width
                                                                    , GraphicsDevice.Viewport.Height), Color.White);
                // Draws score
                spriteBatch.DrawString(font, "Orbiting Score: " + currentLevel.orbitingScore.ToString(), new Vector2(15, 500), Color.White);

                DrawTrajectory((float)gameTime.ElapsedGameTime.TotalSeconds);

                // Draws shooted flask
                if (flask != null)
                    flask.Draw(spriteBatch);

                // Draws planets on screeen
                foreach (Planet p in currentLevel.planetList)
                    p.Draw(spriteBatch);

                // Draws flasks on screeen
                foreach (Flask f in currentLevel.flaskList)
                    f.Draw(spriteBatch);

                // Draws current level
                if (Level.n_level == 0)
                    spriteBatch.Draw(lvl1, new Rectangle(1100, 10, 175, 50), Color.White);
                if (Level.n_level == 1)
                    spriteBatch.Draw(lvl2, new Rectangle(1100, 10, 175, 50), Color.White);
                if (Level.n_level == 2)
                    spriteBatch.Draw(lvl3, new Rectangle(1100, 10, 175, 50), Color.White);
                if (Level.n_level == 3)
                    spriteBatch.Draw(lvl4, new Rectangle(1100, 10, 175, 50), Color.White);
                if (Level.n_level == 4)
                    spriteBatch.Draw(lvl5, new Rectangle(1100, 10, 175, 50), Color.White);
                if (Level.n_level == 5)
                    spriteBatch.Draw(lvl6, new Rectangle(1100, 10, 175, 50), Color.White);

                spriteBatch.Draw(astronaut, new Rectangle(-5, 5, 230, 480), Color.White);


                gun.Draw(spriteBatch);

                if (isPaused == true)
                {
                    spriteBatch.Draw(pauseHUD, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (pauseHUD.Width / 2), 100, pauseHUD.Width, pauseHUD.Height), Color.White);
                    pauseResumeButton.Draw(spriteBatch);
                    pauseMenuButton.Draw(spriteBatch);
                    pauseRestartButton.Draw(spriteBatch);
                }
                else pauseButton.Draw(spriteBatch);

                if (currentLevel.IsFinished == true)
                {
                    spriteBatch.Draw(finishHUD, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (finishHUD.Width / 2), 0, finishHUD.Width, finishHUD.Height), Color.White);
                    currentLevel.DrawFinish(spriteBatch);
                    finalRetryButton.Draw(spriteBatch);
                }
                
            }
            if (gameState == GameState.GameOver)
            {
                spriteBatch.Draw(menu_background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.Draw(gameoverHUD, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (gameoverHUD.Width / 2), 100, gameoverHUD.Width, gameoverHUD.Height), Color.White);
                gameoverMenuButton.Draw(spriteBatch);
                gameoverRetryButton.Draw(spriteBatch);   
            }
            spriteBatch.Draw(cursor, cursorPos, Color.White);
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
            isPlaying = true;

            currentLevel = levelList[lvl];
            Level.n_level = lvl;
            if (flask != null) flask.visible = false;

            MediaPlayer.Stop();
            clickSound.Play();
            gameState = GameState.Playing;
        }

        void DrawTrajectory(float time)
        {
            Vector2 dotInitialPos=new Vector2(70, 375);
            Vector2 dotPos= dotInitialPos;
            Vector2 dotDir = new Vector2(Mouse.GetState().X - dotInitialPos.X, Mouse.GetState().Y - dotInitialPos.Y);
            dotDir.Normalize();
            Vector2 dotVel= dotDir * 300;
            Rectangle dotCube = new Rectangle((int)dotPos.X, (int)dotPos.Y, purpleDot.Width, purpleDot.Height);
            while (GraphicsDevice.Viewport.Bounds.Contains(dotCube) == true)
            {
                foreach (Planet p in currentLevel.planetList)
                {
                    if (Vector2.Distance(dotPos, p.position) <= p.atmosphereRadius)
                    {
                        Vector2 acceleration = new Vector2(p.myspace.Center.X - dotCube.Center.X, p.myspace.Center.Y - dotCube.Center.Y);
                        acceleration.Normalize();
                        acceleration *= 9.8f;
                        if (p.repel == true) dotVel -= acceleration;
                        else dotVel += acceleration;
                    }
                }
                dotPos += dotVel * time;
                dotCube = new Rectangle((int)dotPos.X, (int)dotPos.Y, purpleDot.Width/2, purpleDot.Height/2);
                spriteBatch.Draw(purpleDot, dotCube, Color.White);
            }
        }
    }
}
