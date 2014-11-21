using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Net.RichardLord.Ash.Core
{
/**
* A game entity is a collection object for components. Sometimes, the entities in a game
* will mirror the actual characters and objects in the game, but this is not necessary.
*
* <p>Components are simple value objects that contain data relevant to the entity. Entities
* with similar functionality will have instances of the same components. So we might have
* a position component</p>
*
* <p><code>public class PositionComponent
* {
* public var x : Number;
* public var y : Number;
* }</code></p>
*
* <p>All entities that have a position in the game world, will have an instance of the
* position component. Systems operate on entities based on the components they have.</p>
*/

    [HideInInspector]
    public class EntityBase : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _components;

        /// <summary>
        /// Optional, give the entity a name. This can help with debugging and with serialising the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This signal is dispatched when a component is added to the entity.
        /// </summary>
        public event Action<EntityBase, Type> ComponentAdded;

        /// <summary>
        /// This signal is dispatched when a component is removed from the entity.
        /// </summary>
        public event Action<EntityBase, Type> ComponentRemoved;

        public EntityBase Previous { get; set; }
        public EntityBase Next { get; set; }

        internal Dictionary<Type, object> Components
        {
            get { return _components; }
        }

        public EntityBase()
        {
            _components = new Dictionary<Type, object>();
        }

        /**
        * Add a component to the entity.
        *
        * @param component The component object to add.
        * @param componentClass The class of the component. This is only necessary if the component
        * extends another component class and you want the framework to treat the component as of
        * the base class type. If not set, the class type is determined directly from the component.
        *
        * @return A reference to the entity. This enables the chaining of calls to add, to make
        * creating and configuring entities cleaner. e.g.
        *
        * <code>var entity : Entity = new Entity()
        * .add( new Position( 100, 200 )
        * .add( new Display( new PlayerClip() );</code>
        */

        public EntityBase Add(object component)
        {
            AddComponentAndDispatchAddEvent(component, component.GetType());
            return this;
        }

        public EntityBase Add(object component, Type componentClass)
        {
            if (!componentClass.IsInstanceOfType(component))
            {
                throw new InvalidOperationException("Component is not an instance of " + componentClass +
                                                    " or its parent types.");
            }
            AddComponentAndDispatchAddEvent(component, componentClass);
            return this;
        }

        private EntityBase AddComponentAndDispatchAddEvent(object component, Type componentClass)
        {
            if (component == null)
            {
                throw new NullReferenceException("Component cannot be null.");
            }

            if (_components.ContainsKey(componentClass))
            {
                _components.Remove(componentClass);
            }

            _components[componentClass] = component;

            if (ComponentAdded != null) { ComponentAdded(this, componentClass); }
            return this;
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <param name="componentClass">The class of the component to be removed.</param>
        /// <returns>The component, or null if the component doesn't exist in the entity.</returns>        
        public object Remove<T>()
        {
            return Remove(typeof(T));
        }

        public object Remove(Type componentClass)
        {
            if (_components.ContainsKey(componentClass))
            {
                var component = _components[componentClass];
                _components.Remove(componentClass);
                if (ComponentRemoved != null) { ComponentRemoved(this, componentClass); }
                return component;
            }
            return null;
        }

        /**
        * Get a component from the entity.
        *
        * @param componentClass The class of the component requested.
        * @return The component, or null if none was found.
        */

        public object Get(Type componentClass)
        {
            return _components.ContainsKey(componentClass) ? _components[componentClass] : null;
        }

        /**
        * Get a component from the entity.
        *
        * @param componentClass The class of the component requested.
        * @return The component, or null if none was found.
        */

        public T Get<T>(Type componentClass)
        {
            return _components.ContainsKey(componentClass) ? (T)_components[componentClass] : default(T);
        }

		/**
		 * Get all components from the entity.
		 * 
		 * @return An array containing all the components that are on the entity.
		 */
		public List<object> GetAll()
		{
		    return _components.Values.ToList();
		}

        /**
        * Does the entity have a component of a particular type.
        *
        * @param componentClass The class of the component sought.
        * @return true if the entity has a component of the type, false if not.
        */

        public bool Has(Type componentClass)
        {
            return _components.ContainsKey(componentClass);
        }

        /**
        * Make a copy of the entity
        *
        * @return A new entity with new components that are copies of the components on the
        * original entity.
        */

        public EntityBase Clone()
        {
            var copy = new EntityBase();
            foreach (var component in _components)
            {
                var componentType = component.Key;
                var clonedComponent = Activator.CreateInstance(componentType);
                foreach (var property in componentType.GetProperties().Where(property => property.CanRead && property.CanWrite))
                {
                    property.SetValue(clonedComponent, property.GetValue(component.Value, null), null);
                }
                copy.Add(clonedComponent, component.Key);
            }

            return copy;
        }
    }
}