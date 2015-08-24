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
    /// <summary> 55min
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D opfer, killer, schuss;
        Rectangle reckiller, rectschuss; //Definiert ein Rechteck
        Rectangle[,] rectinvader;
        String[,] invaderalive;
        int invaderspeed = 5; //Geschwindigkeit nach unten
        int rows = 5;
        int cols = 10;
        String direction = "RIGHT";
        String schussvisible = "NO";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            opfer = Content.Load<Texture2D>("opfer");
            rectinvader = new Rectangle[rows, cols];
            invaderalive = new String[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    rectinvader[r, c].Width = opfer.Width;
                    rectinvader[r, c].Height = opfer.Height;
                    rectinvader[r, c].X = 60 * c;
                    rectinvader[r, c].Y = 60 * r;
                    invaderalive[r, c] = "YES";
                }

            killer = Content.Load<Texture2D>("Teema space");
            reckiller.Width = killer.Width;
            reckiller.Height = killer.Height;
            reckiller.X = 0;
            reckiller.Y = 400;

            schuss = Content.Load<Texture2D>("Pfeil");
            rectschuss.Width = schuss.Width;
            rectschuss.Height = schuss.Height;
            rectschuss.X = 0;
            rectschuss.Y = 0;


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            int rightside = graphics.GraphicsDevice.Viewport.Width;
            int leftside = 0;

            //Bewegt alle Opfer
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (direction.Equals("RIGHT"))
                        rectinvader[r, c].X = rectinvader[r, c].X + invaderspeed;
                    if (direction.Equals("LEFT"))
                        rectinvader[r, c].X = rectinvader[r, c].X - invaderspeed;
                }

            //überprüfen ob ein Alien über die rechte Seite drüber ist
            String changedirection = "N";

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (invaderalive[r, c].Equals("YES"))
                    {
                        if (rectinvader[r, c].X + rectinvader[r, c].Width > rightside)
                        {
                            direction = "LEFT";
                            changedirection = "Y";
                        }
                        if (rectinvader[r, c].X < leftside)
                        {
                            direction = "RIGHT";
                            changedirection = "Y";
                        }
                    }
                }

            //Wenn die Richtung sich geändert hat gehen die Opfer eins nach unten
            if (changedirection.Equals("Y"))
            {
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        rectinvader[r, c].Y = rectinvader[r, c].Y + 5;
            }

            //Teemo bewegen
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Left))
                reckiller.X = reckiller.X - 3;
            if (kb.IsKeyDown(Keys.Right))
                reckiller.X = reckiller.X + 3;
            if (kb.IsKeyDown(Keys.Space) && schussvisible.Equals("NO"))
            {
                schussvisible = "YES";
                rectschuss.X = reckiller.X + (reckiller.Width / 2) - (rectschuss.Width / 2);
                rectschuss.Y = reckiller.Y - rectschuss.Height + 2;
            }

            //Sichtbarer Schuss --> Kugel bewegt sich
            if (schussvisible.Equals("YES"))
                rectschuss.Y = rectschuss.Y - 5;

            //Hat Kugel getroffen
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (invaderalive[r, c].Equals("YES"))
                        if (rectschuss.Intersects(rectinvader[r, c]))
                        {
                            schussvisible = "NO";
                            invaderalive[r, c] = "NO";
                        }
                }

            // Kugel ausserhalb des Bildschrims mach es unsichtbar

            if (rectschuss.Y + rectschuss.Height < 0)
                schussvisible = "NO";


            // Zähle noch lebende Aliens

            int count = 0;
             for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (invaderalive[r, c].Equals("Yes"))
                        count = count + 1;

            // Wenn nur nochdie Hälfte an Alienes verdopple Geschwindigekti
            if (count < (rows * cols / 2))  //Noch mehr oder die Hälft vorhanden ändere keine Geschwindigkeit
                invaderspeed = invaderspeed+0;
            
            if (count > (rows * cols / 3))
                invaderspeed = 20;
            
            //Game over
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (invaderalive[r, c].Equals("YES"))
                        if (rectinvader[r, c].Y + rectinvader[r, c].Height > reckiller.Y)
                            this.Exit();  
                }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (invaderalive[r, c].Equals("YES"))
                    spriteBatch.Draw(opfer, rectinvader[r, c], Color.White);
            spriteBatch.Draw(killer, reckiller, Color.White);

            if (schussvisible.Equals("YES"))
                spriteBatch.Draw(schuss, rectschuss, Color.White);
            spriteBatch.End(); 
                            
                    base.Draw(gameTime);
        }
    }
}
