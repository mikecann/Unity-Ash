using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Assets.Libraries.Unity_Ash.Core;

namespace Ash.Core
{
    public class ComponentMatchingFamily<T> : IFamily<T> where T : INode
    {
        private Dictionary<IEntity, T> _nodes;
        private Dictionary<Type, PropertyInfo> _components;
        private INodePool<T> _pool;

        public ComponentMatchingFamily(INodePool<T> pool = null)
        {
            _nodes = new Dictionary<IEntity, T>();
            _pool = pool ?? new NodePool<T>();
            Init();
        }

        private void Init()
        {
            var type = typeof (T);
            _components = type.GetProperties()
                .Where(p => p.Name != "Entity")
                .ToDictionary(i => i.PropertyType, i => i);
        }

        public void ComponentAdded(IEntity entity, Type componentType)
        {
            if (_nodes.ContainsKey(entity))
                return;

            AddIfMatch(entity);
        }

        public void ComponentRemoved(IEntity entity, Type componentType)
        {
            if (!_nodes.ContainsKey(entity))
                return;

            if (!_components.ContainsKey(componentType))
                return;

            RemoveEntity(entity);
        }

        public void EntityAdded(IEntity entity)
        {
            if (_nodes.ContainsKey(entity))
                throw new ComponentMatchingFamilyException("Entity already added to family.");

            AddIfMatch(entity);
        }

        public void EntityRemoved(IEntity entity)
        {
            if (!_nodes.ContainsKey(entity))
                return;

            RemoveEntity(entity);
        }

        private void RemoveEntity(IEntity entity)
        {
            var node = _nodes[entity];
            _pool.Pool(node);
            _nodes.Remove(entity);
        }

        private void AddIfMatch(IEntity entity)
        {
            foreach (var pair in _components)
                if (!entity.Has(pair.Key))
                    return;

            var node = _pool.UnPool();
            _nodes[entity] = node;
            node.Entity = entity;

            foreach (var pair in _components)
                pair.Value.SetValue(node, entity.Get(pair.Key), null);
        }

        public IEnumerable<T> Nodes
        {
            get { return _nodes.Values; }
        }
    }
}
