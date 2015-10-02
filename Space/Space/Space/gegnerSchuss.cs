using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Space
{
    public class gegnerSchuss
    {
        public Texture2D texgSchuss;
        public Vector2 position;
        public Rectangle boundingBox;
        public bool isVisible;
        public float gpSpeed;
        public int pPunkte;


        public gegnerSchuss(Texture2D newTexture)
        {
            texgSchuss = newTexture;
            isVisible = true;
            gpSpeed = 2;
            pPunkte = 50;
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texgSchuss, position, Color.White);
        }
    }
}
