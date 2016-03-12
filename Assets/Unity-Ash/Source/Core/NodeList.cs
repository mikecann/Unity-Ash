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
        enum PendingChange
        {
            Add,
            Remove
        }

        public bool IsLocked { get; private set; }

        private HashSet<T> _nodes;
        private List<KeyValuePair<T, PendingChange>> _pending;

        public NodeList()
        {
            _nodes = new HashSet<T>();
            NodeAddedEvent = new NodeAdded<T>();
            NodeRemovedEvent = new NodeRemoved<T>();
            _pending = new List<KeyValuePair<T, PendingChange>>();
        }

        public void Add(T node)
        {
            if (IsLocked)
            {
                _pending.Add(new KeyValuePair<T, PendingChange>(node, PendingChange.Add));
            }
            else
            {
                NodeAddedEvent.Invoke(node);
                _nodes.Add(node);
            }
        }

        public void Remove(T node)
        {
            if (IsLocked)
            {
                _pending.Add(new KeyValuePair<T, PendingChange>(node, PendingChange.Remove));
            }
            else
            {
                NodeRemovedEvent.Invoke(node);
                _nodes.Remove(node);
            }
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
            ApplyPending();
            _pending.Clear();
        }

        private void ApplyPending()
        {
            foreach (var pair in _pending)
            {
                if (pair.Value == PendingChange.Add)
                    Add(pair.Key);
                else
                    Remove(pair.Key);
            }
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
