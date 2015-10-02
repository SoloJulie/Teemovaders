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
            GameOver,
            gewonnen
        }

        public enum Level
        {
            Lvl1,
            Lvl2,
            Lvl3
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; //Punkte
        Random random = new Random();
        List<Animation> listeAnimation = new List<Animation>();
        List<Item> ListeItem = new List<Item>();

        //3D Modell
        private Model defeat;
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 5, 5), new Vector3(0, -1, 0), Vector3.UnitY); //5 = Abstand zum model, -1 verschiebt nach oben
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);


        //Klassen
        //Animation ani;
        Hintergrund hin;
        GegnerContainer gc;
        Spieler spieler;

        Schutzpilz schutz;
        Sounds sound;


        private SpriteFont font, gO, vic,q,nS;

        private int wahl; //Zufallszahl für Item
        private int status; //Spielstatus an Klassen übergeben
        public float warten, warten2; //Storevariablen für Sounds um um Überlappen zu verhindern
        private bool[] hasSporned; //Array für Item Liste
        private bool skipLvl2 = true;
        private bool skipLvl3 = true;
        private bool garen = false;
        private bool bGaren = false;




        //Punkte
        private int gPunkte = 0;
        private int punkte = 0;
        private int tempPunkte = 0;
        private int schutzPunkte = 0;



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


            hasSporned = new bool[4]; //Item auf 3 pro Level begrenzt, zu beginn false
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
            gO = Content.Load<SpriteFont>("Punkte");
            vic = Content.Load<SpriteFont>("Punkte");
            q = Content.Load<SpriteFont>("Punkte");
            nS = Content.Load<SpriteFont>("Punkte");

            //Klassen
            spieler.LoadContent(Content);     //Lade Spieler
            gc.LoadContent(Content);
            gc.spornRechteck();
            
 
           
            hin.LoadContent(Content);            
            schutz.LoadContent(Content);

            //Model
            defeat = Content.Load<Model>("123");


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


            warten += (float)gameTime.ElapsedGameTime.TotalSeconds; //speichert vergangene Spielzeit in Sekunden //
            warten2 += (float)gameTime.ElapsedGameTime.TotalSeconds; //speichert vergangene Spielzeit in Sekunden

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
                            sound.bewaffnet.Play();
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
                        
                        //Fkt die Lvl unabhängig sind
                        status = 2;
                        spieler.Update(gameTime);
                        gc.Update(gameTime); //remove, schneller, bewegen

                        spielerTrifft(); //iteminteraktion + unsichtbarmachen + punkteberechnung
                        itemZerstoeren();
                        gegnerTrifft();
                        minionKontakt();
                        projektilTreffen();
                        schutzGetroffen();
                        minionTod();
                        itemInteraktGeg(); //Prüft ob Iteminteraktion stattfindet
                        punkteBerechnung();
                        itemTeemo();
                        foreach (Item it in ListeItem)
                            it.Update(gameTime);
                        foreach (Animation a in listeAnimation)
                            a.Update(gameTime);
                        ItemSporn();

                        neuesLevel();
                        lvlSkip();

                        if (skipLvl3 == false && warten2 >= 5)
                            {
                                sound.wahlSound();
                                sound.veigarSpruch(sound.swahl);
                                warten2 = 0;
                            }

                        switch (level) //Je nachdem welches Level geladen wird andere Funktionen
                        {
                            case Level.Lvl1:
                                {
                                    break;
                                } //Case Lvl1 Ende

                            case Level.Lvl2:
                                {
                                    status = 22; //Für Hintergrundklasse Hintergrund Lvl 2 zeichnen                                    
                                    break;
                                }    //Case Lvl2 Ende  
                            case Level.Lvl3:
                                {
                                    status = 23; //Für Hintergrundklasse Hintergrund Lvl 3 zeichnen                                    
                                    break;
                                }//Case Lvl3 Ende 
                        } //Case Level Ende

                        if (spieler.leben > 0 && gc.ListeGegner.Count == 0)
                        {
                            sound.wahlSound();
                            sound.playLachen(sound.swahl);
                            MediaPlayer.Play(sound.menuSong);
                            spielStatus = State.gewonnen;
                        }

                        //Spieler hat kein Leben mehr
                        if (spieler.leben == 0)
                        {
                            sound.tod.Play();
                            MediaPlayer.Play(sound.goSong);
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
                            status = 21;
                            for (int i = 0; i < 27; i++)  //Pilz weieder komplett sichtbar machen
                                schutz.texVisi[i] = true;

                            MediaPlayer.Play(sound.bgrSong);
                            sound.bewaffnet.Play();

                        }
                        else if (keyState.IsKeyDown(Keys.Q))
                        {
                            this.Exit();
                        }
                        break;
                    }
                case State.gewonnen:
                    {
                        status = 4;
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
                            status = 21;

                            for (int i = 0; i < 27; i++)  //Pilz weieder komplett sichtbar machen
                                schutz.texVisi[i] = true;

                            MediaPlayer.Play(sound.bgrSong);
                            sound.bewaffnet.Play();

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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            DrawModel(defeat, world, view, projection); // 3D Model defeat
        
            
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

                        spriteBatch.DrawString(font, "Punkte: " + punkte, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(font, "Leben: " + spieler.leben, new Vector2(600, 0), Color.White);
                        break;
                    }

                //Zeiche GameOver
                case State.GameOver:
                    {
                        DrawModel(defeat, world, view, projection);
                        spriteBatch.DrawString(gO, "GAME OVER!", new Vector2(10, 150), Color.White);
                        spriteBatch.DrawString(font, "Erreichte Punkte: " +punkte, new Vector2(10, 200), Color.White);
                        spriteBatch.DrawString(nS, "Neues Spiel? (J)", new Vector2(10, 250), Color.White);
                        spriteBatch.DrawString(q, "Beenden? (Q)", new Vector2(10, 300), Color.White);
                        break;
                    }

                case State.gewonnen:
                    {
                        hin.Draw(spriteBatch, status);
                        spriteBatch.DrawString(font, "Erreichte Punkte: " + punkte, new Vector2(10, 300), Color.Black);
                        spriteBatch.DrawString(nS, "Neues Spiel? (J)", new Vector2(10, 350), Color.Black);
                        spriteBatch.DrawString(q, "Beenden? (Q)", new Vector2(10, 400), Color.Black);
                        break;
                    }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Funktionen die während des Spiels laufen

        public void auswahl() //Itemtyp zufällig festlegen
        {
            wahl = random.Next(0, 6); //5 nicht inklusive
        }

        public void spielerTrifft() //Spieler trifft + aufruf der Iteminteraktion
        {
            //Macht alle unsichtbar beim Kontakt
            foreach (Gegner gegner in gc.ListeGegner)
            {
                //Erstelle bounding box immer neu für jeden Gegner an jeder position pro frame
                if (gegner.gtyp != 2)
                    gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);
                if (gegner.gtyp == 2)
                    gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.veigar.Width, gc.veigar.Height);



                //Schuss trifft
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (gegner.isVisible && s.isVisible) //Schuss sichtbar und Gegner
                    {
                        if (s.boundingBox.Intersects(gc.boundingBox)) //Schuss trifft Gegner
                        {
                            if (gegner.leben > 1) //Leben ist größer 1
                            {
                                gegner.leben -= 1;
                                s.isVisible = false;
                                
                                if (gegner.leben == 1 && gegner.gtyp != 2)
                                    gegner.gtyp = 0;
                            }

                            else //Gegner hat nur 1 Leben
                            {
                                gegner.machUnsichtbar();
                                listeAnimation.Add(new Animation(Content.Load<Texture2D>("Minion M fade2"), new Vector2(gegner.position.X, gegner.position.Y)));

                                //Zufälliger Sound wird abgespielt
                                if (warten >= 1.5) //Das keine Lachschleife entsteht
                                {
                                    sound.wahlSound();
                                    sound.playLachen(sound.swahl);
                                    warten = 0;
                                }

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

                
            }
        }

        public void itemInteraktGeg() //Gegner interaktion mit Items
        {
            foreach (Gegner gegner in gc.ListeGegner)
            {
                if (gegner.isVisible == true)
                {
                    gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);

                    //ändert Gegner Typ und Gegner Aussehen
                    foreach (Item it in ListeItem)
                    {
                        if (it.iTyp == 6 || it.iTyp == 7)
                            it.boundingBox = new Rectangle((int)it.getX(), (int)it.getY(), it.demaciaTeemo.Width, it.demaciaTeemo.Height);

                        else
                            it.boundingBox = new Rectangle((int)it.getX(), (int)it.getY(), it.prot.Width, it.prot.Height);

                        if (it.isVisible == true && it.iTyp != 6 && it.iTyp != 7)
                        {
                            if (gegner.isVisible && it.boundingBox.Intersects(gc.boundingBox)) //Gegner und Item sichtbar treffen
                            {
                                it.isVisible = false;

                                if (it.iTyp == 1)
                                {
                                    if (gegner.gtyp == 0) //Gegner normal, übergebe Item und ändere Gegnertyp
                                    {
                                        gegner.setTyp(it.iTyp); //Übergebe Itemtyp                                        
                                        gegner.leben = 2;
                                        sound.luluverw.Play();
                                    }
                                    else //Gegner bereits anders, erhöhe leben
                                    {
                                        it.isVisible = false;
                                        gegner.leben += 1;
                                    }
                                }

                                if (it.iTyp == 2) //Item 2 Beschleunigung
                                {
                                    gegner.gspeed += 2;
                                    sound.lspeed.Play();
                                }


                                if (it.iTyp == 3) //Item 3 Todespilz
                                {
                                    if (gegner.gtyp == 0) //Gegner normal, Töte
                                    {
                                        gegner.isVisible = false;
                                        gegner.anzahl--;
                                    }

                                    else //Gegner hat mehr leben --> reduziere um 1
                                        gegner.leben -= 1;
                                }

                                if (it.iTyp == 4) //Item 4 Leben
                                    gegner.leben += 1;


                                if (it.iTyp == 5) // Demacia
                                {
                                    bGaren = true;
                                    sound.demacia.Play();
                                }                                
                            }                            
                        }

                        if (it.isVisible == true && it.iTyp == 7)
                        {
                            if (gegner.isVisible && it.boundingBox.Intersects(gc.boundingBox)) //Gegner und Item sichtbar treffen
                            {
                                if (it.boundingBox.Intersects(gc.boundingBox))
                                {
                                    gegner.isVisible = true;
                                }
                            }
                        }

                        if (gegner.gtyp != 2)
                        {
                            if (it.isVisible == true && it.iTyp == 6)
                            {
                                if (gegner.isVisible && it.boundingBox.Intersects(gc.boundingBox)) //Gegner und Item sichtbar treffen
                                {
                                    if (it.boundingBox.Intersects(gc.boundingBox))
                                    {
                                        gegner.machUnsichtbar();
                                        gc.anzahl--;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            ItemSporn();
        }

        public void itemZerstoeren() //Zerstört Items bei schuss des Spielers
        {            
            foreach (Item it in ListeItem)
                if (it.isVisible == true && it.iTyp < 6) //Großes schwert nciht zerstören
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
                        {
                            spieler.speed += 6;
                            sound.lspeed.Play();
                        }

                        if (it.iTyp == 3) //Todespilz
                        {
                            spieler.leben -= 1;
                            sound.tod.Play();
                        }

                        if (it.iTyp == 4) //Lebenspilz
                            spieler.leben += 1;

                        if (it.iTyp == 5) //Demacia
                        {
                            sound.demacia.Play();
                            ItemSporn();
                            garen = true;
                        }
                        if (it.iTyp == 7) //Demacia                        
                            spieler.leben -= 1;
                        
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
                            sound.tod.Play();
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

            //Gegner trifft den Schutz
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

                    //Teemo Schuss zerstört Pilz
                    foreach (Schuss s in spieler.getSchussListe())
                    {
                        if (s.isVisible == true && s.boundingBox.Intersects(sp))
                        {
                            schutz.texVisi[u] = false;
                            s.isVisible = false;
                            schutzPunkte -= 300;
                        }
                    }

                    //Wenn gegner Pilz berühre, Pilz wird zerstört
                    foreach (Gegner gegner in gc.ListeGegner)
                    {
                        gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.minions.Width, gc.minions.Height);
                        if (gegner.isVisible == true)
                        {
                            if (gc.boundingBox.Intersects(sp))
                            {
                                schutz.texVisi[u] = false;
                                gegner.machUnsichtbar();
                                gc.anzahl--;
                            }
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
                        sound.tod.Play();
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
                    ListeItem.Clear();
                    
                    foreach (Item it in ListeItem)
                        it.isVisible = false;
                    
                    foreach (Animation la in listeAnimation)
                        la.isVisible= false;
                        
                    gc.ListeGProjektil.Clear(); //Gegner Projektile beim Levelübergang löschen

                    if (skipLvl2 == true && skipLvl3 == true)
                        level = Level.Lvl2;

                    if (skipLvl2 == false && skipLvl3 == true)
                        level = Level.Lvl3;

                    if (skipLvl2 == false && skipLvl3 == false)
                        spielStatus = State.gewonnen;
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
        
        public void ItemSporn() //ItemSporn Liste 
        {
            //Anzahl Items über eine LIste max 3
            if (gc.ListeGegner.Count == 15 && hasSporned[0] == false)
            {
                hasSporned[0] = true;
                auswahl();
                Item item = new Item(wahl);
                item.LoadContent(Content);
                ListeItem.Add(item);
            }

            if (gc.ListeGegner.Count == 10 && hasSporned[1] == false)
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

            if (garen == true)
            {
                Item item = new Item(6, 300, 0);
                item.LoadContent(Content);
                ListeItem.Add(item);
                garen = false;
            }

            if (bGaren == true)
            {
                Item item = new Item(7, 300, 0);
                item.LoadContent(Content);
                ListeItem.Add(item);
                bGaren = false;
            }
        }

        public void lvlSkip() // auslösen lvl 2
        {
            if (gc.ListeGegner.Count == 0 && skipLvl2 == true)
            {
                skipLvl2 = false;
                gc.ListeGegner.Clear();
                level = Level.Lvl2;
                gc.spornDreieck();
                hasSporned = new bool[4]; //Item auf 4 pro Level begrenzt, zu beginn false
                for (int i = 0; i < 3; i++)
                    hasSporned[i] = false;
            }

            if (gc.ListeGegner.Count == 0 && skipLvl2 == false && skipLvl3 == true) //lvl3
            {
                skipLvl3 = false;
                gc.ListeGegner.Clear();
                level = Level.Lvl3;
                gc.spornVeigar();
                hasSporned = new bool[4]; //Item auf 4 pro Level begrenzt, zu beginn false
                for (int i = 0; i < 3; i++)
                    hasSporned[i] = false;
            }
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        } //3D Modell

    }
}
