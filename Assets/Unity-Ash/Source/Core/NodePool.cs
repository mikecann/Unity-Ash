using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class NodePool<T> : INodePool<T>
    {
        public T UnPool()
        {
            return Activator.CreateInstance<T>();
        }

        public void Pool(T node)
        {
        }
    }
}
