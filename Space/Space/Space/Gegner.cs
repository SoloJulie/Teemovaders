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
        public Vector2 position;
        public int gspeed, tempBew;
        public bool isVisible, zurueck;
        public int spalte, zeile;
        public int anzahl, leben, punkte;   
        public int gtyp; //Für Gegnertypdeklaration
        public Rectangle boundingBox;

        public Gegner()
        {
            textur = null;
            position = new Vector2();  // Erscheinungsposition
            position.X = 0;
            position.Y = 0;
            gspeed = 2;
            tempBew = 0;
            anzahl = 0;
            gtyp = 0;
            isVisible = true;
            punkte = 0;
            
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
        } //gibt Position zurück

        public int getX()
        {
            return (int)position.X;
        } //gibt X Position zurück

        public int getY()
        {
            return (int)position.Y;
        } //gibt Y Position zurück


        public void setXPos(int x) //setzt X Position
        {
            position.X = x;
        }

        public void setYPos(int y) //setzt Y Position
        {
            position.Y = y;
        }

        public void machUnsichtbar()
        {
            isVisible = false;
            
        } // Macht Gegner unsichtbar
               

        public Rectangle getBounding()
        {
            return boundingBox;
        }

        public void setTyp(int t)
        {
            gtyp = t;
        } //Setzt Gegner Typ

        public int getGtyp()
        {
            return gtyp;
        } // gibt Gegner Typ

        public void setPunkte()
        {
            if (gtyp == 0)
                punkte = 100;
            else if (gtyp == 1)
                punkte = 200;
            else if (gtyp == 2)
                punkte = 500;
            else if (gtyp == 4) //Item 3 ist Todespilz, keine Punkte für Gegner da vernichtung
                punkte = 600;
        } //legt Punkte für Gegner fest (unterschiedlich je Typ)

        public int addPunkte()
        {
            return punkte;
        } // gibt Punkte

        public void berechnungPunkte() //berehnet die Punkte 
        {
            setPunkte();
            addPunkte();
        }

        public void setzeLeben(int l)
        {
            if (gtyp == 0)
            {
                leben = 1;
            }

            else if (gtyp == 1)
            {
                leben = 2;
            }
            else if (gtyp == 2)
            {
                leben = 2;
            }
            else if (gtyp == 4)
            {
                leben = 2;
            }
        }

        public int getLeben()
        {
            return leben;
        }    
        

        
    }
}
