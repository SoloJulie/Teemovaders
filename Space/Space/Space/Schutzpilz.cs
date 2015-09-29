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
    //KLasse Schutzpilz um 3 Pilze zu zeichnen die den Spieler schützen. Bestehen aus je 9 einzelnen gleichgroßen Texturen
    public class Schutzpilz
    {
        public Texture2D t1, t2, t3, t4, t5, t6, t7, t8, t9, tex; //Pilz Texturen + Array
        public Vector2 position;
        public bool[] texVisi; //Array für sichtbarkeit
        public int x, y, a, b, c, d, tw, th; //Positionen für die Pilz Rectangles 
        public List<Rectangle> Listebb; //Rectangle Liste um einzelne Boundingboxen ansprechen zu können

        public Schutzpilz()
        {
            texVisi = new bool[27]; //27 Boxen - Bilder
            position = new Vector2();

            //Array 27 mit true füllen
            for (int i = 0; i < 27; i++)
                texVisi[i] = true;

            Listebb = new List<Rectangle>();

            //Pilz
            x = 0; //X Position 1. Pilztextur
            y = 300; //Y Position 1. Piltextur
            tw = 34; //Texturbreite
            th = 35; //Texturhöhe

            //1. Pilz
            //1. Reihe
            Rectangle p111 = new Rectangle(x, y, tw, th); //Erster Pilz, Erste Reihe, Erste Spalte
            Listebb.Add(p111);
            Rectangle p112 = new Rectangle(x + tw, y, tw, th);
            Listebb.Add(p112);
            Rectangle p113 = new Rectangle(x + (tw * 2), y, tw, th);
            Listebb.Add(p113);
            //2. Reihe
            Rectangle p121 = new Rectangle(x, y + th, tw, th);
            Listebb.Add(p121);
            Rectangle p122 = new Rectangle(x + tw, y + th, tw, th);
            Listebb.Add(p122);
            Rectangle p123 = new Rectangle(x + (tw * 2), y + th, tw, th);
            Listebb.Add(p123);
            //3. Reihe
            Rectangle p131 = new Rectangle(x, y + (th * 2), tw, th);
            Listebb.Add(p131);
            Rectangle p132 = new Rectangle(x + tw, y + (th * 2), tw, th);
            Listebb.Add(p132);
            Rectangle p133 = new Rectangle(x + (tw * 2), y + (th * 2), tw, th);
            Listebb.Add(p133);

            //2. Pilz
            a = 300;
            b = 300;

            //1. Reihe
            Rectangle p211 = new Rectangle(a, b, tw, th); //Zweiter Pilz, Erste Reihe, Erste Spalte
            Listebb.Add(p211);
            Rectangle p212 = new Rectangle(a + tw, b, tw, th);
            Listebb.Add(p212);
            Rectangle p213 = new Rectangle(a + (tw * 2), b, tw, th);
            Listebb.Add(p213);
            //2. Reihe
            Rectangle p221 = new Rectangle(a, b + th, tw, th);
            Listebb.Add(p221);
            Rectangle p222 = new Rectangle(a + tw, b + th, tw, th);
            Listebb.Add(p222);
            Rectangle p223 = new Rectangle(a + (tw * 2), b + th, tw, th);
            Listebb.Add(p223);
            //3. Reihe
            Rectangle p231 = new Rectangle(a, b + (th * 2), tw, th);
            Listebb.Add(p231);
            Rectangle p232 = new Rectangle(a + tw, b + (th * 2), tw, th);
            Listebb.Add(p232);
            Rectangle p233 = new Rectangle(a + (tw * 2), b + (th * 2), tw, th);
            Listebb.Add(p233);

            //3. Pilz
            c = 600;
            d = 300;

            //1. Reihe
            Rectangle p311 = new Rectangle(c, d, tw, th); //Zweiter Pilz, Erste Reihe, Erste Spalte
            Listebb.Add(p311);
            Rectangle p312 = new Rectangle(c + tw, d, tw, th);
            Listebb.Add(p312);
            Rectangle p313 = new Rectangle(c + (tw * 2), d, tw, th);
            Listebb.Add(p313);
            //2. Reihe
            Rectangle p321 = new Rectangle(c, d + th, tw, th);
            Listebb.Add(p321);
            Rectangle p322 = new Rectangle(c + tw, d + th, tw, th);
            Listebb.Add(p322);
            Rectangle p323 = new Rectangle(c + (tw * 2), d + th, tw, th);
            Listebb.Add(p323);
            //3. Reihe
            Rectangle p331 = new Rectangle(c, d + (th * 2), tw, th);
            Listebb.Add(p331);
            Rectangle p332 = new Rectangle(c + tw, d + (th * 2), tw, th);
            Listebb.Add(p332);
            Rectangle p333 = new Rectangle(c + (tw * 2), d + (th * 2), tw, th);
            Listebb.Add(p333);
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
        } //Lädt Textur


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D[] tex = new Texture2D[] { t1, t2, t3, t4, t5, t6, t7, t8, t9 };
            int h = 0; //Laufvbariable um sichtbarkeit der Pilztextur zu testen, da nur dann gezeichnet wird

            //1. Pilz
            position.X = x; //0
            position.Y = y; //300
            int z = 0;

            for (int i = 0; i < tex.Length / 3; i++) //Zeichne 3 pro
            {
                for (int j = 0; j < tex.Length / 3; j++) //Reihen
                {
                    if (texVisi[h] == true)
                    {
                        spriteBatch.Draw(tex[z], position, Color.White);      //zeichne an position z                                        
                    }
                    position.X += tw; //weiterrücken der x position um breite einzelnen Pilztextur
                    z++;
                    h++;
                }
                position.Y += tw; //weiterrücken der y position um höhe einzelnen Pilztextur
                position.X = x; //X Wert zurücksetzen   
                //h = 9
            }

            //2. Pilz
            position.X = a;
            position.Y = b;
            int m = 0;

            for (int i = 0; i < tex.Length / 3; i++)
            {
                for (int j = 0; j < tex.Length / 3; j++)
                {
                    if (texVisi[h] == true)
                    {
                        spriteBatch.Draw(tex[m], position, Color.White);
                    }
                    position.X += tw;
                    m++;
                    h++;
                }
                position.Y += tw;
                position.X = a; //X Wert zurücksetzen
            } //h = 18

            //3. Pilz
            position.X = c;
            position.Y = d;
            int n = 0;

            for (int i = 0; i < tex.Length / 3; i++)
            {
                for (int j = 0; j < tex.Length / 3; j++)
                {
                    if (texVisi[h] == true)
                    {
                        spriteBatch.Draw(tex[n], position, Color.White);
                    }
                    position.X += tw;
                    n++;
                    h++;
                }
                position.Y += th;
                position.X = c; //X Wert zurücksetzen
            } //h = 27

        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
