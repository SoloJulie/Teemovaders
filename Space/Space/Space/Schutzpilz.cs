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
        public Texture2D t1,t2,t3,t4,t5,t6,t7,t8,t9,tex;
        public Vector2 position;
        public Rectangle boundingBox;
        public bool isVisible;
        public int iTyp;


        public Schutzpilz()
        {
            position = new Vector2(0, 0);
            isVisible = true;        
            

        }

        public void LoadContent(ContentManager Content)
        {
            t1 = Content.Load<Texture2D>("P1");
            t2 = Content.Load<Texture2D>("P2");
            t3 = Content.Load<Texture2D>("P3");
            t4 = Content.Load<Texture2D>("P4");
            t5 = Content.Load<Texture2D>("P5");
            t6 = Content.Load<Texture2D>("P6");
            t7 = Content.Load<Texture2D>("P7");
            t8 = Content.Load<Texture2D>("P8");
            t9 = Content.Load<Texture2D>("P9");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D[] tex = new Texture2D[] { t1, t2, t3, t4, t5, t6, t7, t8, t9 };
            
            position.X = 0;
            position.Y = 350;
            int z = 0;

            for (int i = 0; i < tex.Length/3; i++)
            {
                for (int j = 0; j < tex.Length/3; j++)
                {
                    spriteBatch.Draw(tex[z], position, Color.White);
                    position.X += 43;
                    z++;
                }
                   position.Y += 44;
                   position.X =0; //X Wert zurücksetzen
            }
        }

        public void Update(GameTime gameTime)
        {
        }



    }
}
