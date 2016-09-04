using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Collections
{
    public class MusicCollection
    {
        public MyCollection<Song> songs;

        public MusicCollection()
        {
            songs = new MyCollection<Song>();
        }

        public Song FindByName(string name)
        {
            foreach (Song s in songs)
            {
                if (name == s.Title)
                {
                    return s;
                }
            }
            return null;
        }
    }
}
