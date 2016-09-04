using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByYearParam : ISearchParameter
    {
        private string year;

        public SearchByYearParam()
        {

        }

        public SearchByYearParam(string year)
        {
            this.year = year;
        }

        public bool checkSearchParam(Song song)
        {
            if (song.Year.ToString().Contains(this.year))
            {
                return true;
            }
            return false;
        }
    }
}
