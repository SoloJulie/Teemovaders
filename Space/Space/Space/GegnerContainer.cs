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
        public Texture2D minions, minGr, minKl, texProjektil;
        public int speed, maxBew, tempBew;
        public List<Gegner> ListeGegner;
        public List<gegnerSchuss> ListeGProjektil;
        public int spalte, zeile, groesse; //größe rechteck, Anzahl Grgner im Rechteck
        public int anzahl, sDelay, sD, i2s;
        public Rectangle boundingBox;
        public bool zurueck, runter; 


        public GegnerContainer()
        {
            ListeGegner = new List<Gegner>();
            ListeGProjektil = new List<gegnerSchuss>();
            maxBew = 250;
            tempBew = 0;
            zeile = 3;
            spalte = 5;
            anzahl = 0;
            i2s = 0;
            zurueck = false;
            runter = false;
            sD = 4;
            sDelay = sD;
        }


        public void LoadContent(ContentManager Content)
        {
            minions = Content.Load<Texture2D>("Minion Magier");
            minGr = Content.Load<Texture2D>("Minionn Magier 2");
            minKl = Content.Load<Texture2D>("Minionn Magier 2");

            texProjektil = Content.Load<Texture2D>("Projektilblau");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in ListeGegner)
            {
                if (gegner.isVisible == true)
                {
                    if (gegner.gtyp == 0) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(minions, gegner.getPos(), Color.White);                        
                    }

                    else if (gegner.gtyp == 1) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(minGr, gegner.getPos(), Color.White);
                        if (gegner.leben != 0)
                            gegner.setzeLeben(1);
                    }

                    else if (gegner.gtyp == 2) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(minGr, gegner.getPos(), Color.White);
                        if (gegner.leben != 0) 
                            gegner.setzeLeben(2);
                    }

                    else if (gegner.gtyp == 3) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(minGr, gegner.getPos(), Color.White);
                        gegner.isVisible = false;
                        anzahl--;
                    }

                    else if (gegner.gtyp == 4) //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(minGr, gegner.getPos(), Color.White);
                        if (gegner.leben != 0)
                            gegner.setzeLeben(4);
                    }
                }
            }

            //gegner Schüsse Zeichnen
            foreach (gegnerSchuss gs in ListeGProjektil)
                gs.Draw(spriteBatch);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            bewegen();
            remove();
            schneller();
            //Schuss();
            //updateGegnerSchussListe();
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

                    ListeGegner.Add(gegner);
                    anzahl++;
                    groesse = anzahl;
                }
            }
        }


        public int GegnerAnzahl()
        {
            return ListeGegner.Count();
        }



        public void sichtbar()
        {
            foreach (Gegner gegner in ListeGegner)
            {
                if (gegner.isVisible == false)
                    ListeGegner.Remove(gegner);
            }

        }


        public void remove()
        {
            for (int i = 0; i < ListeGegner.Count; i++)
            {
                if (ListeGegner.ElementAt(i).isVisible == false)
                {
                    ListeGegner.RemoveAt(i);
                    break;
                }
            }
        }

        public void bewegen()
        {
            if (runter == true)  //Runter gehen
            {
                foreach (Gegner tempGegner in ListeGegner)
                {
                    tempGegner.setYPos(tempGegner.getY() + 10);
                    i2s += 1;
                }

                runter = false;
            }

            //int q = 0;

            foreach (Gegner gegner in ListeGegner)
            {
                if (gegner.zurueck == false)
                {
                    gegner.setXPos(gegner.getX() + gegner.gspeed); //eigentliche Bewegung für jeden Gegner

                    gegner.tempBew += gegner.gspeed; //Wert um Position nicht Bildschirm überschreiten zu lassen


                    // Gegner nicht aus Bildschirm             

                    if (gegner.tempBew >= maxBew)
                    {
                        gegner.tempBew = 0; //für jeden Gegner einzeln prüfen
                        gegner.zurueck = true;
                        runter = true;
                    }
                }

                else
                {
                    gegner.setXPos(gegner.getX() - gegner.gspeed);
                    gegner.tempBew += gegner.gspeed;

                    if (gegner.tempBew >= maxBew)
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

        public void Schuss()
        {
           //Schießt nur wenn Delay auf null ist
            //if (sDelay >= 0)
            //{
            //    sDelay--;
            //}

            //delay ist 0, neuer Schuss sichtbar und in Liste schreiben
            //if (sDelay <= 0)
            //{
            //for (int i = 0; anzahl % 5 == 0; i++)
            //{
                foreach (Gegner gegner in ListeGegner)
                {                    
                    gegnerSchuss GProjektil = new gegnerSchuss(texProjektil); //KLasse gegnerSchuss wird erstellt und Textur übergeben
                    GProjektil.position = new Vector2(gegner.getX() + GProjektil.texgSchuss.Width+2, gegner.getY() + GProjektil.texgSchuss.Height+25); //Schuss aus der Mitte von Teemo auslösen           

                    GProjektil.isVisible = true;

                    if (ListeGProjektil.Count < 10)
                        ListeGProjektil.Add(GProjektil);
                }
                //Maximal 10 Projektile zur selben Zeit möglich

           // }

                    //if (sDelay == 0)
                    //    sDelay = sD;
                
            
            
        }


        //Update Gegner Schuss
        public void updateGegnerSchussListe()
        {
            foreach (gegnerSchuss gs in ListeGProjektil)
            {
                //Bounding Box um die Projektile
                gs.boundingBox = new Rectangle((int)gs.position.X, (int)gs.position.Y, gs.texgSchuss.Width, gs.texgSchuss.Height);


                //Jedes Projektil mit Geschwindigkeit versehen
                gs.position.Y = gs.position.Y + gs.gpSpeed;


                //aus Bild raus, werde unsichtbar
                if (gs.position.Y >= 500)
                    gs.isVisible = false;
            }


            // wenn eine Kugel unsichtbar wird, entferne sie aus der Liste
            for (int i = 0; i < ListeGProjektil.Count; i++)
            {
                if (ListeGProjektil[i].isVisible == false) //Wenn Projektil an Stelle i nicht sichtbar ist, entferne sie aus der Liste, setze i--
                {
                    ListeGProjektil.RemoveAt(i);
                    i--;
                }
            }
        }


        public void schneller()
        {
            foreach (Gegner gegner in ListeGegner)
            {
                //Bei halber Gegneranzahl erhöhe Gegner Geschwindigkeit
                if (anzahl < groesse / 1.5)
                    gegner.gspeed = 3;

                if (anzahl < groesse / 2)
                    gegner.gspeed = 4;

                if (anzahl < groesse / 3)
                    gegner.gspeed = 5;
            }
        }
    }

}
