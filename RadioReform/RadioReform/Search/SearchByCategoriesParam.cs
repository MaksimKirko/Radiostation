using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class SearchByCategoriesParam : ISearchParameter
    {
        private string category;

        public SearchByCategoriesParam()
        {

        }

        public SearchByCategoriesParam(string category)
        {
            this.category = category;
        }

        public bool checkSearchParam(Song song)
        {
            foreach (string ct in song.Categories)
            {
                if (ct.ToLower().Contains(this.category.ToLower()))
                {
                    return true;
                }                
            }
            return false;
        }
    }
}
