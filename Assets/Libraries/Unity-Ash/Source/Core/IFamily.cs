using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IFamily
    {
        void ComponentAdded(IEntity entity, Type componentType);
        void ComponentRemoved(IEntity entity, Type componentType);
        void EntityAdded(IEntity entity);
        void EntityRemoved(IEntity entity);
    }

    public interface IFamily<T> : IFamily
    {
        INodeList<T> Nodes { get; }
    }
}
