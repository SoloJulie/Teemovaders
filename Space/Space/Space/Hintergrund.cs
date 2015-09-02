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
        public Texture2D textur;
        public Vector2 position;


        public Hintergrund()
        {
            textur = null;
            position = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Hintergrund");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        //Update
        public void Update(GameTime gameTime)
        {
        }
    }
}
