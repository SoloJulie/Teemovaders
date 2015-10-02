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
    //Bestimmt welcher Hintergrund gezeichnet wird, je nach übergebener Statusvariable aus der Game Klasse
    class Hintergrund
    {
        public Texture2D textur, titel, gameover, backLvl2, backlvl3, gewonnen;
        public Vector2 position;
        public int status;


        public Hintergrund()
        {
            textur = null;
            titel = null;
            gameover = null;
            backLvl2 = null;
            backlvl3 = null;
            gewonnen = null;
            position = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            textur = Content.Load<Texture2D>("lvl1");
            titel = Content.Load<Texture2D>("Titelbild");
            gameover = Content.Load<Texture2D>("GameOver");
            backLvl2 = Content.Load<Texture2D>("forest");
            backlvl3 = Content.Load<Texture2D>("dark-forest");
            gewonnen = Content.Load<Texture2D>("Victory700"); 
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch, int status)
        {
            this.status = status;

            if (status == 1) //Menue
            {
                spriteBatch.Draw(titel, position, Color.White);
            }

            else if (status == 2) //spielen, lvl1
            {            
                spriteBatch.Draw(textur, position, Color.White);
            }

            else if (status == 22 || status == 21) //spielen, lvl2
            {
                spriteBatch.Draw(backLvl2, position, Color.White);
            }

            else if (status == 23) //spielen, lvl2
            {
                spriteBatch.Draw(backlvl3, position, Color.White);
            }
            
            else if (status == 3) //gameOver
            {
                spriteBatch.Draw(gameover, position, Color.White);
            }
            else if (status == 4) //Gewonnen
            {
                spriteBatch.Draw(gewonnen, position, Color.White);
            }
        }

        //Update
        public void Update(GameTime gameTime)
        {
        }
    }
}
