using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Libraries.Unity_Ash.Core;

namespace Ash.Core
{
    public interface IEngine
    {
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
        void AddSystem(ISystem system, int priority);
        void RemoveSystem(ISystem system);
        IEnumerable<T> GetNodes<T>() where T : INode;
    }
}
