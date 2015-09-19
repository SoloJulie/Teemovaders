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
    public class Gegner
    {
        public Texture2D textur;
        public Vector2 position, bbposition;
        public int speed, tempBew;
        public bool isVisible, zurueck;
        public int spalte;
        public int zeile, anzahl;     
        public List<Gegner> GegnerListe;
        public int gtyp; //Für Gegnertypdeklaration
        public Rectangle boundingBox;

        public Gegner()
        {
            textur = null;
            position = new Vector2();  // Erscheinungsposition
            position.X = 0;
            position.Y = 0;
            speed = 2;
            GegnerListe = new List<Gegner>();
            zeile = 3;
            spalte = 5;
            tempBew = 0;
            zurueck = false;
            anzahl = 0;
            gtyp = 0;
            
        }


        //Content
        public void LoadContent(ContentManager Content)
        {
            //textur = Content.Load<Texture2D>("opfer");
            //boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height);
            
        }


        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //foreach (Gegner gegner in GegnerListe)
            //{
            //    spriteBatch.Draw(textur, gegner.getPos(), Color.White);
            //}
        }

        //Update leer
        public void Update(GameTime gameTime)
        {
            
        }

        

        public Vector2 getPos()
        {
            return position;
        }

        public void setXPos(int x)
        {
            position.X = x;
        }

        public void setYPos(int y)
        {
            position.Y = y;
        }

        public int getX()
        {
            return (int)position.X;
        }

        public int getY()
        {
            return (int)position.Y;
        }

        public int GegnerAnzahl()
        {
            return GegnerListe.Count();
        }

        public void machUnsichtbar()
        {
            isVisible = false;
        }

        public bool visible()
        {
            return isVisible;
        }

        

        public Rectangle getBounding()
        {
            return boundingBox;
        }


        public void setTyp(int t)
        {
            gtyp = t;
        }

        

        
    }
}
