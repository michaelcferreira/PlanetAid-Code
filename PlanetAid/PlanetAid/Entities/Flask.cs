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
    public class Flask
    {
        public Vector2 velocity;
        public Vector2 position;
        private Texture2D flaskImg;
        private int speed = 5;
        public bool Visible { get; set; }
        public Rectangle myspace
        {
        get{
            Rectangle myr = new Rectangle((int)position.X, (int)position.Y, flaskImg.Width / 16, flaskImg.Height / 16);
            return myr;
    }
    }

        public Flask(Vector2 pos, Texture2D img)
        {
            flaskImg = img;
            Vector2 vel;
            position = new Vector2(pos.X - flaskImg.Width / 8 / 2, pos.Y - flaskImg.Height / 8 / 2 - 35);
            vel = new Vector2(Mouse.GetState().X - position.X + (flaskImg.Width / 8 / 2), Mouse.GetState().Y - position.Y - (flaskImg.Height / 8 / 2));
            vel.Normalize();
            velocity = vel;
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
