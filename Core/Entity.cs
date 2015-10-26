using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    public class Entity : MonoBehaviour, IEntity
    {
        private ComponentAdded _componentAdded = new ComponentAdded();
        private ComponentRemoved _componentRemoved = new ComponentRemoved();

        public bool HasComponent(Type type)
        {
            return gameObject.GetComponent(type) != null;
        }

        public object GetComponent(Type type)
        {
            return gameObject.GetComponent(type);
        }

        public T AddComponent<T>() where T : Component
        {
            var component = gameObject.AddComponent<T>();
            ComponentAdded.Invoke(this, typeof (T));
            return component;
        }

        public void RemoveComponent(Component component)
        {
            Destroy(component);
            ComponentRemoved.Invoke(this, component.GetType());
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
