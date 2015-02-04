using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid.Entities
{
    public class Sprite
    {
        public Texture2D Img;           // Sprites texture
        public float rotation = 0;      // Sprites rotation
        public string ImgName;          // Sprite Image Name
        public Vector2 position;        // Sprite position
        public Vector2 velocity;        // Sprite velocity
        public Vector2 origin;          // Sprite image center
        public Color color=Color.White; // Sprite color (only used for atmosphere color)
        public bool visible=true;       // Sprite visability
        public Rectangle myspace        // Sprite rectangle (for colisions)
        {
            get
            {
                Rectangle myr = new Rectangle((int)position.X - (int)origin.X, (int)position.Y - (int)origin.Y, Img.Width, Img.Height);
                return myr;
            }
        }

        public Sprite()
        {
        }
        public virtual void Load(ContentManager content)
        {
            Img = content.Load<Texture2D>(ImgName);
        }
        public virtual void Update(TimeSpan ts)
        {
            position += velocity * (float)ts.TotalSeconds;
        }
        public virtual void Draw(SpriteBatch sb)
        {
            if (visible == true)
            {
                sb.Draw(Img, new Rectangle((int)position.X, (int)position.Y, Img.Width, Img.Height), null, color, rotation, origin, SpriteEffects.None, 0);
            }
        }
    }
}
