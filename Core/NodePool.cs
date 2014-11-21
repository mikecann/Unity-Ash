using System;

namespace Net.RichardLord.Ash.Core
{
	/**
	 * This internal class maintains a pool of deleted nodes for reuse by framework. This reduces the overhead
	 * from object creation and garbage collection.
	 */
	internal class NodePool
	{
	    private Node _tail;
		private Node _cacheTail;
	    private Type _nodeType;

        public NodePool(Type nodeType)
        {
            _nodeType = nodeType;
        }

		internal Node Get()
		{
			if (_tail != null)
			{
				var node = _tail;
				_tail = _tail.Previous;
				node.Previous = null;
				return node;
			}
			else
			{
				return (Node)Activator.CreateInstance(_nodeType);
			}
		}

		internal void Dispose(Node node)
		{
			node.Next = null;
			node.Previous = _tail;
			_tail = node;
		}
		
		internal void Cache(Node node)
		{
			node.Previous = _cacheTail;
			_cacheTail = node;
		}
		
		internal void ReleaseCache()
		{
			while (_cacheTail != null)
			{
				var node  = _cacheTail;
				_cacheTail = node.Previous;
				node.Next = null;
				node.Previous = _tail;
				_tail = node;
			}
		}
	}
}
