using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByTagsParam : ISearchParameter
    {
        private string tag;

        public SearchByTagsParam()
        {

        }

        public SearchByTagsParam(string tag)
        {
            this.tag = tag;
        }

        public bool checkSearchParam(Song song)
        {
            foreach (string tag in song.Tags)
            {
                if (tag.ToLower().Contains(this.tag.ToLower()))
                {
                    return true;
                }                
            }
            return false;
        }
    }
}
