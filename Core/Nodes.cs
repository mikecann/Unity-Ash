using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class Node
    {
    }

    public class Node<T>
    {
        public T component1;
    }

    public class Node<T1,T2>
    {
        public T1 component1;
        public T2 component2;
    }

    public class Node<T1,T2,T3>
    {
        public T1 component1;
        public T2 component2;
        public T3 component3;
    }

    public class Node<T1, T2, T3, T4>
    {
        public T1 component1;
        public T2 component2;
        public T3 component3;
        public T4 component4;
    }

    public class Node<T1, T2, T3, T4, T5>
    {
        public T1 component1;
        public T2 component2;
        public T3 component3;
        public T4 component4;
        public T5 component5;
    }

    public class Node<T1, T2, T3, T4, T5, T6>
    {
        public T1 component1;
        public T2 component2;
        public T3 component3;
        public T4 component4;
        public T5 component5;
        public T6 component6;
    }
}
