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
    public class Item
    {
        public Texture2D textur;
        public Vector2 position;
        public int ispeed;
        public Rectangle boundingBox;
        public bool isVisible;
        public int iTyp;


        public Item()
        {
            textur = null;
            position = new Vector2(0,0);
            ispeed = 1;
            isVisible = true;
            position.X = 0;
            position.Y = 0;
            iTyp = 1;

        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Pilz");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
            {
                spriteBatch.Draw(textur, position, Color.White);
            }
        }

        public void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height);
            fallen();
        }

        public int getX()
        {
            return (int)position.X;
        }

        public int getY()
        {
            return (int)position.Y;
        }

        public void setXPos(int x)
        {
            position.X = x;
        }

        public void setYPos(int y)
        {
            position.Y = y;
        }

        public void fallen()
        {
            setYPos(getY() + ispeed);
        }

        

    }
}
