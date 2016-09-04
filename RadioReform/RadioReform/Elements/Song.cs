using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Elements
{
    public class Song
    {
        private string title;
        private string artist;
        private string album;
        private int year;
        private int duration;
        private double raiting;
        private int numOfPlays;
        private int id;
        private string filename;
        private List<string> genres;
        private List<string> categories;
        private List<string> tags;

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

        public string Artist
        {
            get
            {
                return artist;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    artist = "Unknown";
                }
                else
                    artist = value;
            }
        }

        public string Album
        {
            get
            {
                return album;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    album = "Unknown";
                }
                else
                    album = value;
            }
        }

        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value < 0)
                {
                    year = Math.Abs(value);
                }
                else
                    year = value;
            }
        }

        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        public double Raiting
        {
            get
            {
                return raiting;
            }
            set
            {
                if (value < 0)
                {
                    raiting = Math.Abs(value);
                }
                else
                    raiting = value;
            }
        }

        public int NumOfPlays
        {
            get
            {
                return numOfPlays;
            }
            set
            {
                if (value < 0)
                {
                    numOfPlays = Math.Abs(value);
                }
                else
                    numOfPlays = value;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                if (value < 0)
                {
                    id = Math.Abs(value);
                }
                else
                    id = value;
            }
        }

        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    filename = "Unknown";
                }
                else
                    filename = value;
            }
        }

        public List<string> Genres
        {
            get
            {
                return genres;
            }
            set
            {
                genres = value;
            }
        }

        public List<string> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
            }
        }

        public List<string> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
            }
        }

        public Song()
        {
            Year = 0;
            NumOfPlays = 0;
            Raiting = 0.00;
            Duration = 0;
            ID = 0;
            Genres = new List<string>();
            Categories = new List<string>();
            Tags = new List<string>();
        }

        public Song(string title, string artist, string album, int year, int duration, double raiting, int numOfPlays,
            int id, string filename, List<string> genres, List<string> categories, List<string> tags)
        {
            Title = title;
            Artist = artist;
            Album = album;
            Year = year;
            Duration = duration;
            Raiting = raiting;
            NumOfPlays = numOfPlays;
            ID = id;
            Filename = filename;
            Genres = genres;
            Categories = categories;
            Tags = tags;
        }

        public Song ParseFromFile(string filename)
        {
            Song newSong = new Song();

            TagLib.File audioFile = TagLib.File.Create(filename);
            newSong.Title = audioFile.Tag.Title;
            newSong.Artist = String.Join(", ", audioFile.Tag.Performers);
            newSong.Album = audioFile.Tag.Album;
            newSong.Year = Convert.ToInt32(audioFile.Tag.Year);
            newSong.Duration = Convert.ToInt32(audioFile.Properties.Duration.TotalSeconds);
            newSong.Filename = filename;

            return newSong;
        }

        public List<string> ParseFromAddEditBlock(string line)
        {
            List<string> results = new List<string>();

            string[] separators = new string[1]; separators[0] = ", ";
            string[] items = line.Split(separators, 10, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in items)
            {
                results.Add(s);
            }

            return results;
        }
    }
}
