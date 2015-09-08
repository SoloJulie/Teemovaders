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
    public class GegContainer
    {
        public Texture2D textur;
        public Vector2 position;
        public int speed;
        public List<Gegner> GegnerListe ;
        public int spalte; 
        public int zeile, anzahl;

        public GegContainer()
        {
            GegnerListe = new List<Gegner>();
            zeile = 3;
            spalte = 5;
            anzahl = 0;
        }


        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("opfer");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Gegner gegner in GegnerListe)
            {
                spriteBatch.Draw(textur, gegner.getPos(), Color.White);                
            }
            //Sporn();
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //GegnerListe.Count();
            //Sporn();
            //for (int z = 0; z < zeile; zr++)
            //    for (int s = 0; c < spalte; s++)
            //    {
            //        if (direction.Equals("RIGHT"))
            //            rectinvader[r, c].X = rectinvader[r, c].X + invaderspeed;
            //        if (direction.Equals("LEFT"))
            //            rectinvader[r, c].X = rectinvader[r, c].X - invaderspeed;
            //    }
        }

        public void Sporn()
        {
            for (int y = 0; y <= zeile; y++) //Beginn bei 20px Abstand oben, max px nach unten, 70px erhöhen ( Abstand zwischen Opfern)
            {
                for (int x = 0; x <= spalte; x++)
                {
                    Gegner gegner = new Gegner();
                    gegner.setXPos(x * 80);
                    gegner.setYPos(y * 80);
                    GegnerListe.Add(gegner);
                }
            }  
         }
        

        public int GegnerAnzahl()
        {
            return GegnerListe.Count();
        }

        
    }

}
