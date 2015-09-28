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
        public bool texVisi1, texVisi2, texVisi3, texVisi4, texVisi5, texVisi6, texVisi7, texVisi8, texVisi9, texVisi;
        public int i;


        public Schutzpilz()
        {
            position = new Vector2(0, 0);
            isVisible = true;
            texVisi1 = true;
            texVisi2 = true;
            texVisi3 = true;
            texVisi4 = true;
            texVisi5 = true;
            texVisi6 = true;
            texVisi7 = false;
            texVisi8 = true;
            texVisi9 = true;
            i = 0;
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
            bool[] texVisi = new bool[] { texVisi1, texVisi2, texVisi3, texVisi4, texVisi5, texVisi6, texVisi7, texVisi8, texVisi9 };
            
            
            position.X = 0;
            position.Y = 300;
            int z = 0;
            int t =6;
            

            
                boundingBox = new Rectangle((int)getX(), (int)getY(), t1.Width, t1.Height);
                for (int i = 0; i < tex.Length / 3; i++)
                {
                    if (texVisi[i] == true)
                    {
                        t++;
                        for (int j = 0; j < tex.Length / 3; j++) //Reihen
                        {
                            spriteBatch.Draw(tex[z], position, Color.White);
                            position.X += t1.Width;
                            z++;
                        }
                        position.Y += t1.Height;
                        position.X = 0; //X Wert zurücksetzen
                    }
                }

                //2. Pilz
                position.X = 300;
                position.Y = 300;
                int m = 0;
                boundingBox = new Rectangle((int)getX(), (int)getY(), t1.Width, t1.Height);

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
                position.X = 600;
                position.Y = 300;
                int n = 0;
                boundingBox = new Rectangle((int)getX(), (int)getY(), t1.Width, t1.Height);

                for (int i = 0; i < tex.Length / 3; i++)
                {
                    for (int j = 0; j < tex.Length / 3; j++)
                    {
                        spriteBatch.Draw(tex[n], position, Color.White);
                        position.X += t1.Width;
                        n++;
                    }
                    position.Y += t1.Height;
                    position.X = 600; //X Wert zurücksetzen
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

        //public bool sichtbar(int i)
        //{
        //    int temp = i;
        //    return texVisi[temp];
        //}

        //public bool test()
        //{
        //    return (bool)texVisi[i];
        //}





    }
}
