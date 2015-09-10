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
            hin.LoadContent(Content);
            gc.SpornRechteck();
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


            //Macht alle unsichtbar beim Kontakt

            foreach (Gegner gegner in gc.GegnerListe)
            {
                foreach (Schuss s in spieler.getSchussListe())
                {
                    if (gegner.isVisible && s.isVisible && gegner.getBounding().Intersects(s.boundingBox))
                    {
                        gegner.machUnsichtbar();
                        s.isVisible = false;                        
                        break;
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
            hin.Draw(spriteBatch);  //Erst Hintergrund da nacheinander gezeichnet wird
            gc.Draw(spriteBatch);

            punkte = gc.GegnerAnzahl();
            spieler.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Punkte: " + gc.GegnerAnzahl(), new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
