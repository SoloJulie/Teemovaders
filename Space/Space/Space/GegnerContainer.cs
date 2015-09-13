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
    public class GegnerContainer
    {
        public Texture2D textur;
        public int speed;
        public List<Gegner> GegnerListe;
        public int spalte;
        public int zeile, anzahl;
        public Rectangle boundingBox;
        public bool isVisible;

        public GegnerContainer()
        {
            GegnerListe = new List<Gegner>();
            zeile = 3;
            spalte = 5;
            anzahl = 0;
            isVisible = true;
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
                if (gegner.visible())
                {
                    spriteBatch.Draw(textur, gegner.getPos(), Color.White);
                }
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
            bewegen();            
        }

        public void SpornRechteck()
        {
            for (int y = 0; y <= zeile; y++) //Beginn bei 20px Abstand oben, max px nach unten, 70px erhöhen ( Abstand zwischen Opfern)
            {
                for (int x = 0; x <= spalte; x++)
                {
                    Gegner gegner = new Gegner();                    
                    gegner.isVisible = true;
                    gegner.setXPos(x * 80);
                    gegner.setYPos(y * 80);

                    gegner.setXbb(x * 80);
                    gegner.setYbb(y * 80);
                    gegner.boundingBox = new Rectangle((int)gegner.position.X, (int)gegner.position.Y, textur.Width, textur.Height);

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


        public void remove() //klappt noch nihct
        {
            foreach (Gegner gegner in GegnerListe)
            {
                for (int i = 0; i < GegnerListe.Count; i++)
                {
                    if (GegnerListe[i].isVisible == false) 
                    {
                        GegnerListe.RemoveAt(i);
                        i--;
                    }
                }
            }
        }


        public void bewegen()
        {
            foreach (Gegner gegner in GegnerListe)
            {

                gegner.setXPos(gegner.getX() + gegner.speed);
                gegner.setXbb(gegner.getXbb() + gegner.speed);
                
                //gegner.setXbb(gegner.getX());
                    
                

                //if (gegner.getX() >= 700)
                //{
                //    break;
                //}

                
            }      
                       
           
        }

        public void zurueck()
        {
            foreach (Gegner gegner in GegnerListe)
            {
                if (gegner.getX() >= 700)
                {
                    break;
                }

            }
        }



    }

}
