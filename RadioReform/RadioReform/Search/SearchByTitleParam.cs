using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByTitleParam : ISearchParameter
    {
        private string title;

        public SearchByTitleParam()
        {

        }

        public SearchByTitleParam(string title)
        {
            this.title = title;
        }

        public bool checkSearchParam(Song song)
        {
            if (song.Title.ToLower().Contains(this.title.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}
