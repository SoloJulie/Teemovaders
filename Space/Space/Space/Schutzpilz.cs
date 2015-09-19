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
    public class Schutzpilz
    {
        public Texture2D textur,t2;
        public Vector2 position;
        public Rectangle boundingBox;
        public bool isVisible;
        public int iTyp;


        public Schutzpilz()
        {
            textur = null;
            position = new Vector2(0, 350);
            isVisible = true;

        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("P1");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height);
        }



        //public void schutz()
        //{
        //    for (int i = 0; i<=9; i++)
        //    {
            
        //    position.Y = 400;

        //}
    }
}
