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
    public class Spieler
    {
        public Texture2D textur, projektilTextur;
        public Vector2 position;
        public int speed;
        public float pDelay; //Verhindern von Dauerfeuer
        public Rectangle boundingBox;
        public bool kollision;
        public List<Schuss> schussListe;

        public Spieler()
        {
            schussListe = new List<Schuss>();
            textur = null;
            position = new Vector2(300, 400);
            speed = 5;
            kollision = false;
            pDelay = 5;
        }


        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("Teemo");
            projektilTextur = Content.Load<Texture2D>("Projectilrot");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textur, position, Color.White);

            foreach (Schuss p in schussListe)
                p.Draw(spriteBatch);
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
                Schuss nProjektil = new Schuss(projektilTextur);
                nProjektil.position = new Vector2(position.X + 25 - nProjektil.textur.Width / 2, position.Y);

                nProjektil.isVisible = true;
                
                //Maximal 20 Projektile zur selben Zeit möglich
                if (schussListe.Count() < 20)
                    schussListe.Add(nProjektil);
            }

            if (pDelay == 0)
                pDelay = 5;

        }

        //Update Schuss
        public void updateSchussList()
        {
            foreach (Schuss p in schussListe)
            {
                p.position.Y = p.position.Y - p.pSpeed;

                
                //aus Bild raus, werde unsichtbar
                if (p.position.Y <= 0)
                    p.isVisible = false;
            }


            // wenn eine Kugel unsichtbar wird, entferne sie aus der Liste
            for (int i = 0; i < schussListe.Count; i++)
            {
                if (!schussListe[i].isVisible) 
                {
                    schussListe.RemoveAt(i);
                    i--;
                }
            }

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


            //Schuss bei Space
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
