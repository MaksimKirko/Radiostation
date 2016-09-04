using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Sort
{
    class PlaysComparer : IComparer<Song>
    {
        public int Compare(Song s1, Song s2)
        {
            return s1.NumOfPlays.CompareTo(s2.NumOfPlays);
        }
    }
}
