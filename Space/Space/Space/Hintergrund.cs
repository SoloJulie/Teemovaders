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
    class Hintergrund
    {
        public Texture2D textur, titel, gameover;
        public Vector2 position;
        public int status;


        public Hintergrund()
        {
            textur = null;
            titel = null;
            gameover = null;

            position = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Hintergrund");
            titel = Content.Load<Texture2D>("Buch1");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch, int status)
        {
            this.status = status;

            if (status == 1)
            {
                spriteBatch.Draw(titel, position, Color.White);
            }

            else if (status == 2)
            {            
                spriteBatch.Draw(textur, position, Color.White);
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
        }
    }
}
