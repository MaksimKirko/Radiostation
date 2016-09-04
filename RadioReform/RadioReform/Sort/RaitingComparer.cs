using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Sort
{
    class RaitingComparer : IComparer<Song>
    {
        public int Compare(Song s1, Song s2)
        {
            return s1.Raiting.CompareTo(s2.Raiting);
        }
    }
}
