using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ash.Core
{
    public class ComponentMatchingFamily<T> : IFamily<T>
    {
        private Dictionary<IEntity,T> _entities;
        private NodeList<T> _nodes;
        private Dictionary<Type, PropertyInfo> _components;
        private INodePool<T> _pool;

        public ComponentMatchingFamily(INodePool<T> pool = null)
        {
            _entities = new Dictionary<IEntity, T>();
            _nodes = new NodeList<T>();
            _pool = pool ?? new NodePool<T>();
            Init();
        }

        private void Init()
        {
            var type = typeof (T);
            _components = type.GetProperties()
                .ToDictionary(i => i.PropertyType, i => i);
        }

        public void ComponentAdded(IEntity entity, Type componentType)
        {
            if (_entities.ContainsKey(entity))
                return;

            AddIfMatch(entity);
        }

        public void ComponentRemoved(IEntity entity, Type componentType)
        {
            if (!_entities.ContainsKey(entity))
                return;

            if (!_components.ContainsKey(componentType))
                return;

            RemoveEntity(entity);
        }

        public void EntityAdded(IEntity entity)
        {
            if (_entities.ContainsKey(entity))
                throw new ComponentMatchingFamilyException("Entity already added to family.");

            AddIfMatch(entity);
        }

        public void EntityRemoved(IEntity entity)
        {
            if (!_entities.ContainsKey(entity))
                return;

            RemoveEntity(entity);
        }

        private void RemoveEntity(IEntity entity)
        {
            var node = _entities[entity];
            _pool.Pool(node);
            _entities.Remove(entity);
            _nodes.Remove(node);
        }

        private void AddIfMatch(IEntity entity)
        {
            foreach (var pair in _components)
                if (!entity.Has(pair.Key))
                    return;

            var node = _pool.UnPool();
            _entities[entity] = node;

            foreach (var pair in _components)
                pair.Value.SetValue(node, entity.Get(pair.Key), null);

            _nodes.Add(node);
        }

        public INodeList<T> Nodes
        {
            get { return _nodes; }
        }
    }
}
