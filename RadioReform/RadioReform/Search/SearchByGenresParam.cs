using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByGenresParam : ISearchParameter
    {
        private string genre;

        public SearchByGenresParam()
        {

        }

        public SearchByGenresParam(string genre)
        {
            this.genre = genre;
        }

        public bool checkSearchParam(Song song)
        {
            foreach (string genre in song.Genres)
            {
                if (genre.ToLower().Contains(this.genre.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
