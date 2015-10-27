using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface INodePool<T>
    {
        T UnPool();
        void Pool(T node);
    }
}
