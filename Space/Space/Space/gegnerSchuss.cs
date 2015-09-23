﻿using System;
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
        public Vector2 position, origin;
        public Rectangle boundingBox;
        public bool isVisible;
        public float pSpeed;


        public gegnerSchuss(Texture2D newTexture)
        {
            texgSchuss = newTexture;
            isVisible = false;
            pSpeed = 10;
        }


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texgSchuss, position, Color.White);
        }
    }
}