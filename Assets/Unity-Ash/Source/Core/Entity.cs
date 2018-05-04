using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ash.Core
{
    public class Entity : MonoBehaviour, IEntity
    {
        private readonly ComponentAdded _componentAdded = new ComponentAdded();
        private readonly ComponentRemoved _componentRemoved = new ComponentRemoved();
        private IEngine _engine;

        protected void Awake()
        {
            _engine = FindEngine();
            if (_engine == null)
                throw new EntityException("Canont find Engine!");

            _engine.AddEntity(this);
        }

        protected virtual IEngine FindEngine()
        {
            return Engine.Current;
        }

        public bool Has(Type type)
        {
            return GetComponent(type) != null;
        } 

        public object Get(Type type)
        {
            return gameObject.GetComponent(type);
        }

        public void Destroy()
        {
            if (Application.isPlaying)
                Destroy(gameObject);
        }

        public T Add<T>() where T : Component
        {
            var component = gameObject.AddComponent<T>();
            ComponentAdded.Invoke(this, typeof(T));
            return component;
        }

        public void Remove(Component component)
        {
            DestroyComponent(component);
            ComponentRemoved.Invoke(this, component.GetType());
        }

        protected virtual void DestroyComponent(Component component)
        {
            DestroyImmediate(component);
        }

        private void OnDestroy()
        {
            if (_engine != null)
                _engine.RemoveEntity(this);
        }

        public ComponentAdded ComponentAdded
        {
            get { return _componentAdded; }
        }

        public ComponentRemoved ComponentRemoved
        {
            get { return _componentRemoved; }
        }
    }
}