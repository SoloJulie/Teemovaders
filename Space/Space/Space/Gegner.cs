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
        public bool isVisible;
        public int spalte;
        public int zeile, anzahl;

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
            isVisible = true;
            GegnerListe = new List<Gegner>();
            zeile = 3;
            spalte = 5;
            anzahl = 0;
            
        }


        //Content
        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
            boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height);
            
        }


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in GegnerListe)
            {
                spriteBatch.Draw(textur, gegner.getPos(), Color.White);
            }
        }

        //Update leer
        public void Update(GameTime gameTime)
        {
            
        }

        

        public Vector2 getPos()
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

        public void Sporn()
        {
            for (int y = 0; y <= zeile; y++) //Beginn bei 20px Abstand oben, max px nach unten, 70px erhöhen ( Abstand zwischen Opfern)
            {
                for (int x = 0; x <= spalte; x++)
                {
                    Gegner gegner = new Gegner();
                    //boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height);
                    isVisible = true;
                    gegner.setXPos(x * 80);
                    gegner.setYPos(y * 80);
                    GegnerListe.Add(gegner);
                }
            }
        }


        public int GegnerAnzahl()
        {
            return GegnerListe.Count();
        }

        public void sichtbar()
        {
            foreach (Gegner gegner in GegnerListe)
            {
                if (isVisible == false)
                    GegnerListe.Remove(gegner);
            }

        }


        public void remove()
        {
            foreach (Gegner gegner in GegnerListe)
            {
                for (int i = 0; i < GegnerListe.Count; i++)
                {
                    if (!GegnerListe[i].isVisible) //Wenn Projektil an Stelle i nicht sichtbar ist, entferne sie aus der Liste, setze i--
                    {
                        GegnerListe.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
