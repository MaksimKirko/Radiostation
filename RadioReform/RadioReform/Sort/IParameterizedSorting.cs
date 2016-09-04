using RadioReform.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Sort
{
    interface IParameterizedSorting<T>
    {
        void SetParams(IComparer<T> element, MyCollection<T> sortableList, bool order);
        void Sort();
    }
}
