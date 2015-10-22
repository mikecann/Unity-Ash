using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class Engine : IEngine
    {
        private List<PrioritizedSystem> _systems;
        private List<IEntity> _entities;
        private Dictionary<Type, IFamily> _families;

        public Engine()
        {
            _systems = new List<PrioritizedSystem>();
            _entities = new List<IEntity>();
            _families = new Dictionary<Type, IFamily>();
        }

        public void AddEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddSystem(ISystem system, int priority)
        {
            _systems.Add(new PrioritizedSystem(system, priority));
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
                family = new ComponentMatchingFamily<T>();
                _families[type] = family;
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
    }
}
