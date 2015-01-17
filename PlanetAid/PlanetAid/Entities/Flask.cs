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
        private Vector2 speed;
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
                ImgName = "Flask";
                speed = new Vector2(300,300);
            }
            else if (flaskType == Type.MonoFlask)
            {
                ImgName = "MonoFlask";
                speed = new Vector2(50, 50);
            }
            else if (flaskType == Type.FatoFlask)
            {
                ImgName = "FatoFlask";
                speed = new Vector2(50,50);
            }
        }

        public override void Load(ContentManager content)
        {
            Img = content.Load<Texture2D>(ImgName);

            Vector2 pos = new Vector2(150, 375);
            Vector2 direction;
            position = new Vector2(pos.X - Img.Width / 16, pos.Y - Img.Height / 16 - 50);
            direction = new Vector2(Mouse.GetState().X - position.X - (Img.Width / 2), Mouse.GetState().Y - position.Y - (Img.Height / 2));
            direction.Normalize();
            velocity = direction * speed;
        }

        public override void Update(TimeSpan ts)
        {
            velocity.Y += 2f;
            position += velocity * (float)ts.TotalSeconds;
        }
    }
}
