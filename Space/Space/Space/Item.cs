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
        public Texture2D prot,pgruen,pblau,ptot,pilze;
        public Vector2 position;
        public int ispeed, x, y;
        public Rectangle boundingBox;
        public bool isVisible;
        public int iTyp, wahl, iPunkte;
        public Random random;


        public Item(int i)
        {
            prot = null;
            pgruen = null;
            pblau = null;
            ptot = null;

            Random random = new Random();
            x = random.Next(1, 500);
            y = random.Next(1, 250);
            position = new Vector2(x, y);
            ispeed = 1;
            isVisible = true;
            iTyp = i;
            iPunkte = 0;
            
        }

        public void LoadContent(ContentManager Content)
        {
            prot = Content.Load<Texture2D>("Pilzrot");      //iT = 1
            pblau = Content.Load<Texture2D>("Pilzblau");    //iT = 2
            ptot = Content.Load<Texture2D>("Pilztot");      //iT = 3
            pgruen = Content.Load<Texture2D>("Pilzgrün");     //iT = 4
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
            {
                if (iTyp == 1)
                {
                    spriteBatch.Draw(prot, position, Color.White);
                }
                else if (iTyp == 2)
                {
                    spriteBatch.Draw(pblau, position, Color.White);
                }

                else if (iTyp == 3)
                {
                    spriteBatch.Draw(ptot, position, Color.White);
                }

                else if (iTyp == 4)
                {
                    spriteBatch.Draw(pgruen, position, Color.White);
                }
            }
            
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

        public void auswahl()
        {
            wahl = random.Next(1, 5); //5 nicht inklusive
        }

        public void setTyp(int i)
        {
            iTyp = i;
        }

        public int getTyp()
        {
            return iTyp;
        }

        //public int randXPosition()
        //{
        //    return position.X = random.Next(1, 750);
        //}

        //public int randYPosition()
        //{
        //    return y = random.Next(1, 500);
        //}


        public void setIPunkte()
        {            
            if (iTyp == 1)
                iPunkte = 200;
            else if (iTyp == 2)
                iPunkte = 500;
            else if (iTyp == 3)
                iPunkte = 700;
            else if (iTyp == 4) 
                iPunkte = 1000;
        }

        public int addIPunkte()
        {
            setIPunkte();
            return iPunkte;
        }

        public void berechnungIPunkte()
        {
            if (isVisible == true)
            {
                setIPunkte();
                addIPunkte();
            }
        } 


        

    }
}
