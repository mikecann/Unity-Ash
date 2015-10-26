using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Helpers
{
    public class NodelssSystem<T1,T2> : ISystem
    {
        protected Action<T1, T2> _updateCallback;
        private IEnumerable<Node<T1, T2>> _nodes;
 
        public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2>>();
        }

        public void RemovedFromEngine(Engine engine)
        {
        }

        public void Update(float delta)
        {
            if (_updateCallback != null)
            {
                foreach (var node in _nodes)
                {
                    _updateCallback(node.component1, node.component2);
                }
            }
        }
    }
}
