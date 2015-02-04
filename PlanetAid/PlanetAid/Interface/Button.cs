using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetAid
{
    public class Button
    {
        private Texture2D image;    // Button image
        private string imageName;   // Button image name
        public Rectangle bRect;     // Button rectangle
        public bool clicked=false;  // Checks if button is clicked

        // Declaring values for buttons
        public Button(int x, int y, string name)
        {
            imageName = name;
            bRect.X = x;
            bRect.Y = y;
            bRect.Width = 100;
            bRect.Height =100;
            clicked = false;
        }

        public void Load(ContentManager content)
        {
            image = content.Load<Texture2D>(imageName);
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            Rectangle clickArea = new Rectangle(mouse.X, mouse.Y, 1, 1);

            // Checks if the mouse clicks button
            if (clickArea.Intersects(bRect) && mouse.LeftButton == ButtonState.Pressed)
                clicked = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, bRect, Color.White);
        }
    }
}