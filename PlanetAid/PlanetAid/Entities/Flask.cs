using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid.Entities
{
    public class Flask : Sprite
    {
        public Vector2 speed;
        public Vector2 initialPos;
        public float idleRotation;

        public enum Type
        {
            Flasky,
            MonoFlask,
            FatoFlask,
            JollieFlask,
            HypeFlask,
            BloodFlask
        }

        public Flask(Type flaskType,Vector2 pos,float rot)
        {
            if (flaskType == Type.Flasky)
            {
                ImgName = "Flasks/Flask";
                speed = new Vector2(300,300);
                visible = true;
            }
            else if (flaskType == Type.MonoFlask)
            {
                ImgName = "Flasks/MonoFlask";
                speed = new Vector2(300,300);
                visible = true;
            }
            else if (flaskType == Type.FatoFlask)
            {
                ImgName = "Flasks/FatoFlask";
                speed = new Vector2(300, 300);
                visible = true;
            }
            else if (flaskType == Type.JollieFlask)
            {
                ImgName = "Flasks/JollieFlask";
                speed = new Vector2(300, 300);
                visible = true;
            }
            else if (flaskType == Type.HypeFlask)
            {
                ImgName = "Flasks/HypeFlask";
                speed = new Vector2(300, 300);
                visible = true;
            }
            else if (flaskType == Type.BloodFlask)
            {
                ImgName = "Flasks/BloodFlask";
                speed = new Vector2(300, 300);
                visible = true;
            }
            position = pos;
            idleRotation = 0;
            rotation = (float)Math.PI/180*rot;

        }

        public override void Load(ContentManager content)
        {
            Img = content.Load<Texture2D>(ImgName);
            origin = new Vector2(Img.Width / 2, Img.Height / 2);
        }

        public override void Update(TimeSpan ts)
        {
            rotation += idleRotation * (float)ts.TotalSeconds;
            base.Update(ts);
        }

        public bool calculateGravity(Planet planet)
        {
            if (Vector2.Distance(position, planet.position) <= planet.atmosphereRadius)
            {
                Vector2 acceleration = new Vector2(planet.myspace.Center.X - myspace.Center.X, planet.myspace.Center.Y - myspace.Center.Y);
                acceleration.Normalize();
                acceleration *= 9.8f;
                if (planet.repel == true) velocity -= acceleration;
                else 
                {
                    velocity += acceleration;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
