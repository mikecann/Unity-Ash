using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Core
{
    public class NodeList<T> : INodeList<T>
    {
        private HashSet<T> _nodes;

        public NodeList()
        {
            _nodes = new HashSet<T>();
            NodeAddedEvent = new NodeAdded<T>();
            NodeRemovedEvent = new NodeRemoved<T>();
        }

        internal void Add(T node)
        {
            NodeAddedEvent.Invoke(node);
            _nodes.Add(node);
        }

        internal void Remove(T node)
        {
            NodeRemovedEvent.Invoke(node);
            _nodes.Remove(node);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public NodeAdded<T> NodeAddedEvent { get; private set; }
        public NodeRemoved<T> NodeRemovedEvent { get; private set; }
    }
}
