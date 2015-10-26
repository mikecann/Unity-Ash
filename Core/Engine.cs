using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    public class Engine : IEngine
    {
        private List<SystemPriorityPair> _systems;
        private Dictionary<Type, IFamily> _families;
        private IFamilyFactory _familyFactory;
        private List<IEntity> _entities;

        public Engine(IFamilyFactory familyFactory = null)
        {
            Current = this;
            _familyFactory = familyFactory ?? new ComponentMatchingFamilyFactory();
            _systems = new List<SystemPriorityPair>();
            _families = new Dictionary<Type, IFamily>();
            _entities = new List<IEntity>();
        }

        public void AddEntity(IEntity entity)
        {
            foreach (var pair in _families)
                pair.Value.EntityAdded(entity);

            entity.ComponentAdded.AddListener(OnComponentAdded);
            entity.ComponentRemoved.AddListener(OnComponentRemoved);

            _entities.Add(entity);
        }

        private void OnComponentRemoved(IEntity entity, Type component)
        {
            foreach (var pair in _families)
                pair.Value.ComponentRemoved(entity, component);
        }

        private void OnComponentAdded(IEntity entity, Type component)
        {
            foreach (var pair in _families)
                pair.Value.ComponentAdded(entity, component);
        }

        public void RemoveEntity(IEntity entity)
        {
            foreach (var pair in _families)
                pair.Value.EntityRemoved(entity);

            entity.ComponentAdded.RemoveListener(OnComponentAdded);
            entity.ComponentRemoved.RemoveListener(OnComponentRemoved);

            _entities.Remove(entity);
        }

        public void AddSystem(ISystem system, int priority)
        {
            _systems.Add(new SystemPriorityPair(system, priority));
            _systems = _systems.OrderBy(s => s.Priority).ToList();
            system.AddedToEngine(this);
        }

        public void RemoveSystem(ISystem system)
        {
            _systems.RemoveAll(s => s.System == system);
            system.RemovedFromEngine(this);
        }

        public IEnumerable<T> GetNodes<T>()
        {
            var type = typeof (T);
            IFamily<T> family;

            if (_families.ContainsKey(type))
                family = _families[type] as IFamily<T>;
            else
            {
                family = _familyFactory.Produce<T>();
                _families[type] = family;

                foreach (var entity in _entities)
                    family.EntityAdded(entity);
            }

            return family.Nodes;
        }

        public void ReleaseNodes<T>()
        {
            var type = typeof(T);
            if (!_families.ContainsKey(type))
                return;

            _families.Remove(type);
        }

        public void Update(float delta)
        {
            foreach (var prioritizedSystem in _systems)
                prioritizedSystem.System.Update(delta);
        }

        public static IEngine Current { get; set; }
    }
}
