﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid.Entities
{
    public class Planet : Sprite
    {
        public enum Type
        {
            Planet1,
            Planet2,
            Planet3,
            Asteroid1,
            Asteroid2,
            Asteroid3,
            Asteroid4
        }
        private bool rotatingLeft = false;
        private float rotationAmmount = 7;
        public bool isTarget;
        public Texture2D fieldImg;
        public float radius;
        public float atmosphereRadius;

        public Planet(Type planetType, Vector2 pos, float atmSize, bool isTrgt, Color colr)
        {
            if (planetType == Type.Planet1)
            {
                ImgName = "Planets/Planet-1";
            }
            else if (planetType == Type.Planet2)
            {
                ImgName = "Planets/Planet-2";
                
            }
            else if (planetType == Type.Planet3)
            {
                ImgName = "Planets/Planet-3";
            }
            position = pos;
            atmosphereRadius = atmSize;
            isTarget = isTrgt;
            color = colr;
            
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            origin = new Vector2(Img.Width / 2, Img.Height / 2);
            fieldImg = content.Load<Texture2D>("Field");  
            radius = Img.Width / 2;
            
        }

        public override void Update(TimeSpan ts)
        {

            //Planet rotation
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
            sb.Draw(fieldImg, new Rectangle((int)position.X, (int)position.Y, (int)atmosphereRadius * 2, (int)atmosphereRadius * 2), null, color, 0f, new Vector2(125, 125), SpriteEffects.None, 0);
        }
    }
}
