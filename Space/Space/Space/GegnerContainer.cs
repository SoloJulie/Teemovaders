using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Space
{
    public class GegnerContainer
    {
        public Texture2D textur;
        Gegner[] gegner;
        int zeile = 5;
        int spalte = 4;
        int anzahl = 0;


        public GegnerContainer(int anzahl)
        {
            gegner = new Gegner[anzahl];
            this.anzahl = anzahl;

        }

        public int getAnzahl()
        {
            return anzahl;
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //for (int d = 0; d < anzahl; d++)
            //{
            //    gegner[d].Draw(spriteBatch);
            //}
        }

        //Update
        public void Update(GameTime gameTime)
        {
            
        }

        public void Sporn()
        {
            for (int d = 0; d < anzahl; d++)
            {
                for (int z = 0; z < zeile; z++)
                {
                    for (int s = 0; s < spalte; s++)
                    {
                        int tempX = gegner[d].getX() + (20 * z);
                        int tempY = gegner[d].getY() + (20 * s);
                        //gegner[d].setPosition(tempX, tempY);
                        //Lade Gegner}
                    }
                }
            }
        }



        
    }
}
