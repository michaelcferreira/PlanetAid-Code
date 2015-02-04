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
    class Gun : Sprite
    {
        public float gunRotation;   // Gun Rotation
        public bool canShoot;       // Checks if its possible to shoot


        public Gun()
        {
            ImgName = "Shooter/Gun";
            canShoot = true;
        }

        public override void Update(TimeSpan ts)
         {
            //Railgun rotation and limitations
            gunRotation = (float)Math.Atan2(Mouse.GetState().Y - 360, Mouse.GetState().X - 100);

            if (gunRotation <= -.700)
            {
                gunRotation = -.700f;
            }
            if (gunRotation >= .600)
            {
                gunRotation = .600f;
            }
            base.Update(ts);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Img, new Rectangle(100, 360, Img.Width, Img.Height), null, Color.White, gunRotation, new Vector2(23, 26), SpriteEffects.None, 0);
        }


    }
}
