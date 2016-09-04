using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Collections
{
    public class PlaylistCollection
    {
        public MyCollection<Playlist> playlists;

        public PlaylistCollection()
        {
            playlists = new MyCollection<Playlist>();
        }

        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (Playlist p in playlists)
            {
                names.Add(p.Title);
            }
            return names;
        }

        public Playlist FindByName(string name)
        {
            foreach (Playlist p in playlists)
            {
                if (name == p.Title)
                {
                    return p;
                }
            }
            return null;
        }

        public Song FindSongByName(string sName, string pName)
        {
            Playlist p = this.FindByName(pName);
            foreach (Song s in p.Songs)
            {
                if (sName == s.Title)
                {
                    return s;
                }
            }
            return null;
        }

        public Song FindSongByName(string name, Playlist p)
        {
            foreach (Song s in p.Songs)
            {
                if (name == s.Title)
                {
                    return s;
                }
            }
            return null;
        }

        public Playlist Make(string name, List<string> criteries, List<Song> songs)
        {
            Playlist pl = new Playlist();
            pl.Title = name;
            foreach (Song s in songs)
            {
                if (isSuit(s, criteries))
                {
                    pl.Songs.Add(s);
                }
            }
            return pl;
        }

        private bool isSuit(Song song, List<string> criteries)
        {
            foreach (string s in criteries)
            {
                s.ToLower();
            }
            if (criteries.Contains(song.Raiting.ToString()) || criteries.Contains(song.Year.ToString()))
            {
                Console.WriteLine("Crit find!");
                return true;
            }
            foreach (string s in song.Tags)
            {
                if (criteries.Contains(s.ToLower()))
                {
                    Console.WriteLine("Crit find!");
                    return true;
                }
            }
            foreach (string s in song.Categories)
            {
                if (criteries.Contains(s.ToLower()))
                {
                    Console.WriteLine("Crit find!");
                    return true;
                }
            }
            foreach (string s in song.Genres)
            {
                if (criteries.Contains(s.ToLower()))
                {
                    Console.WriteLine("Crit find!");
                    return true;
                }
            }
            return false;
        }
    }
}
