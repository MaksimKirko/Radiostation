using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Collections
{
    public class MyCollection<T> : List<T>
    {
        public MyCollection()
        { }

        public void AddItem(T t)
        {
            this.Add(t);
        }

        public void EditItem(T t1, T t2)
        {
            if (this.Contains(t1))
            {
                this[this.IndexOf(t1)] = t2;
            }
        }

        public void DeleteItem(T t)
        {
            if (this.Contains(t))
            {
                this.Remove(t);
            }
        }
    }
}
