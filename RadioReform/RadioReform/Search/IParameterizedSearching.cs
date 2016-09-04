using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioReform.Collections;

namespace RadioReform.Search
{
    interface IParameterizedSearching<T>
    {
        MyCollection<T> getSearchResults(Dictionary<string, ISearchParameter> searchParams, MyCollection<T> collection);
    }
}
