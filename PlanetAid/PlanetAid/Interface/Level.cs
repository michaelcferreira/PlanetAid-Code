using PlanetAid.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace PlanetAid.Interface
{
    public class Level
    {
        public List<Flask> flaskList;
        public List<Planet> planetList;

        public Level()
        {
            flaskList = new List<Flask>();
            planetList = new List<Planet>();
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
        }

    }
}
