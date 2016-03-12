using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Ash.Core
{
    public interface INodeList<T> : IEnumerable<T>
    {
        NodeAdded<T> NodeAddedEvent { get; }
        NodeRemoved<T> NodeRemovedEvent { get; }
    }
}
