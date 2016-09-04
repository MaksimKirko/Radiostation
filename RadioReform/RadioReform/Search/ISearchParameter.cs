using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Search
{
    interface ISearchParameter
    {
        bool checkSearchParam(Song song);
    }
}
