using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByAlbumParam : ISearchParameter
    {
        private string album;

        public SearchByAlbumParam()
        {

        }

        public SearchByAlbumParam(string album)
        {
            this.album = album;
        }

        public bool checkSearchParam(Song song)
        {
            if (song.Album.ToLower().Contains(this.album.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
