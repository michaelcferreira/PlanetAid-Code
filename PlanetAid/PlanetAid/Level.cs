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
        public static int n_level;
        public List<Flask> flaskList;
        public List<Planet> planetList;
        public Score totalScore;
        public bool IsFinished;
        public Button previousLevelButton;
        public Button nextLevelButton;
        public int attemptScore;
        public int orbitingScore;
        public int initialScore;
        public Vector2 scorePos;
        private SpriteFont font;
        private MouseState oldMouseState;
        private List<List<Score>> highScores;

        public struct Score
        {
            public string name;
            public int score;
        }

        public Level()
        {
            flaskList = new List<Flask>();
            planetList = new List<Planet>();

            // Level Passed Buttons
            nextLevelButton = new Button(690, 320, "Buttons/Button_Next");
            previousLevelButton = new Button(490, 320, "Buttons/Button_Previews");
            scorePos = new Vector2(15, 500);
            initialScore = 40000;
            attemptScore = 0;
            orbitingScore = 0;
            totalScore.score = 0;
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
            foreach (Flask flask in flaskList)
            {
                flask.Load(content);
            }
            foreach (Planet planet in planetList)
            {
                planet.Load(content);
            }

            font = content.Load<SpriteFont>("Segoi UI Symbol");

            // Level Completed Elements
            nextLevelButton.Load(content);
            previousLevelButton.Load(content);
        }

        public void UpdateFinish()
        {
            MouseState mouse = Mouse.GetState();

            // read scores
            ReadScores();

            totalScore.name = Game1.checkedPlayerName;
            totalScore.score = initialScore + attemptScore + orbitingScore;

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

            nextLevelButton.Update();
            previousLevelButton.Update();

            oldMouseState = mouse;
        }

        public void DrawFinish(SpriteBatch sb)
        {
            nextLevelButton.Draw(sb);
            previousLevelButton.Draw(sb);
            sb.DrawString(font, "Score: " + totalScore.score.ToString(), new Vector2(600,200), Color.White);
            
            for (int i = 0; i < highScores[n_level].Count; i++)
                if (i < 4)
                    sb.DrawString(font, highScores[n_level][i].name + " --- " + highScores[n_level][i].score, new Vector2(600, 300 + 25 * i), Color.White);
        }

        public void ReadScores()
        {
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
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), 100, false, true));

            levelList.Add(level);

            // Level 3
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(500, 450), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(900, 230), 200, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1000, 600), 100, false, true));

            levelList.Add(level);

            // Level 4
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 40), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 150), 350));
            level.flaskList.Add(new Flask(Flask.Type.Flasky, new Vector2(25, 205), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 300), 250, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 300), 230, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(150, 90), 90, false, true));

            levelList.Add(level);

            // Level 5
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.MonoFlask, new Vector2(25, 40), 20));

            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(450, 150), 120, true));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(800, 500), 220, true));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(700, 110), 120, false, true));

            levelList.Add(level);

            // Level 6
            level = new Level();

            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 205), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 150), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 95), 20));
            level.flaskList.Add(new Flask(Flask.Type.FatoFlask, new Vector2(25, 40), 20));


            level.planetList.Add(new Planet(Planet.Type.Planet1, new Vector2(600, 60), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet2, new Vector2(1000, 60), 220, false));
            level.planetList.Add(new Planet(Planet.Type.Planet3, new Vector2(1200, 400), 150, false, true));

            levelList.Add(level);

            return levelList;
        }

    }
}
