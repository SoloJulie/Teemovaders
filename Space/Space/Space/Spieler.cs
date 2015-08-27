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
    public class Spieler
    {
        public Texture2D textur;
        public Vector2 position;
        public int speed;

        //Kollision
        public Rectangle boundingBox;
        public bool kollision;

        public Spieler()
        {
            textur = null;
            position = new Vector2(300, 300);
            speed = 5;
            kollision = false;
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Teemo");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            // Bewegung Teemo
            if (kb.IsKeyDown(Keys.Left))
                position.X = position.X - speed;

            if (kb.IsKeyDown(Keys.Right))
                position.X = position.X + speed;


            //Spieler bleibt innerhalb des Bildschirms
            if (position.X <= 0)
                position.X = 0;

            if (position.X >= 700 - textur.Width)
                position.X = 700 - textur.Width;




        }

    }
}
