using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Helpers
{
    public class NodelessSystem : ISystem
    {
        protected Action<Engine> _addedToEngineCallback;

        public virtual void AddedToEngine(Engine engine)
        {
            if (_addedToEngineCallback != null)
                _addedToEngineCallback(engine);
        }

        public virtual void RemovedFromEngine(Engine engine)
        {
        }

        public virtual void Update(float delta)
        {
        }
    }

    public class NodelessSystem<T1> : NodelessSystem
    {
        protected Action<float, T1> _updateCallback;

        private IEnumerable<Node<T1>> _nodes;

        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
        {
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

        private IEnumerable<Node<T1, T2>> _nodes;
 
        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
        {
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

        private IEnumerable<Node<T1, T2, T3>> _nodes;

        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
        {
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

        private IEnumerable<Node<T1, T2, T3, T4>> _nodes;

        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
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

        private IEnumerable<Node<T1, T2, T3, T4, T5>> _nodes;

        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4, T5>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
        {
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

        private IEnumerable<Node<T1, T2, T3, T4, T5, T6>> _nodes;

        override public void AddedToEngine(Engine engine)
        {
            _nodes = engine.GetNodes<Node<T1, T2, T3, T4, T5, T6>>();
            base.AddedToEngine(engine);
        }

        override public void RemovedFromEngine(Engine engine)
        {
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
