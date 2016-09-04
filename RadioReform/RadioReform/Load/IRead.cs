using RadioReform.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Load
{
    interface IRead<T>
    {
        MyCollection<T> GetItems();
    }
}
