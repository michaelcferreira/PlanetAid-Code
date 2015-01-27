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
        public bool Visible { get; set; }
        public Vector2 direction;

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
                speed = new Vector2(300, 300);
            }
            else if (flaskType == Type.FatoFlask)
            {
                ImgName = "FatoFlask";
                speed = new Vector2(300, 300);
            }
        }

        public override void Load(ContentManager content)
        {
            Img = content.Load<Texture2D>(ImgName);
        }

        public override void Update(TimeSpan ts)
        {
            velocity = direction * speed;
            position += velocity * (float)ts.TotalSeconds;
        }
    }
}
