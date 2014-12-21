using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid.Entities
{
    class Flask
    {
        private Vector2 velocity;
        private Vector2 position;
        private Texture2D flaskImg;


       public Flask(Vector2 vel, Vector2 pos, Texture2D img)
        {
            //vel.Normalize();
            velocity = vel;
            position = pos;
            flaskImg = img;
        }
       public void update()
       {

           position += new Vector2(20, 0);
       }

       public void Draw(SpriteBatch sb)
       {
           sb.Draw(flaskImg, new Rectangle((int)position.X, (int)position.Y, flaskImg.Width/8, flaskImg.Height/8), Color.White);

       }

    
    }
}
