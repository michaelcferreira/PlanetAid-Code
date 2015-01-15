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
        private float gunRotation;
        public bool canShoot;

        public Gun()
        {
            ImgName = "SpaceStation";
        }

        public override void Update(TimeSpan ts)
        {
            // Railgun rotation and limitations
            gunRotation = (float)Math.Atan2(Mouse.GetState().Y - 220, Mouse.GetState().X - 10);
            canShoot = true;
            if (gunRotation <= -.523)
            {
                gunRotation = -.523f;
                canShoot = false;
            }

            if (gunRotation >= .174)
            {
                gunRotation = .174f;
                canShoot = false;
            }
            base.Update(ts);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Img, new Rectangle(180, 340, Img.Width, Img.Height), null, Color.White, gunRotation, new Vector2(176, 176), SpriteEffects.None, 0);
        }


    }
}
