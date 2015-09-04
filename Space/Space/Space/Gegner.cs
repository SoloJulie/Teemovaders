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
    public class Gegner
    {
        public Texture2D textur;
        public Vector2 position;
        public int speed;

        int zeile = 5;
        int spalte = 4; 


        //Kollision
        public Rectangle boundingBox;
        public bool kollision;

        public Gegner()
        {
            textur = null;
            position = new Vector2(300, 200);  // Erscheinungsposition
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
            //gegner[zeile, spalte].Draw(spriteBatch);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Gegner nebeneinander zeichnen
            //for (int z = 0; z < zeile; z++)
            //{
            //    for (int s = 0; s < spalte; s++)
            //    {
            //        gegner[z, s].Update(gameTime);
            //        //Lade Gegner}
            //    }
            //} 
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public int getX()
        {
            return (int)position.X;
        }

        public int getY()
        {
            return (int)position.Y;
        }



    }
}
