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
        public Texture2D textur, projektilTextur;
        public Vector2 position;
        public int ispeed;
        public Rectangle boundingBox;
        public List<Schuss> schussListe; //Liste um Projektile besser händeln zu können
        public bool isVisible;


        public Item()
        {
            textur = null;
            position = new Vector2(300, 0);
            ispeed = 2;
            isVisible = true;
            position.X = 0;
            position.Y = 0;
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Pilz");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);
        }

        public void Update(GameTime gameTime)
        {
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
