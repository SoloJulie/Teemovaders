using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Space
{
    public class Spieler
    {
        public Texture2D textur, superT, texProjektil, superProjekt;
        public Vector2 position;
        public int speed, leben, spTyp;
        public float pDelay, pD; //Verhindern von Dauerfeuer
        public Rectangle boundingBox;
        public List<Schuss> ListeSchuss; //Liste um Projektile besser händeln zu können
        public bool isVisible;
        public SoundEffect effect;     



        public Spieler()
        {
            ListeSchuss = new List<Schuss>(); 
            textur = null;
            superT = null;
            superProjekt = null;
            position = new Vector2(300, 450); //Teemo Sporn 300,450 x,y
            speed = 5;
            pD = 25; // Um änderungen in Schussmethode nicht 2mal durch Wert Ändern zu müssen, nur zur Wertanpassung / vereinfachung gedacht
            pDelay = pD;
            isVisible = true;
            leben = 3;
            spTyp = 1;
        }


        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Teemo");
            superT = Content.Load<Texture2D>("Super Teemo");
            texProjektil = Content.Load<Texture2D>("teemoPfeil");
            superProjekt = Content.Load<Texture2D>("Super Teemo - Emblem");  
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (spTyp == 1)
            {
                spriteBatch.Draw(textur, position, Color.White);
                foreach (Schuss p in ListeSchuss)
                    p.Draw(spriteBatch, texProjektil);
            }

            else 
            {
                spriteBatch.Draw(superT, position, Color.White);

                foreach (Schuss p in ListeSchuss)
                    p.Draw(spriteBatch, superProjekt);
            }

            
        }

        

        //Schießen Methode
        public void Schuss()
        {
            //Schießt nur wenn Delay auf null ist
            if (pDelay >= 0)
            {
                pDelay--;
            }

            // delay ist 0, neuer Schuss sichtbar und in Liste schreiben
            if (pDelay <= 0)
            {
                Schuss nProjektil = new Schuss(texProjektil);
                nProjektil.position = new Vector2(position.X + 25 - nProjektil.textur.Width / 2, position.Y); //Schuss aus der Mitte von Teemo auslösen

                nProjektil.isVisible = true;

                
                //Maximal 20 Projektile zur selben Zeit möglich
                if (ListeSchuss.Count() < 20)
                    ListeSchuss.Add(nProjektil);
            }

            if (pDelay == 0)
                pDelay = pD;

        }

        //Update Schuss
        public void updateSchussList()
        {
            foreach (Schuss p in ListeSchuss)
            {
                //Bounding Box um die Projektile
                p.boundingBox = new Rectangle((int)p.position.X, (int)p.position.Y, p.textur.Width, p.textur.Height);
                
                
                //Jedes Projektil mit Geschwindigkeit versehen
                p.position.Y = p.position.Y - p.pSpeed;

                
                //aus Bild raus, werde unsichtbar
                if (p.position.Y <= 0)
                    p.isVisible = false;
            }


            // wenn eine Kugel unsichtbar wird, entferne sie aus der Liste
            for (int i = 0; i < ListeSchuss.Count; i++)
            {
                if (ListeSchuss[i].isVisible == false) //Wenn Projektil an Stelle i nicht sichtbar ist, entferne sie aus der Liste, setze i--
                {
                    ListeSchuss.RemoveAt(i);
                    i--;
                }
            }

        }

        public List<Schuss> getSchussListe()
        {
            return ListeSchuss;
        }


        //Update (Keyboard einstellungen)
        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            // Bewegung Teemo
            if (kb.IsKeyDown(Keys.Left))
                position.X = position.X - speed;

            if (kb.IsKeyDown(Keys.Right))
                position.X = position.X + speed;
            
            //BoundingBox Spieler
            boundingBox = new Rectangle((int)position.X, (int)position.Y, textur.Width, textur.Height); 


            //Schuss bei Leertaste
            if (kb.IsKeyDown(Keys.Space))
            {
                Schuss();
            }

            updateSchussList();


            //Spieler bleibt innerhalb des Bildschirms
            if (position.X <= 0)
                position.X = 0;
            if (position.X >= 700 - textur.Width)
                position.X = 700 - textur.Width;

        }

        
    }
}
