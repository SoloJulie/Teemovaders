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

        public enum Level
        {
            Lvl1,
            Lvl2
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; //Punkte
        Random random = new Random();
        List<Animation> listeAnimation = new List<Animation>();
        List<Item> ListeItem = new List<Item>();


        //Klassen
        //Animation ani;
        Hintergrund hin;
        GegnerContainer gc;
        Spieler spieler;
        //Item item;

        Schutzpilz schutz;
        Sounds sound;

        //private SoundEffect effect;

        private SpriteFont font;

        private int wahl;
        private int status; //Spielstatus an Klassen übergeben

        //Punkte
        private int gPunkte = 0;
        private int punkte = 0;
        private int tempPunkte = 0;
        private int schutzPunkte = 0;
        private bool[] hasSporned; //Array für Item Liste
        private bool skipLvl2 = true;

        // Spielstatus beim Start
        State spielStatus = State.Menue;

        Level level = Level.Lvl1;


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
            schutz = new Schutzpilz();
            sound = new Sounds();

            hasSporned = new bool[3]; //Item auf 3 pro Level begrenzt, zu beginn false
            for (int i = 0; i < 3; i++)
                hasSporned[i] = false;

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
            gc.spornRechteck();          
            hin.LoadContent(Content);            
            schutz.LoadContent(Content);
            sound.LoadContent(Content);
            MediaPlayer.Play(sound.menuSong);
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
            switch (spielStatus) //Entscheidung welcher Spielstatus erreicht wurde  Menü, spielen GameOVer
            {
                case State.Menue:
                    {
                        status = 1;
                        //
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.N))
                        {
                            spielStatus = State.spielen;
                            MediaPlayer.Play(sound.bgrSong);
                        }
                        else if (keyState.IsKeyDown(Keys.Q))
                        {
                            this.Exit();
                        }
                        
                        break;
                    }

                case State.spielen:
                    {
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.S))
                        {
                            level = Level.Lvl2;
                        }
                        status = 2;
                        spieler.Update(gameTime);
                        getroffen(); //iteminteraktion + unsichtbarmachen + punkteberechnung
                        itemZerstoeren();             
                        gegnerTrifft();
                        minionKontakt();
                        projektilTreffen();
                        schutzGetroffen();
                        minionTod();
                        punkteBerechnung();
                        itemTeemo();
                        gc.Update(gameTime); //remove, schneller, bewegen

                        switch (level) //Je nachdem welches Level geladen wird andere Funktionen
                        {
                            case Level.Lvl1:
                                {   //Interaktionen des Spiels             

                                    //ani.minionTod();
                                    //ani.Update(gameTime);

                                    foreach (Animation a in listeAnimation)
                                        a.Update(gameTime);

                                    foreach (Item it in ListeItem)
                                        it.Update(gameTime);

                                    eventTrigger();
                                    neuesLevel();
                                    break;
                                } //Case Lvl1 Ende

                            case Level.Lvl2:
                                {
                                    status = 22; //Für Hintergrundklasse Hintergrund Lvl 2 zeichnen                                    
                                    eventTrigger();
                                    
                                    break;
                                }    //Case Lvl2 Ende                   
                        } //Case Level Ende


                        //Spieler hat kein Leben mehr
                        if (spieler.leben == 0)
                        {
                            punkte = 0;
                            spielStatus = State.GameOver;
                        }

                        break;
                    } //Case State:spielen ende                     




                case State.GameOver:
                    {
                        status = 3; //GameOver Hintergrund
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.J))
                        {
                            //Clearen & Neustart vorbereiten
                            gc.ListeGegner.Clear(); //Leert die Gegner Liste 
                            gc.ListeGProjektil.Clear(); //Löscht Gegnerprojektile
                            spieler.ListeSchuss.Clear(); //Löscht eigene Schüsse     
                            ListeItem.Clear(); //Leert Itemliste
                            gc.spornRechteck(); //Lässt Gegner wieder neu erscheinen
                            spieler.leben = 3;
                            spielStatus = State.spielen;
                        }
                        else if (keyState.IsKeyDown(Keys.Q))
                        {
                            this.Exit();
                        }
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
                        hin.Draw(spriteBatch, status); //Wenn Status Menue, zeichne Menue Übergabe der Statusvariable an Hintergrundklasse
                        break;
                    }

                //Zeiche Spielinhalte
                case State.spielen:
                    {
                        hin.Draw(spriteBatch, status);  //Erst Hintergrund, da nacheinander gezeichnet wird, //übergebe status zahl für hintergrund
                        spieler.Draw(spriteBatch);
                        gc.Draw(spriteBatch);
                        schutz.Draw(spriteBatch);

                         foreach (Item it in ListeItem)
                            it.Draw(spriteBatch);

                        foreach (Animation a in listeAnimation)
                            a.Draw(spriteBatch);

                        switch (level)
                        {
                            case Level.Lvl1:
                                {
                                    break;
                                }
                            case Level.Lvl2:
                                {                           
                                    
                                   
                                    break;
                                }
                        }
                        spriteBatch.DrawString(font, "Punkte: " + gc.anzahl, new Vector2(0, 0), Color.Black);
                        spriteBatch.DrawString(font, "Leben: " + spieler.leben, new Vector2(600, 0), Color.Black);
                        break;
                    }

                //Zeiche GameOver
                case State.GameOver:
                    {
                        hin.Draw(spriteBatch, status);
                        break;
                    }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Funktionen die während des Spiels laufen

        public void auswahl() //Itemtyp zufällig festlegen
        {
            wahl = random.Next(5, 6); //5 nicht inklusive
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
                            if (gegner.leben == 2) //Leben ist größer 1
                            {
                                gegner.leben = 1;
                                //gegner.getLeben();
                                //gegner.machUnsichtbar();
                                s.isVisible = false;
                            }

                            else
                            {
                                gegner.machUnsichtbar();
                                listeAnimation.Add(new Animation(Content.Load<Texture2D>("Minion M fade2"), new Vector2(gegner.position.X, gegner.position.Y)));

                                //Zufälliger Sound wird abgespielt
                                sound.wahlLachen();
                                sound.playLachen(sound.swahl);

                                //Gegner
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

        public void itemInterakt() //Gegner interaktion mit Items
        {
            foreach (Gegner gegner in gc.ListeGegner)
            {
                //BoundingBox für Item
                foreach (Item it in ListeItem)
                    it.boundingBox = new Rectangle((int)it.getX(), (int)it.getY(), it.prot.Width, it.prot.Height);

                gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);


                //ändert Gegner Typ und Gegner Aussehen
                foreach (Item it in ListeItem)
                    if (it.isVisible == true)
                    {
                        if (gegner.isVisible && it.boundingBox.Intersects(gc.boundingBox)) //Gegner und Item sichbar treffen
                        {
                            if (gegner.gtyp == 0) //Gegner normal, übergebe Item und ändere Gegnertyp
                            {
                                gegner.setTyp(it.iTyp); //Übergebe Itemtyp 
                                it.isVisible = false;
                                gegner.leben = 2;
                            }
                        }
                    }
            }
        }

        public void itemZerstoeren() //Zerstört Items bei schuss des Spielers
        {            
            foreach (Item it in ListeItem)
                if (it.isVisible == true)
                {
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (s.boundingBox.Intersects(it.boundingBox))
                    {
                        it.isVisible = false;
                        s.isVisible = false;
                        tempPunkte = it.addIPunkte(); //Punkte für Itemzerstörung
                    }
                }
            }
        }

        public void itemTeemo() //Item Interaktion mit Teemo
        {
            foreach (Item it in ListeItem)
                if (it.isVisible == true)
                {
                    if (spieler.boundingBox.Intersects(it.boundingBox))
                    {
                        it.isVisible = false;
                         if (it.iTyp == 1) //roterPilz
                             spieler.spTyp = 2;
                         if (it.iTyp == 2) //blaue Pilz
                            spieler.speed += 6;
                         if (it.iTyp == 3) //Todespilz
                            spieler.leben -= 1;
                         if (it.iTyp == 4) //Lebenspilz
                            spieler.leben += 1;
                         if (it.iTyp == 5) //Lebenspilz
                             sound.demacia.Play();
                             spieler.leben += 1;
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
                            tempPunkte += gs.pPunkte; //Punkte für zerströrte Gegner je nach Typ
                        }
                    }
                }
            }
        }

        public void schutzGetroffen() //Schutzpilz wird getroffen
        {
            int u = -1; //Laufvariable für einzelne Texturen Maximaler u wert 26 da insgesamt 27 einzelne Pilztexturen

            foreach (Rectangle sp in schutz.Listebb)
            {
                u++; //Vorhererhöhung um nicht durcheinander zu kommen
                if (schutz.texVisi[u] == true) //Textur sichtbar an stelle u
                {
                    foreach (gegnerSchuss gs in gc.ListeGProjektil) //Gegner Schuss
                    {
                        if (gs.isVisible == true && gs.boundingBox.Intersects(sp)) //Gegnerschuss sichtbar und trifft Schutz
                        {
                            schutz.texVisi[u] = false;
                            gs.isVisible = false;
                        }
                    }

                    foreach (Schuss s in spieler.getSchussListe()) //Teemo Schuss
                    {
                        if (s.isVisible == true && s.boundingBox.Intersects(sp))
                        {
                            schutz.texVisi[u] = false;
                            s.isVisible = false;
                            schutzPunkte -= 300;
                        }
                    }
                }
            }
        }

        //Wenn Minion Spieler trifft
        public void minionKontakt()
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


        public State getGameState()
        {
            return spielStatus;
        } //Gib Spielstatus


        public void minionTod() //Animation der Minion Tode
        {
            for (int i = 0; i < listeAnimation.Count; i++)
            {
                if (listeAnimation[i].isVisible == false)
                {
                    listeAnimation.RemoveAt(i);
                    i--;
                }
            }
        }

        public void neuesLevel() //Wenn alle gegner tot starte neues level
        {
                if (gc.ListeGegner.Count == 0)
                {
                    ListeItem.Clear();
                    listeAnimation.Clear();
                    foreach (Animation la in listeAnimation)
                        la.aFrame=0  ;
                    gc.ListeGProjektil.Clear(); //Gegner Projektile beim Levelübergang löschen
                    level = Level.Lvl2;              
                    
                }
        }

        
        
        public void punkteBerechnung()
        {
            if (spieler.leben == 0)
            {
                punkte = 0;
                tempPunkte = 0;
                gPunkte = 0;
                schutzPunkte = 0;
            }
            else
            {
                punkte = tempPunkte + gPunkte + schutzPunkte; //Itempunkte + Gegnerpunkte
            }
        }//Berechnung Punkte (Item + Gegnerpunkte + Projektile + Schutz)


        public void eventTrigger() //ItemSporn Liste + auslösen lvl 2
        {
            //Anzahl Items über eine LIste max 3
            if (gc.ListeGegner.Count == 10 && hasSporned[0] == false)
            {
                hasSporned[0] = true;
                auswahl();
                Item item = new Item(wahl);
                item.LoadContent(Content);
                ListeItem.Add(item);
            }

            if (gc.ListeGegner.Count == 5 && hasSporned[1] == false)
            {
                hasSporned[1] = true;
                auswahl();
                Item item = new Item(wahl);
                item.LoadContent(Content);
                ListeItem.Add(item);
            }

            if (gc.ListeGegner.Count == 1 && hasSporned[2] == false)
            {
                hasSporned[2] = true;
                auswahl();
                Item item = new Item(wahl);
                item.LoadContent(Content);
                ListeItem.Add(item);
            }

            if (gc.ListeGegner.Count == 0 && skipLvl2 == true)
            {
                skipLvl2 = false;
                gc.ListeGegner.Clear();
                level = Level.Lvl2;
                gc.spornDreieck();
            }                    

        }       

    }
}
