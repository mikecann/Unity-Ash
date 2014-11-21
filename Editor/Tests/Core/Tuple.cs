using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.RichardLord.AshTests.Core
{
    struct Tuple<T1,T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
