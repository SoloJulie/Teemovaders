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
    public class GegnerContainer
    {
        public Texture2D minions, minGr, minVei, texProjektil, veigar;
        public int speed, maxBew, tempBew;
        public List<Gegner> ListeGegner;
        public List<gegnerSchuss> ListeGProjektil;
        public int groesseRecht, groesseDrei; //größe rechteck, Anzahl Grgner im Rechteck
        public int anzahl, sDelay, sD, i2s;
        public Rectangle boundingBox;
        public bool zurueck, runter;
        public Random random;
        public int m;
        public float warten;


        public GegnerContainer()
        {
            ListeGegner = new List<Gegner>();
            ListeGProjektil = new List<gegnerSchuss>();
            anzahl = 0;
            zurueck = false;
            runter = false;
            veigar = null;
            minions = null;
            minGr = null;
            minVei = null;
            texProjektil = null;
        }


        public void LoadContent(ContentManager Content)
        {
            minions = Content.Load<Texture2D>("Minion Magier");
            minGr = Content.Load<Texture2D>("Minionn Magier 2");
            texProjektil = Content.Load<Texture2D>("Projektil violett");
            veigar = Content.Load<Texture2D>("Veigar Minion");
            minVei = Content.Load<Texture2D>("Minion Magier");
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
                    }

                    else //welcher Gegnertyp wird gezeichnet
                    {
                        spriteBatch.Draw(veigar, gegner.getPos(), Color.White);
                    }
                }

                //gegner Schüsse Zeichnen
                if (gegner.gtyp == 1 || gegner.gtyp == 0)
                {
                    foreach (gegnerSchuss gs in ListeGProjektil)
                        gs.Draw(spriteBatch, texProjektil);
                }

                else
                {
                    foreach (gegnerSchuss gs in ListeGProjektil)
                        gs.Draw(spriteBatch, minVei);
                }
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
            warten += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (warten >= 0.5)
            {
                bewegen();
                remove();
                schneller();
                Schuss();
                updateGegnerSchussListe();
            }
            
        }

        public void spornRechteck()
        {
            int zeile = 4;
            int spalte = 5;
            int abstand = 80;

            for (int y = 0; y < zeile; y++) //Beginn bei 20px Abstand oben, max px nach unten, 80px erhöhen ( Abstand zwischen Opfern)
            {
                for (int x = 0; x < spalte; x++)
                {
                    Gegner gegner = new Gegner();
                    gegner.isVisible = true;
                    gegner.setXPos(x * abstand);
                    gegner.setYPos(y * abstand);
                    gegner.setzeLeben(gegner.getGtyp());
                    ListeGegner.Add(gegner);
                    anzahl++;                      
                }
            }
            groesseRecht = anzahl; //zur Berechnung der Geschwindigtkeit, die originalgröße der Liste übergeben
        }
        
        public void spornDreieck()
        {
                int x = 200;
                int y = 0;
                int max = 5; //Anzahl Reihen
                int tempX = 80;
                int tempY = 50;
                int tempX2 = 35;

                for (int i = 1; i <= max; i++) //Reihe
                {
                    for (int j = 0; j < i; j++) 
                    {
                        Gegner gegner = new Gegner();
                        gegner.isVisible = true;
                        gegner.setXPos(x + ((max - i) * tempX2) + j * tempX);
                        gegner.setYPos(y + i * tempY);
                        gegner.setzeLeben(gegner.getGtyp());
                        anzahl++;

                        if ((i == 1 && j == 0) || (i == 3 && j < 5)) //Verwandlung einiger Minions in KLasse 2 Minion
                        {
                            gegner.gtyp = 1;
                            gegner.setzeLeben(gegner.getGtyp());

                        }
                        else
                        {
                            gegner.gtyp = 0;
                        }
                        
                        ListeGegner.Add(gegner);
                    }
                }       
            groesseDrei = anzahl;
        }

        public void spornVeigar()
        {
            int x = 200;
            int y = 100;

            Gegner gegner = new Gegner();
            gegner.isVisible = true;
            gegner.setXPos(x);
            gegner.setYPos(y);
            gegner.gtyp = 2;
            gegner.setzeLeben(gegner.getGtyp());
            ListeGegner.Add(gegner);
            anzahl++;
        }
               

        public void remove()
        {
            for (int w = 0; w < groesseRecht; w++) //Falls mehrere pro Frame getroffen werden
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
        }

        public void bewegen()
        {
            
            tempBew = 0;
            if (groesseDrei != 0) //Andere Abstandsberechung für dreieck
                maxBew = 150; 
            else
                maxBew = 300;

            
            if (runter == true)  //Runter ist true, also nach unten gehen 
            {
                foreach (Gegner gegner in ListeGegner)
                {
                    gegner.setYPos(gegner.getY() + 10);
                }

                runter = false;
            }

            // Gegnerbewegung auf X nach rechts
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
            }
        }            


        public void Schuss() //Sorgt für die Anzahl Schüsse der Gegner, die Position bei den Gegnern und deren Sichtbarkeit
        {
            //randomZahl();
            //int rGl = m;
            //ListeGegner[rGl] mit gegner tauschen
            
            foreach (Gegner gegner in ListeGegner)
            {
                gegnerSchuss GProjektil = new gegnerSchuss(texProjektil); //KLasse gegnerSchuss wird erstellt und Textur übergeben

                        GProjektil.position = new Vector2(gegner.getX() + GProjektil.texgSchuss.Width + 2, gegner.getY() + GProjektil.texgSchuss.Height + 50); //Schuss           

                        GProjektil.isVisible = true;

                        if (ListeGProjektil.Count < 2) //Maximal 5 Projektile zur selben Zeit möglich
                            ListeGProjektil.Add(GProjektil);
                }
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
                if (anzahl < groesseRecht / 1.5)
                    gegner.gspeed = 3;

                if (anzahl < groesseRecht / 2)
                    gegner.gspeed = 4;

                if (anzahl < groesseRecht / 3)
                    gegner.gspeed = 5;
            }
        }

        public void randomZahl()
        {
            Random random = new Random();
            m = random.Next(1, 20); 
        }

       
            
    }
}
