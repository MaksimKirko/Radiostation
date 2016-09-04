using RadioReform.Collections;
using RadioReform.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Sort
{
    class ParameterizedSorting<T> : IParameterizedSorting<T>
    {
        private IComparer<T> element;
        private MyCollection<T> sortableList;
        private bool order;

        public ParameterizedSorting()
        {

        }

        public ParameterizedSorting(IComparer<T> element, MyCollection<T> sortableList, bool order)
        {
            this.element = element;
            this.sortableList = sortableList;
            this.order = order;
        }

        public void SetParams(IComparer<T> element, MyCollection<T> sortableList, bool order)
        {
            this.element = element;
            this.sortableList = sortableList;
            this.order = order;
        }

        public void Sort()
        {
            if (sortableList != null)
            {
                sortableList.Sort(element);
                //sortParams.Sort(sortableList, order);
            }
            if (!order)
            {
                sortableList.Reverse();
            }
            
        }
    }
}
