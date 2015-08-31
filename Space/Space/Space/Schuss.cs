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
    class Schuss
    {
        public Texture2D textur;
        public Vector2 position;

        public Schuss()
        {
            textur = null;
            position = new Vector2(300,10);
        }


        //Content
        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Schuss");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
