using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Helpers
{
    public class NodelessSystem : ISystem
    {
        protected Action<IEngine> _addedToEngineCallback;

        public virtual void AddedToEngine(IEngine engine)
        {
            if (_addedToEngineCallback != null)
                _addedToEngineCallback(engine);
        }

        public virtual void RemovedFromEngine(IEngine engine)
        {
        }

        public virtual void Update(float delta)
        {
        }
    }

    public class NodelessSystem<T1> : NodelessSystem
    {
        protected Action<float, T1> _updateCallback;
        protected Action<T1> _nodeAddedCallback;
        protected Action<T1> _nodeRemovedCallback;

        private INodeList<Node<T1>> _nodes;

        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1>>();
            _nodes.NodeAddedEvent.AddListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.AddListener(OnNodeRemoved);
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1);
        }

        private void OnNodeAdded(Node<T1> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
            _nodes.NodeAddedEvent.RemoveListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.RemoveListener(OnNodeRemoved);
            engine.ReleaseNodes(_nodes);
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1);
        }
    }

    public class NodelessSystem<T1,T2> : NodelessSystem
    {
        protected Action<float, T1, T2> _updateCallback;
        protected Action<T1, T2> _nodeAddedCallback;
        protected Action<T1, T2> _nodeRemovedCallback;

        private INodeList<Node<T1, T2>> _nodes;
 
        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2>>();
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1,T2> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1, node.Component2);
        }

        private void OnNodeAdded(Node<T1,T2> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1, node.Component2);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
            _nodes.NodeAddedEvent.RemoveListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.RemoveListener(OnNodeRemoved);
            engine.ReleaseNodes(_nodes);
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1, node.Component2);
        }
    }

    public class NodelessSystem<T1, T2, T3> : NodelessSystem
    {
        protected Action<float, T1, T2, T3> _updateCallback;
        protected Action<T1, T2, T3> _nodeAddedCallback;
        protected Action<T1, T2, T3> _nodeRemovedCallback;

        private INodeList<Node<T1, T2, T3>> _nodes;

        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3>>();
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1, T2, T3> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1, node.Component2, node.Component3);
        }

        private void OnNodeAdded(Node<T1, T2, T3> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1, node.Component2, node.Component3);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
            _nodes.NodeAddedEvent.RemoveListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.RemoveListener(OnNodeRemoved);
            engine.ReleaseNodes(_nodes);
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1, node.Component2, node.Component3);
        }
    }

    public class NodelessSystem<T1, T2, T3, T4> : NodelessSystem
    {
        protected Action<float, T1, T2, T3, T4> _updateCallback;
        protected Action<T1, T2, T3, T4> _nodeAddedCallback;
        protected Action<T1, T2, T3, T4> _nodeRemovedCallback;

        private INodeList<Node<T1, T2, T3, T4>> _nodes;

        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4>>();
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1, T2, T3, T4> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1, node.Component2, node.Component3, node.Component4);
        }

        private void OnNodeAdded(Node<T1, T2, T3, T4> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1, node.Component2, node.Component3, node.Component4);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1, node.Component2, node.Component3,
                        node.Component4);
        }
    }

    public class NodelessSystem<T1, T2, T3, T4, T5> : NodelessSystem
    {
        protected Action<float, T1, T2, T3, T4, T5> _updateCallback;
        protected Action<T1, T2, T3, T4, T5> _nodeAddedCallback;
        protected Action<T1, T2, T3, T4, T5> _nodeRemovedCallback;

        private INodeList<Node<T1, T2, T3, T4, T5>> _nodes;

        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4, T5>>();
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1, T2, T3, T4, T5> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1, node.Component2, node.Component3,
                    node.Component4, node.Component5);
        }

        private void OnNodeAdded(Node<T1, T2, T3, T4, T5> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1, node.Component2, node.Component3,
                    node.Component4, node.Component5);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
            _nodes.NodeAddedEvent.RemoveListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.RemoveListener(OnNodeRemoved);
            engine.ReleaseNodes(_nodes);
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1, node.Component2, node.Component3,
                        node.Component4, node.Component5);
        }
    }

    public class NodelessSystem<T1, T2, T3, T4, T5, T6> : NodelessSystem
    {
        protected Action<float, T1, T2, T3, T4, T5, T6> _updateCallback;
        protected Action<T1, T2, T3, T4, T5, T6> _nodeAddedCallback;
        protected Action<T1, T2, T3, T4, T5, T6> _nodeRemovedCallback;

        private INodeList<Node<T1, T2, T3, T4, T5, T6>> _nodes;

        override public void AddedToEngine(IEngine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4, T5, T6>>();
            base.AddedToEngine(engine);
        }

        private void OnNodeRemoved(Node<T1, T2, T3, T4, T5, T6> node)
        {
            if (_nodeRemovedCallback != null)
                _nodeRemovedCallback(node.Component1, node.Component2, node.Component3,
                    node.Component4, node.Component5, node.Component6);
        }

        private void OnNodeAdded(Node<T1, T2, T3, T4, T5, T6> node)
        {
            if (_nodeAddedCallback != null)
                _nodeAddedCallback(node.Component1, node.Component2, node.Component3,
                    node.Component4, node.Component5, node.Component6);
        }

        override public void RemovedFromEngine(IEngine engine)
        {
            _nodes.NodeAddedEvent.RemoveListener(OnNodeAdded);
            _nodes.NodeRemovedEvent.RemoveListener(OnNodeRemoved);
            engine.ReleaseNodes(_nodes);
        }

        override public void Update(float delta)
        {
            if (_updateCallback != null)
                foreach (var node in _nodes)
                    _updateCallback(delta, node.Component1, node.Component2, node.Component3,
                        node.Component4, node.Component5, node.Component6);
        }
    }

    public delegate void Action<T1, T2, T3, T4, T5>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    public delegate void Action<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
    public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7);
}
