using RadioReform.Collections;
using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    class ParameterizedSearching : IParameterizedSearching<Song>
    {
        public ParameterizedSearching()
        {

        }

        public MyCollection<Song> getSearchResults(Dictionary<string, ISearchParameter> searchParams, MyCollection<Song> collection)
        {
            MyCollection<Song> results = new MyCollection<Song>();

            foreach (KeyValuePair<string, ISearchParameter> kvp in searchParams)
            {
                foreach (Song song in collection)
                {
                    if (kvp.Value.checkSearchParam(song) && !results.Contains(song))
                    {
                        results.Add(song);
                    }
                }
            }

            return results;
        }
    }
}
