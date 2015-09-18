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
        public int speed, maxBew, tempBew;
        public List<Gegner> GegnerListe;
        public int spalte, zeile;
        public int anzahl;
        public Rectangle boundingBox;
        public bool isVisible, zurueck, runter;
  

        public GegnerContainer()
        {
            GegnerListe = new List<Gegner>();
            maxBew = 250;
            tempBew = 0;
            zeile = 3;
            spalte = 5;
            anzahl = 0;
            isVisible = true;
            zurueck = false;
            runter = false;
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
                    if (gegner.type == 0) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(textur, gegner.getPos(), Color.White);
                    }

                    
                    else
                    {
                        spriteBatch.Draw(textur, gegner.getPos(), Color.White);
                    }
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

                   
                    //gegner.boundingBox = new Rectangle((int)gegner.position.X, (int)gegner.position.Y, textur.Width, textur.Height);

                    GegnerListe.Add(gegner);
                    anzahl++;
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
            
            for (int i = 0; i < GegnerListe.Count; i++)
            {
                if (GegnerListe.ElementAt(i).isVisible == false) 
                {
                    GegnerListe.RemoveAt(i);
                    break;
                }
            }
            
        }


        public void bewegen()
        {
            if (runter == true)  //Runter gehen
            {
                foreach (Gegner tempGegner in GegnerListe)
                {
                    tempGegner.setYPos(tempGegner.getY() + 10);
                }
                runter = false; 
            }

            foreach (Gegner gegner in GegnerListe)
            {
                if (gegner.zurueck == false)
                {
                    gegner.setXPos(gegner.getX() + gegner.speed); //eigentliche Bewegung für jeden Gegner
                    gegner.tempBew += gegner.speed; //Wert um Position nicht Bildschirm überschreiten zu lassen
                        
                    
                    // Gegner nicht aus Bildschirm
                    if (gegner.tempBew == maxBew) 
                    {
                        gegner.tempBew = 0; //für jeden Gegner einzeln prüfen
                        gegner.zurueck = true;
                        runter = true;
                    }                    
                }

                else
                {
                    gegner.setXPos(gegner.getX() - gegner.speed);
                    gegner.tempBew += gegner.speed;

                    if (gegner.tempBew == maxBew)
                    {
                        gegner.tempBew = 0;
                        gegner.zurueck = false;
                        runter = true;                        
                    }
                }                
            }                     
           
        }

       



    }

}
