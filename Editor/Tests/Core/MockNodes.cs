using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class MockNode
    {
    }

    public class MockNode<T>
    {
        public T component1;
    }

    public class MockNode<T1,T2>
    {
        public T1 component1;
        public T2 component2;
    }

    public class MockNode<T1,T2,T3>
    {
        public T1 component1;
        public T2 component2;
        public T3 component3;
    }

    public class MockNodeWithProperties<T>
    {
        public T Component1 { get; set; }
    }

    public class MockNodeWithProperties<T1, T2>
    {
        public T1 Component1 { get; set; }
        public T2 Component2 { get; set; }
    }

    public class MockNodeWithProperties<T1, T2, T3>
    {
        public T1 Component1 { get; set; }
        public T2 Component2 { get; set; }
        public T3 Component3 { get; set; }
    }

    
}
