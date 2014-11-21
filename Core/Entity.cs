using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Net.RichardLord.Ash.Core
{
    sealed public class Entity : EntityBase
    {       
        private IGame _engine;
        public IGame Engine 
        {
            get { return _engine; }
            set
            {
                if (_engine!=null) _engine.RemoveEntity(this);
                _engine = value;
                if (_engine != null) _engine.AddEntity(this);
            }
        }

        private IGame engine;

        void Awake()
        {
            AddNewComponents(GetComponents<Component>());
            Engine = FindEngine();
        }

        private IGame FindEngine()
        {
            var p = transform.parent;
            while (p != null)
            {
                var e = p.GetComponent<AshGame>();
                if (e != null) return e.Engine;
                p = p.parent;
            }
            return null;
        }

        void Update()
        {          
            var components = GetComponents<Component>();
            AddNewComponents(components);
            RemoveOldComponents(components);
        }

        private void AddNewComponents(Component[] components)
        {
            foreach (var component in components)
            {
                if (!Has(component.GetType()))
                    Add(component);
            }
        }

        private void RemoveOldComponents(Component[] components)
        {
            var toRemove = new List<Type>();
            foreach (var pair in Components)
            {
                if (!components.Contains(pair.Value))
                    toRemove.Add(pair.Key);
            }
            foreach (var type in toRemove)
                Remove(type);
        }

        void OnDestroy()
        {
            if (Engine == null) return;
            Engine.RemoveEntity(this);
        }

    }
}
