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
    public class Flask : Sprite
    {
        private int speed;
        private int mass;
        public bool Visible { get; set; }
        public enum Type
        {
            Flasky,
            MonoFlask,
            FatoFlask
        }

        public Flask(Type flaskType)
        {
            if (flaskType == Type.Flasky)
            {
                ImgName = "Flasky";
                speed = 50;
            }
            else if (flaskType == Type.MonoFlask)
            {
                ImgName = "MonoFlask";
                speed = 70;
            }
            else if (flaskType == Type.FatoFlask)
            {
                ImgName = "FatoFlask";
                speed = 80;
            }
        }

        public override void Load(ContentManager content)
        {
            Img = content.Load<Texture2D>(ImgName);

            Vector2 pos = new Vector2(150, 375);
            Vector2 vel;
            position = new Vector2(pos.X - Img.Width / 16, pos.Y - Img.Height / 16 - 50);
            vel = new Vector2(Mouse.GetState().X - position.X - (Img.Width / 2), Mouse.GetState().Y - position.Y - (Img.Height / 2));
            vel.Normalize();
            velocity = vel;
        }

        public override void Update(TimeSpan ts)
        {
            position += velocity * speed * (float)ts.TotalSeconds;
        }
    }
}
