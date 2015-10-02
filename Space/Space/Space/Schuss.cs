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
    public class Schuss
    {
        public Texture2D textur;
        public Vector2 position, origin;
        public Rectangle boundingBox;
        public bool isVisible;
        public float pSpeed;
    

        public Schuss(Texture2D newTexture)
        {
            textur = newTexture;
            isVisible = false;
            pSpeed = 10;
        }
                     

        //Draw
        public void Draw(SpriteBatch spriteBatch, Texture2D akTex)        
        {
            spriteBatch.Draw(akTex, position, Color.White);
        }        
    }
}
