using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    public interface IEntity
    {
        bool Has(Type type);
        object Get(Type type);
        T Add<T>() where T : Component;
        void Remove(Component component);
        ComponentAdded ComponentAdded { get; }
        ComponentRemoved ComponentRemoved { get; }
    }
}
