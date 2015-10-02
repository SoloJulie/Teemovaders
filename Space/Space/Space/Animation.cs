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
    public class Animation
    {
        public Texture2D textur;
        public Vector2 position, origin;
        public float timer, interval;
        public int aFrame, sWidth, sHeight, abstand;
        public Rectangle sourceRect;
        public bool isVisible;
        public List<Animation> listeAnimation;


        public Animation(Texture2D textur, Vector2 position)
        {
            this.textur = textur;
            this.position = position;
            abstand = 40;
            timer = 0f; //Zählt die vergangene ZEit
            interval = 50f; //Wie schnell werden die Bilder abgespielt
            aFrame = 1; //aktuelles Frame
            sWidth = 50; //Weite pro einzelbild
            sHeight = 50;  //Höhe pro einzelbild
            isVisible = true;
            listeAnimation = new List<Animation>();
        }


        public void LoadConten(ContentManager Content)
        { 
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Timer wird um Milisekunden nach dem letzten update hochgesetzt
            if (timer > interval)
            {                
                aFrame++; //zeige das nächste frame
                timer = 0f; //timer reset
            }

            // wenn am letzten Bild angekommen, unsichtbar und auf erstes Frame zurücksetzen
            if (aFrame == 10)
            {
                isVisible = false;
                aFrame = 0;
            }

            sourceRect = new Rectangle(aFrame * sWidth, 0, sWidth, sHeight);
            origin = new Vector2(sourceRect.Width / abstand, sourceRect.Height / abstand);            
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
