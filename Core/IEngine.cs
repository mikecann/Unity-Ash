using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IEngine
    {
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
        void AddSystem(ISystem system, int priority);
        void RemoveSystem(ISystem system);
        IEnumerable<T> GetNodes<T>();
    }
}
