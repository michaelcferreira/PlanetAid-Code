using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private int speed=4;

        public Flask(Vector2 pos, Texture2D img)
        {
            Vector2 vel;
            vel = new Vector2(Mouse.GetState().X - pos.X, Mouse.GetState().Y - pos.Y);
            vel.Normalize();

            velocity = vel;
            position = pos;
            flaskImg = img;
        }
        public void update()
        {

            position += velocity * speed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(flaskImg, new Rectangle((int)position.X, (int)position.Y, flaskImg.Width / 8, flaskImg.Height / 8), Color.White);

        }


    }
}
