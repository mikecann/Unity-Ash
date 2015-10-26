using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }

        public bool HasComponent(Type type)
        {
            return _components.Any(c => c.GetType() == type);
        }

        public object GetComponent(Type type)
        {
            if (!HasComponent(type))
                throw new Exception("Invalid type");

            return _components.FirstOrDefault(c => c.GetType() == type);
        }

        public ComponentAdded ComponentAdded { get; private set; }
        public ComponentRemoved ComponentRemoved { get; private set; }
    }

    public class MockEntity<T1> : MockEntity
    {
        public MockEntity()
        {
            _components = new List<object>()
            {
                Activator.CreateInstance<T1>()
            };
        }
    }

    public class MockEntity<T1,T2> : MockEntity
    {
        public MockEntity()
        {
            _components = new List<object>()
            {
                Activator.CreateInstance<T1>(),
                Activator.CreateInstance<T2>()
            };
        }
    }

    public class MockEntity<T1, T2, T3> : MockEntity
    {
        public MockEntity()
        {
            _components = new List<object>()
            {
                Activator.CreateInstance<T1>(),
                Activator.CreateInstance<T2>(),
                Activator.CreateInstance<T3>()
            };
        }
    }
}
