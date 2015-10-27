using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Libraries.Unity_Ash.Core;

namespace Ash.Core
{
    public class Node : INode
    {
        public IEntity Entity { get; set; }
    }

    public class Node<T> : Node
    {
        public T Component1 { get; set; }
    }

    public class Node<T1,T2> : Node<T1>
    {
        public T2 Component2 { get; set; }
    }

    public class Node<T1,T2,T3> : Node<T1, T2>
    {
        public T3 Component3 { get; set; }
    }

    public class Node<T1, T2, T3, T4> : Node<T1, T2, T3>
    {
        public T4 Component4 { get; set; }
    }

    public class Node<T1, T2, T3, T4, T5> : Node<T1, T2, T3, T4>
    {
        public T5 Component5 { get; set; }
    }

    public class Node<T1, T2, T3, T4, T5, T6> : Node<T1, T2, T3, T4, T5>
    {
        public T6 Component6 { get; set; }
    }
}
