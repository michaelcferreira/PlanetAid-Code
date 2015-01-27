using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid.Entities
{
    class Planet : Sprite
    {
        public enum Type
        {
            Planet1,
            Planet2,
            Planet3
        }
        private float rotation = 0;
        private bool rotatingLeft = false;
        private float rotationAmmount = 7;
        public Texture2D fieldImg;
        public bool isTarget=false;

        public Planet(Type planetType)
        {
            if (planetType == Type.Planet1)
            {
                ImgName = "planet-1";
                position = new Vector2(640, 350);
                isTarget=false;
            }
            else if (planetType == Type.Planet2)
            {
                ImgName = "planet-2";
                position = new Vector2(1040, 150);
                isTarget = true;
            }
            else if (planetType == Type.Planet3)
            {
                ImgName = "planet-3";
            }
            
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            origin = new Vector2(Img.Width / 2, Img.Height / 2);
            fieldImg = content.Load<Texture2D>("Field");  

        }

        public override void Update(TimeSpan ts)
        {

            //Gun rotation and limitations
            rotation += rotationAmmount * ((float)Math.PI / 180) * (float)ts.TotalSeconds;
            if (rotatingLeft)
            {
                if (rotation <= 0 * ((float)Math.PI / 180))
                {
                    rotationAmmount = 7;
                    rotatingLeft = false;
                }
            }
            else
            {
                if (rotation >= 30 * ((float)Math.PI / 180))
                {
                    rotationAmmount = -7;
                    rotatingLeft = true;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Img, new Rectangle((int)position.X, (int)position.Y, Img.Width, Img.Height), null, Color.White, rotation, origin, SpriteEffects.FlipHorizontally, 0);
            sb.Draw(fieldImg, new Rectangle(640, 350, 300, 300), null, Color.White, 0f,new Vector2(125,125) , SpriteEffects.None, 0);
        }
    }
}
