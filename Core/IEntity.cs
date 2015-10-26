using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IEntity
    {
        bool HasComponent(Type type);
        object GetComponent(Type type);
        ComponentAdded ComponentAdded { get; }
        ComponentRemoved ComponentRemoved { get; }
    }
}
