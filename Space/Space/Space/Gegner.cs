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
    public class Gegner
    {
        public Texture2D textur;
        public Vector2 position;
        public int speed;


        //Kollision
        public Rectangle boundingBox;
        public bool kollision;

        public Gegner()
        {
            textur = null;
            position = new Vector2(300, 200);
            speed = 2;
            kollision = false;
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        //Update
        public void Update(GameTime gameTime)
        {
        }
    }
}
