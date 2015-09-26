using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Spielstatus festlegen
        public enum State
        {
            Menue,
            spielen,
            GameOver
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; //Punkte
        Random random = new Random();
        Hintergrund hin;
        
        GegnerContainer gc;
        Spieler spieler;
        Item item;
        Schutzpilz schutz;

        private SoundEffect effect;

        private SpriteFont font;
        private int gPunkte = 0;
        private int punkte = 0;
        private int wahl, tempPunkte;
        private Texture2D menuHint;
        private int status; //Spielstatus an Klassen übergeben

        // Spielstatus beim Start
        State spielStatus = State.Menue;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 700; //X
            graphics.PreferredBackBufferHeight = 500; //Y
            this.Window.Title = "Teemovaders";
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            spieler = new Spieler();
            hin = new Hintergrund();
            gc = new GegnerContainer();
            auswahl(); //zum Random erzeugen eines Items
            item = new Item(wahl);
            schutz = new Schutzpilz();

            base.Initialize();
        }

        //LoadContent
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Punkte");
            spieler.LoadContent(Content);     //Lade Spieler
            gc.LoadContent(Content);
            item.LoadContent(Content);
            hin.LoadContent(Content);
            gc.SpornRechteck();
            schutz.LoadContent(Content);
            effect = Content.Load<SoundEffect>("4");
            menuHint = Content.Load<Texture2D>("Titelbild");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        //Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Spielstatus
            switch(spielStatus) 
            {
                case State.Menue:
                    {
                        status = 1;
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.Enter))
                        {
                            spielStatus = State.spielen;
                        }
                        break;
                    }
                case State.spielen:
                    {
                        status = 2;
                        spieler.Update(gameTime);
                        item.Update(gameTime); 
                        gc.Update(gameTime); //remove, schneller, bewegen
                        getroffen(); //iteminteraktion + unsichtbarmachen + punkteberechnung
                        itemZerstoeren();
                        punkteBerechnung();
                        gegnerTrifft();
                        projektilTreffen();
                        schutzGetroffen();
                        minionKontakt();
                        if (spieler.leben == 0)
                            spielStatus = State.GameOver;                        
                        break;
                    }
                
                case State.GameOver:
                    {
                        status = 3;
                        break;
                    }
            }       
            
            base.Update(gameTime);
        }


        //Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (spielStatus)
            {
                    //Zeiche Menue
                case State.Menue:
                    {
                        hin.Draw(spriteBatch, status); //Wenn Status Menue, zeichne Menue
                        break;
                    }

                //Zeiche Spielinhalte
                case State.spielen:
                    {
                        //Erst Hintergrund, da nacheinander gezeichnet wird
                        hin.Draw(spriteBatch, status);  
                        spieler.Draw(spriteBatch);
                        gc.Draw(spriteBatch);
                        schutz.Draw(spriteBatch);
                        item.Draw(spriteBatch);
                        spriteBatch.DrawString(font, "Punkte: " + punkte, new Vector2(0, 0), Color.Black);
                        spriteBatch.DrawString(font, "Leben: " + spieler.leben, new Vector2(600, 0), Color.Black);
                        break;
                    }

                //Zeiche GameOver
                case State.GameOver:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        break;
                    }
            }

            
            spriteBatch.End();

           



            base.Draw(gameTime);
        }

        public void auswahl() //Itemtyp zufällig festlegen
        {
            wahl = random.Next(1, 5); //5 nicht inklusive
        }

        public void getroffen() //Spieler trifft + aufruf der Iteminteraktion
        {
            //Macht alle unsichtbar beim Kontakt
            foreach (Gegner gegner in gc.ListeGegner)
            {
                //Erstelle bounding box immer neu für jeden Gegner an jeder position pro frame
                gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);

                //Schuss trifft
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (gegner.isVisible && s.isVisible) //Schuss sichtbar und Gegner
                    {
                        if (s.boundingBox.Intersects(gc.boundingBox)) //Schuss trifft Gegner
                        {
                            if (gegner.leben > 1) //Leben ist größer 1
                            {
                                gegner.leben--;
                                s.isVisible = false;
                            }

                            else
                            {
                                gegner.machUnsichtbar();
                                effect.Play(); //Sound wird abgespielt
                                s.isVisible = false; //Schuss unsichtbar
                                gc.anzahl--;
                                gegner.berechnungPunkte();
                                gPunkte += gegner.punkte; //Punkteberechnung aus Gegner
                                break;
                            }
                        }
                    }
                }

                itemInterakt(); 
            }
        }
        
        public void itemInterakt() //verwandelt Gegner
        {
            foreach (Gegner gegner in gc.ListeGegner)
            {
                //BoundingBox für Item
                item.boundingBox = new Rectangle((int)item.getX(), (int)item.getY(), item.prot.Width, item.prot.Height);

                //ändert Gegner Typ und Gegner Aussehen
                if (item.isVisible == true)
                {
                    if (gegner.isVisible && item.boundingBox.Intersects(gc.boundingBox)) //Gegner und Item sichbar treffen
                    {
                        if (gegner.gtyp == 0) //Gegner normal, übergebe Item und ändere Gegnertyp
                        {
                            gegner.setTyp(item.iTyp); //Übergebe Itemtyp 
                            item.isVisible = false;
                        }
                    }
                }
            }
        } 

        public void itemZerstoeren() //Zerstört Items bei schuss des Spielers
        {
            if (item.isVisible == true)
            {
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (s.boundingBox.Intersects(item.boundingBox))
                    {
                        item.isVisible = false;
                        s.isVisible = false;
                        tempPunkte = item.addIPunkte();
                    }
                }
            }
        }

        public void gegnerTrifft() //Gegner Schuss trifft Spieler
        {
            foreach (gegnerSchuss gs in gc.ListeGProjektil)
            {          
                if (gs.isVisible == true)
                {
                    if (gs.boundingBox.Intersects(spieler.boundingBox))
                    {
                        if (spieler.leben >= 1)
                        {
                            spieler.leben--;
                            gs.isVisible = false;
                            break;
                        }
                        
                    }
                }
            }
        } 

        public void projektilTreffen() //beide Projektile treffen sich
        { 
            foreach (gegnerSchuss gs in gc.ListeGProjektil)
            {          
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (gs.isVisible == true && s.isVisible == true)
                    {
                        if (s.boundingBox.Intersects(gs.boundingBox))
                        {
                            s.isVisible = false;
                            gs.isVisible = false;
                            tempPunkte += gs.pPunkte;
                        }
                    }
                }
            }
        }


        public void schutzGetroffen()
        {
            schutz.boundingBox = new Rectangle((int)schutz.getX(), (int)schutz.getY(), schutz.t1.Width, schutz.t1.Height);
            
            foreach (gegnerSchuss gs in gc.ListeGProjektil)
            {          
                foreach (Schuss s in spieler.getSchussListe())
                {
                    int temp = 2;
                    //if (schutz.texVisi[2] == true) //Pilz sichtbar
                    //{
                        if (gs.isVisible == true || s.isVisible == true) // Gegner oder Spieler Schuss sichtbar
                        {
                            if (s.boundingBox.Intersects(schutz.boundingBox))
                            {
                                schutz.isVisible = false;
                                s.isVisible = false;
                            }

                            if (gs.boundingBox.Intersects(schutz.boundingBox))
                            {
                                schutz.isVisible = false;
                                gs.isVisible = false;
                            }
                        }
                    }
                }
               
        }

        public void minionKontakt() //Wenn Minion Spieler trifft
        {
            foreach (Gegner gegner in gc.ListeGegner)
            {
                gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);

                if (gegner.isVisible == true)
                {
                    if (spieler.boundingBox.Intersects(gc.boundingBox))
                    {
                        spieler.leben--;
                        gegner.isVisible = false;
                    }
                }
            }
        }



        //Berechnung Punkte (Item + Gegnerpunkte + Projektile)
        public void punkteBerechnung()
        {
            punkte = tempPunkte + gPunkte;
        }

        public State getGameState()
        {
            return spielStatus;
        }











    }           
}
