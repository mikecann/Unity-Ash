using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IFamily
    {
        void EntityAdded(IEntity entity);
        void EntityRemoved(IEntity entity);
    }

    public interface IFamily<T> : IFamily
    {
        IEnumerable<T> Nodes { get; }
    }
}
