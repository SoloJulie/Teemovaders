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

        public List<Gegner> GegnerListe;



        //Kollision
        public Rectangle boundingBox;
        public bool kollision;

        public Gegner()
        {
            textur = null;
            position = new Vector2();  // Erscheinungsposition
            position.X = 0;
            position.Y = 0;
            speed = 2;
            kollision = false;
            GegnerListe = new List<Gegner>();
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in GegnerListe)
            {
                spriteBatch.Draw(textur, gegner.getPos(), Color.White);
            } 
        }

        //Update
        public void Update(GameTime gameTime)
        {
            Sporn();
        }

        public void Sporn()
        {
            for (int y = 30; y <= 300; y += 60)
            {
                for (int x = 30; x <= 600; x += 60)
                {
                    Gegner gegner = new Gegner();
                    GegnerListe.Add(gegner);
                    gegner.setXPos(x);
                    gegner.setYPos(y);
                }
            }
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setXPos(int x)
        {
            position.X = x;
        }

        public void setYPos(int y)
        {
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

        public Vector2 getPos()
        {
            return position;
        }



    }
}
