using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlanetAid.Entities;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace PlanetAid
{
    public class Level 
    {
        public static int n_level;              // Level number
        public List<Flask> flaskList;           // List containing the flaks
        public List<Planet> planetList;         // List containing the planets
        public bool IsFinished;                 // Bool to check if level is finished
        public Button previousLevelButton;      // Button for the previous level
        public Button nextLevelButton;          // Button for the next level
        public Vector2 scorePos;        
        private SpriteFont font;
        private MouseState oldMouseState;
        private List<List<Score>> highScores;
        public int attemptScore;
        public int orbitingScore;               // The score given by navigating through the atmosphere
        public int initialScore;                // The score that defines the attempt score
        public Score totalScore;                // The total of all scores

        public struct Score                     // The score that gets the player name and his score
        {
            public string name;                 // Gets  checked player name
            public int score;                   // Gets total score
        }

        public Level()
        {
            // Defines the lists
            flaskList = new List<Flask>();
            planetList = new List<Planet>();

            // Level Passed Buttons
            nextLevelButton = new Button(690, 320, "Buttons/Button_Next");
            previousLevelButton = new Button(490, 320, "Buttons/Button_Previews");

            // In-Game score position and its initial values
            scorePos = new Vector2(15, 500);
            initialScore = 40000;
            attemptScore = 0;
            orbitingScore = 0;
            totalScore.score = 0;

            // Different lists to be saved in the highscores XML file
            // in order to be saved by level so it means each list is a level
            highScores = new List<List<Score>>() {
                new List<Score>(),
                new List<Score>(),
                new List<Score>(),
                new List<Score>(),
                new List<Score>(),
                new List<Score>(),
            };
        }

        public void Load(ContentManager content)
        {
            // Loads Flaks and Planets for each entitie in list
            foreach (Flask flask in flaskList)
            {
                flask.Load(content);
            }
            foreach (Planet planet in planetList)
            {
                planet.Load(content);
            }

            // Loads Font
            font = content.Load<SpriteFont>("Segoi UI Symbol");

            // Level Completed Buttons
            nextLevelButton.Load(content);
            previousLevelButton.Load(content);
        }

        public void UpdateFinish()
        {
            MouseState mouse = Mouse.GetState();

            // Read scores on XML file
            ReadScores();

            totalScore.name = Game1.checkedPlayerName;
            totalScore.score = initialScore + attemptScore + orbitingScore;

            // If either next level button or previous level button is clicked
            if ((nextLevelButton.clicked) && (oldMouseState.LeftButton == ButtonState.Released))
            {
                SaveScores();
                n_level++;
                nextLevelButton.clicked = false;
                IsFinished = false;
            }
            if ((previousLevelButton.clicked) && (oldMouseState.LeftButton == ButtonState.Released))
            {
                SaveScores();
                n_level--;
                previousLevelButton.clicked = false;
                IsFinished = false;
            }

            // Buttons update (checks the click)
            nextLevelButton.Update();
            previousLevelButton.Update();

            oldMouseState = mouse;
        }

        public void DrawFinish(SpriteBatch sb)
        {
            // Draws buttons on screen
            nextLevelButton.Draw(sb);
            previousLevelButton.Draw(sb);

            // Draws total score
            sb.DrawString(font, "Score: " + totalScore.score.ToString(), new Vector2(600,200), Color.White);
            
            // Draws 4 highscores ands repective player
            for (int i = 0; i < highScores[n_level].Count; i++)
                if (i < 4)
                    sb.DrawString(font, highScores[n_level][i].name + " --- " + highScores[n_level][i].score, new Vector2(600, 300 + 25 * i), Color.White);
        }

        public void ReadScores()
        {
            // Reads file data only if it exists
            if (File.Exists("highscores.txt"))
            {
                FileStream stream = File.Open("highscores.txt", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<List<Score>>));
                highScores = (List<List<Score>>)serializer.Deserialize(stream);
                stream.Close();
            }
        }

        public void SaveScores()
        {
            // Saves scores to the file

            if ((n_level >= 0) && (n_level < 6))
            {
                FileStream stream = File.Open("highscores.txt", FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(List<List<Score>>));


                // Remove existing highscore entry if it already exists
                List<Score> scoresToSave = new List<Score>();
                bool playerFirstTime = true;

                foreach (Score savedScore in highScores[n_level])
                {
                    Score newScore = savedScore;

                    if (savedScore.name == totalScore.name)
                        playerFirstTime = false;
                    
                    if (totalScore.score > savedScore.score)
                        newScore.score = totalScore.score;
                    
                    scoresToSave.Add(newScore);
                }

                if (playerFirstTime) scoresToSave.Add(totalScore);
                scoresToSave = scoresToSave.OrderByDescending(levelScore => levelScore.score).ToList();

                highScores[n_level] = scoresToSave;
                serializer.Serialize(stream, highScores);
                stream.Close();
            }
        }

        public static List<Level> CreateLevelsList()
        {
            //Adding levels
            List<Level> levelList = new List<Level>();


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
            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(640, 350), Vector2.Zero, 190, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 200), Vector2.Zero, 150, false, true));
            

            levelList.Add(level);

            // Level 2
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(1000, 290), Vector2.Zero, 200, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), Vector2.Zero, 100, false, true));

            levelList.Add(level);

            // Level 3
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(500, 450), Vector2.Zero, 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(900, 230), Vector2.Zero, 200, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), Vector2.Zero, 100, false, true));
            level.planetList.Add(new Planet(Planet.Type.Asteroid1, new Vector2(850, 500), new Vector2(-70, 70), 0, false));

            levelList.Add(level);

            // Level 4
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 40), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 150), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 205), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 300), Vector2.Zero, 250, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 300), Vector2.Zero, 230, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(150, 90), Vector2.Zero, 90, false, true));

            levelList.Add(level);

            // Level 5
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(450, 150), Vector2.Zero, 120, true));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(800, 500), Vector2.Zero, 220, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(800, 110), Vector2.Zero, 120, false, true));
            level.planetList.Add(new Planet(Planet.Type.Asteroid1, new Vector2(620, 250), Vector2.Zero, 0, false));

            levelList.Add(level);

            // Level 6
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));


            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 60), Vector2.Zero, 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 60), Vector2.Zero, 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), Vector2.Zero, 150, false, true));

            levelList.Add(level);

            return levelList;
        }

    }
}
