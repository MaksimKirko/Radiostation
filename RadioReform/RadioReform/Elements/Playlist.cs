using RadioReform.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Elements
{
    public class Playlist
    {
        private string title;
        private string picturePath;

        private MyCollection<Song> songs;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    title = "Unknown";
                }
                else
                    title = value;
            }
        }

        public string PicturePath
        {
            get
            {
                return picturePath;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    picturePath = "Unknown";
                }
                else
                    picturePath = value;
            }
        }

        public MyCollection<Song> Songs
        {
            get
            {
                return songs;
            }
            set
            {
                songs = value;
            }
        }

        public Playlist()
        {
            Songs = new MyCollection<Song>();
        }

        public Playlist(string name, MyCollection<Song> list)
        {
            Title = name;
            Songs = list;
        }

        public Playlist(string name, string picturePath, MyCollection<Song> list)
        {
            Title = name;
            PicturePath = picturePath;
            Songs = list;
        }
    }
}
