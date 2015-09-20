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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Spieler spieler;

        Hintergrund hin;
        GegnerContainer gc;

        Item item;
        Schutzpilz schutz;

        private SoundEffect effect;

        private SpriteFont font;
        private int punkte = 0;   


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 500;
            this.Window.Title = "Teemovaders";
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            spieler = new Spieler();
            hin = new Hintergrund();
            gc = new GegnerContainer();
            item = new Item();
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

            spieler.Update(gameTime);
            item.Update(gameTime);
            //schutz.Update(gameTime);



            //Macht alle unsichtbar beim Kontakt

            foreach (Gegner gegner in gc.GegnerListe)
            {
                //Erstelle bounding box immer neu für jeden Gegner an jeder position pro frame
                gc.boundingBox = new Rectangle((int)gegner.getX(), (int)gegner.getY(), gc.textur.Width, gc.textur.Height);

                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (gegner.isVisible && s.isVisible)
                    {                
                        if (s.boundingBox.Intersects(gc.boundingBox))
                        {
                            gegner.machUnsichtbar();
                            effect.Play();
                            s.isVisible = false;
                            gc.anzahl--;
                            break;
                        }                        
                    }
                }

                //BoundingBox für Item
                item.boundingBox = new Rectangle((int)item.getX(), (int)item.getY(), item.textur.Width, item.textur.Height); 

                // ändert Gegner Typ und Gegner Aussehen
                if (item.isVisible == true)
                {
                    if (gegner.isVisible && item.boundingBox.Intersects(gc.boundingBox))
                    {

                        if (gegner.gtyp != 1)
                        {
                            gegner.setTyp(item.iTyp);
                            item.isVisible = false;
                        }
                    }
                }
            }



            

            gc.Update(gameTime);

            base.Update(gameTime);
        }


        //Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            hin.Draw(spriteBatch);  //Erst Hintergrund, da nacheinander gezeichnet wird
            gc.Draw(spriteBatch);
            item.Draw(spriteBatch);
            schutz.Draw(spriteBatch);

            punkte = gc.GegnerAnzahl();
            spieler.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Punkte: " + gc.anzahl, new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
