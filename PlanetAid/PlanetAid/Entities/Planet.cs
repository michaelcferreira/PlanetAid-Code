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

        public Planet(Type planetType)
        {
            if (planetType == Type.Planet1)
            {
                ImgName = "planet-1";
            }
            else if (planetType == Type.Planet2)
            {
                ImgName = "planet-2";
            }
            else if (planetType == Type.Planet3)
            {
                ImgName = "planet-3";
            }
            position = new Vector2(1000, 200);
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            origin = new Vector2(Img.Width / 2, Img.Height / 2);
        }

        public override void Update(TimeSpan ts)
        {
            //ADD YOUR UPDATE LOGIC HERE aka gravity shit

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
        }
    }
}
