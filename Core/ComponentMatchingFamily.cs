using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Net.RichardLord.Ash.Core
{
	/**
	 * An default class for managing a NodeList. This class creates the NodeList and adds and removes
	 * nodes to/from the list as the entities and the components in the game change.
	 * 
	 * It uses the basic entity matching pattern of an entity system - entities are added to the list if
	 * they contain components matching all the public properties of the node class.
	 */
	public class ComponentMatchingFamily : IFamily
	{
		private NodeList _nodes;
	    private Type _nodeType;
	    private Dictionary<EntityBase, Node> _entities;
	    private Dictionary<Type, string> _components;
	    private NodePool _nodePool;
		private IGame _game;

		public void Setup(IGame game, Type nodeType)
		{
            _game = game;
		    _nodeType = nodeType;
       		Init();
		}

		private void Init()
		{
		    _nodePool = new NodePool(_nodeType);
			_nodes = new NodeList();
		    _entities = new Dictionary<EntityBase, Node>();

		    _components = new Dictionary<Type, string>();
            foreach (var property in _nodeType.GetProperties(BindingFlags.Instance|BindingFlags.Public))
            {
                if (!(property.Name == "Entity" || property.Name == "Previous" || property.Name == "Next"))
                {
                    _components.Add(property.PropertyType, property.Name);
                }
            }
		}
		
		public NodeList NodeList { get { return _nodes;  } }

		public void NewEntity(EntityBase entity)
		{
			AddIfMatch(entity);
		}
		
		public void ComponentAddedToEntity(EntityBase entity, Type componentClass)
		{
			AddIfMatch(entity);
		}
		
		public void ComponentRemovedFromEntity(EntityBase entity, Type componentClass)
		{
			if (_components.ContainsKey(componentClass))
			{
				RemoveIfMatch(entity);
			}
		}
		
		public void RemoveEntity(EntityBase entity)
		{
			RemoveIfMatch(entity);
		}
		
		private void AddIfMatch(EntityBase entity)
		{
			if (!_entities.ContainsKey(entity))
			{
				if (_components.Keys.Any(componentClass => !entity.Has(componentClass)))
				{
				    return;
				}

				var node = _nodePool.Get();
				node.Entity = entity;
				foreach (var componentClass in _components.Keys)
				{
				    node.SetProperty(_components[componentClass], entity.Get(componentClass));
				}
			    _entities.Add(entity, node);
				_nodes.Add(node);
			}
		}
		
		private void RemoveIfMatch(EntityBase entity)
		{
			if (_entities.ContainsKey(entity))
			{
				var node = _entities[entity];
				_entities.Remove(entity);
				_nodes.Remove(node);
				if (_game.Updating)
				{
					_nodePool.Cache(node);
					_game.UpdateComplete += ReleaseNodePoolCache;
				}
				else
				{
					_nodePool.Dispose(node);
				}
			}
		}
		
		private void ReleaseNodePoolCache()
		{
			_game.UpdateComplete -= ReleaseNodePoolCache;
			_nodePool.ReleaseCache();
		}
		
		public void CleanUp()
		{
			for (var node = _nodes.Head; node != null; node = node.Next)
			{
				_entities.Remove(node.Entity);
			}
			_nodes.RemoveAll();
		}
    }
}
