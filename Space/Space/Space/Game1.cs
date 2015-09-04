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

        Gegner gegner;
        //Gegner[,] gegner = null;
        Hintergrund hin;
        GegnerContainer gc;


        int zeile = 5;
        int spalte = 4;       


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
            //Gegner[,] gegner = new Gegner[zeile, spalte];
            gegner = new Gegner();
            gc = new GegnerContainer(zeile*spalte);

            base.Initialize();
        }

        //LoadContent
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gegner.LoadContent(Content);
            spieler.LoadContent(Content);     //Lade Spieler
            gc.LoadContent(Content);
            hin.LoadContent(Content);
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
            
            //Gegner nebeneinander zeichnen
            //for (int z = 0; z < zeile; z++)
            //{
            //    for (int s = 0; s < spalte; s++)
            //    {
            //        gegner[z, s].Update(gameTime);
            //        //Lade Gegner}
            //    }
            //}

            gc.Update(gameTime);
            hin.Update(gameTime);

            base.Update(gameTime);
        }


        //Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            hin.Draw(spriteBatch);  //Erst Hintergrund da nacheinander gezeichnet wird
            spieler.Draw(spriteBatch);
                  
            

            gegner.Draw(spriteBatch);
            gc.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
