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
{ //Managed die einzelnen Sounds, Teemo lachen, Musik, etc
    public class Sounds
    {
        public SoundEffect effect, tl1, tl2, tl3, tl4, lachen, demacia, bewaffnet, tod;
        public Random random;
        public Song bgrSong, menuSong, goSong;
        public int swahl;


        public Sounds()
        { 
           
        }

        public void LoadContent(ContentManager Content)
        {
            tl1 = Content.Load<SoundEffect>("Teemo Lacht");
            tl2 = Content.Load<SoundEffect>("Teemo Lacht 2");
            tl3 = Content.Load<SoundEffect>("Teemo lacht 3");
            tl4 = Content.Load<SoundEffect>("Teemo Dmw");
            tod = Content.Load<SoundEffect>("Teemo Tod");
            bewaffnet = Content.Load<SoundEffect>("Teemo Bewaffnet!");
            demacia = Content.Load<SoundEffect>("Demacia");
            goSong = Content.Load<Song>("GOsong");
            menuSong = Content.Load<Song>("Menusong");
            bgrSong = Content.Load<Song>("Lvl song");
            
        }

        public void playLachen(int i)
        {
            SoundEffect[] lachen = new SoundEffect[] { tl1, tl2, tl3, tl4 };
            lachen[i].Play();
        }

        public void wahlLachen()
        {
            Random random = new Random();
            swahl = random.Next(0, 4); 
        }

    }
         

}
