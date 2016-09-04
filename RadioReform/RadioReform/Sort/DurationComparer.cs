using RadioReform.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioReform.Sort
{
    class DurationComparer : IComparer<Song>
    {
        public int Compare(Song s1, Song s2)
        {
            return s1.Duration.CompareTo(s2.Duration);
        }
    }
}
