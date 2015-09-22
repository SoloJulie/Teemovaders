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
        public Texture2D textur, t2, texprojektil;
        public int speed, maxBew, tempBew;
        public List<Gegner> GegnerListe;
        public List<gegnerSchuss> ListeGegnerProjektil;
        public int spalte, zeile;
        public int anzahl, sDelay, sD;
        public Rectangle boundingBox;
        public bool isVisible, zurueck, runter;
  

        public GegnerContainer()
        {
            GegnerListe = new List<Gegner>();
            ListeGegnerProjektil = new List<gegnerSchuss>();
            maxBew = 250;
            tempBew = 0;
            zeile = 3;
            spalte = 5;
            anzahl = 0;
            isVisible = true;
            zurueck = false;
            runter = false;
            sD = 2;
            sDelay = sD;
        }


        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
            t2 = Content.Load<Texture2D>("opferR");
            texprojektil = Content.Load<Texture2D>("Projektilblau");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in GegnerListe)
            {
                if (gegner.visible())
                {
                    if (gegner.gtyp == 0) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(textur, gegner.getPos(), Color.White);
                    }

                    else if (gegner.gtyp == 1) //welcher Gegnertyp wird gezeichnet
                    {
                         spriteBatch.Draw(t2, gegner.getPos(), Color.White);
                    }
                }
            }

            //gegner Schüsse Zeichnen
            foreach (gegnerSchuss gs in ListeGegnerProjektil) 
                gs.Draw(spriteBatch);
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

            int q = 0;

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


                //Untere Reihe frei zum schießen
                //bool frei = true;

                //for (int w = q; q < GegnerListe.Count; w = +spalte)
                //{
                //    if (GegnerListe.ElementAt(w).isVisible)
                //    {
                //        frei = false;
                //    }
                //}

                //if (frei && ListeGegnerProjektil.Count <= 3)
                //{
                //    Schuss(gegner.getX(), gegner.getY());
                //    updateGegnerSchussListe();
                //}

                //q++;
            }                     
           
        }

         public void Schuss(int x, int y)
         {
            //Schießt nur wenn Delay auf null ist
            if (sDelay >= 0)
            {
                sDelay--;
            }

             //delay ist 0, neuer Schuss sichtbar und in Liste schreiben
            if (sDelay <= 0)            
            {
                gegnerSchuss GProjektil = new gegnerSchuss(texprojektil); //KLasse gegnerSchuss wird erstellt und Textur übergeben
                GProjektil.position = new Vector2(x - 25 + texprojektil.Width / 2, y); //Schuss aus der Mitte von Teemo auslösen

                GProjektil.isVisible = true;


                //Maximal 3 Projektile zur selben Zeit möglich
                if (ListeGegnerProjektil.Count < 3)
                    ListeGegnerProjektil.Add(GProjektil);


                if (sDelay == 0)
                    sDelay = sD;
            }
        }


        //Update Gegner Schuss
         public void updateGegnerSchussListe()
         {
             foreach (gegnerSchuss gs in ListeGegnerProjektil)
             {
                 //Bounding Box um die Projektile
                 gs.boundingBox = new Rectangle((int)gs.position.X, (int)gs.position.Y, gs.texgSchuss.Width, gs.texgSchuss.Height);


                 //Jedes Projektil mit Geschwindigkeit versehen
                 gs.position.Y = gs.position.Y - gs.pSpeed;


                 //aus Bild raus, werde unsichtbar
                 if (gs.position.Y <= 0)
                     gs.isVisible = false;
             }


             // wenn eine Kugel unsichtbar wird, entferne sie aus der Liste
             for (int i = 0; i < ListeGegnerProjektil.Count; i++)
             {
                 if (!ListeGegnerProjektil[i].isVisible) //Wenn Projektil an Stelle i nicht sichtbar ist, entferne sie aus der Liste, setze i--
                 {
                     ListeGegnerProjektil.RemoveAt(i);
                     i--;
                 }
             }
         }
    }

}
