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
            position.Y = 300;
            int z = 0;

            if (isVisible == true)
            {
                for (int i = 0; i < tex.Length / 3; i++)
                {
                    for (int j = 0; j < tex.Length / 3; j++)
                    {
                        spriteBatch.Draw(tex[z], position, Color.White);
                        position.X += t1.Width;
                        z++;
                    }
                    position.Y += t1.Height;
                    position.X = 0; //X Wert zurücksetzen
                }

                //2. Pilz
                position.X = 300;
                position.Y = 300;
                int m = 0;

                for (int i = 0; i < tex.Length / 3; i++)
                {
                    for (int j = 0; j < tex.Length / 3; j++)
                    {
                        spriteBatch.Draw(tex[m], position, Color.White);
                        position.X += t1.Width;
                        m++;
                    }
                    position.Y += t1.Height;
                    position.X = 300; //X Wert zurücksetzen
                }

                //3. Pilz
                position.X = 575;
                position.Y = 300;
                int n = 0;

                for (int i = 0; i < tex.Length / 3; i++)
                {
                    for (int j = 0; j < tex.Length / 3; j++)
                    {
                        spriteBatch.Draw(tex[n], position, Color.White);
                        position.X += t1.Width;
                        n++;
                    }
                    position.Y += t1.Height;
                    position.X = 575; //X Wert zurücksetzen
                }
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public int getX()
        {
            return (int)position.X;
        }

        public int getY()
        {
            return (int)position.Y;
        }

        public bool sichtbar()
        {
            return isVisible;
        }




    }
}
