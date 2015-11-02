using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    public class MockEntity : IEntity
    {
        protected List<object> _components;
        public List<object> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public MockEntity()
        {
            ComponentAdded = new ComponentAdded();
            ComponentRemoved = new ComponentRemoved();
            _components = new List<object>();
        }

        public bool Has(Type type)
        {
            return _components.Any(c => type.IsAssignableFrom(c.GetType()));
        }

        public bool IsDestroyed { get; private set; }

        public object Get(Type type)
        {
            if (!Has(type))
                throw new Exception("Invalid type");

            return _components.FirstOrDefault(c => type.IsAssignableFrom(c.GetType()));
        }

        public T Add<T>() where T : Component
        {
            var inst = Activator.CreateInstance<T>();
            _components.Add(inst);
            return inst;
        }

        public void Remove(Component component)
        {
            _components.Remove(component);
        }

        public ComponentAdded ComponentAdded { get; private set; }
        public ComponentRemoved ComponentRemoved { get; private set; }
    }

    public class MockEntity<T1> : MockEntity
    {
        public MockEntity()
        {
            _components.Add(Activator.CreateInstance<T1>());
        }
    }

    public class MockEntity<T1,T2> : MockEntity
    {
        public MockEntity()
        {
            _components.Add(Activator.CreateInstance<T1>());
            _components.Add(Activator.CreateInstance<T2>());
        }
    }

    public class MockEntity<T1, T2, T3> : MockEntity
    {
        public MockEntity()
        {
            _components.Add(Activator.CreateInstance<T1>());
            _components.Add(Activator.CreateInstance<T2>());
            _components.Add(Activator.CreateInstance<T3>());
        }
    }
}
