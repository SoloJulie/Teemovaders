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
    public class GegCont
    {
            public Texture2D textur;
            public Vector2 position;
            public int speed;
            public List<Gegner> GegnerListe;

        public GegCont()
        {
            GegnerListe = new List<Gegner>();
        }


        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in GegnerListe)
            {
                spriteBatch.Draw(gegner.textur, gegner.getPos(), Color.White);
            }
            
        }

        //Update
        public void Update(GameTime gameTime)
        {
            Sporn();
        }

        public void Sporn()
        {
            for (int y = 100; y <= 300; y += 50)
            {
                for (int x = 200; x <= 700; x += 50)
                {
                    Gegner gegner = new Gegner();
                    GegnerListe.Add(gegner);
                    gegner.setXPos(x);
                    gegner.setYPos(y);
                }
            }
        }
    }

}
