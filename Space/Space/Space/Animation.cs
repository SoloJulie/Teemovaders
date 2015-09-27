using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Space
{
    public class Animation
    {
        public Texture2D textur;
        public Vector2 position, origin;
        public float timer, interval;
        public int aFrame, sWidth, sHeight;
        public Rectangle sourceRect;
        public bool isVisible;


        public Animation(Texture2D textur, Vector2 position)
        {
            this.textur = textur;
            this.position = position;
            timer = 0f;
            interval = 20f;
            aFrame = 1;
            sWidth = 128;
            sHeight = 128;
            isVisible = true;
        }


        public void LoadConten(ContentManager Conten)
        { }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Timer wird um Milisekunden nach dem letzten update hochgesetzt
            if (timer > interval)
            {                
                aFrame++; //zeige das nächste frame
                timer = 0f; //timer reset
            }

            // wenn am letzten Bild angekommen, unsichtbar und auf erstes Frame zurücksetzen
            if (aFrame == 4)
            {
                isVisible = false;
                aFrame = 0;
            }

            sourceRect = new Rectangle(aFrame * sWidth, 0, sHeight, sHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
            {
                spriteBatch.Draw(textur, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }
            
        }
    }         
}
