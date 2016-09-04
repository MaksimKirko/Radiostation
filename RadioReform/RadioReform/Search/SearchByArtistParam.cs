using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByArtistParam : ISearchParameter
    {
        private string artist;

        public SearchByArtistParam()
        {

        }

        public SearchByArtistParam(string artist)
        {
            this.artist = artist;
        }

        public bool checkSearchParam(Song song)
        {
            if (song.Artist.ToLower().Contains(this.artist.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
