using System;

namespace Net.RichardLord.Ash.Core
{
	/**
	 * A collection of nodes.
	 * 
	 * <p>Systems within the game access the components of entities via NodeLists. A NodeList contains
	 * a node for each Entity in the game that has all the components required by the node. To iterate
	 * over a NodeList, start from the head and step to the next on each loop, until the returned value
	 * is null.</p>
	 * 
	 * <p>for( var node : Node = nodeList.head; node; node = node.next )
	 * {
	 *   // do stuff
	 * }</p>
	 * 
	 * <p>It is safe to remove items from a nodelist during the loop. When a Node is removed form the 
	 * NodeList it's previous and next properties still point to the nodes that were before and after
	 * it in the NodeList just before it was removed.</p>
	 */
	public class NodeList
	{
		/**
		 * The first item in the node list, or null if the list contains no nodes.
		 */
		public Node Head { get; set; }

        /**
		 * The last item in the node list, or null if the list contains no nodes.
		 */
		public Node Tail { get; set; }
		
		/**
		 * A signal that is dispatched whenever a node is added to the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was added.
		 */
	    public event Action<Node> NodeAdded;

		/**
		 * A signal that is dispatched whenever a node is removed from the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was removed.
		 */
	    public event Action<Node> NodeRemoved;

        public void Add(Node node)
		{
			if (Head == null)
			{
				Head = Tail = node;
				node.Next = node.Previous = null;
			}
			else
			{
				Tail.Next = node;
				node.Previous = Tail;
				node.Next = null;
				Tail = node;
			}

            if (NodeAdded != null)
            {
                NodeAdded(node);
            }
		}

        public void Remove(Node node)
		{
			if (Head == node)
			{
				Head = Head.Next;
			}
			if (Tail == node)
			{
				Tail = Tail.Previous;
			}
			
			if (node.Previous != null)
			{
				node.Previous.Next = node.Next;
			}
			
			if (node.Next != null)
			{
				node.Next.Previous = node.Previous;
			}

            if (NodeRemoved != null)
            {
                NodeRemoved(node);
            }
			// N.B. Don't set node.next and node.previous to null because that will break the list iteration if node is the current node in the iteration.
		}

        public void RemoveAll()
		{
			while(Head != null)
			{
				var node = Head;
				Head = node.Next;
				node.Previous = null;
				node.Next = null;

                if (NodeRemoved != null)
				{
				    NodeRemoved(node);
				}
			}
			Tail = null;
		}
		
		/**
		 * true if the list is empty, false otherwise.
		 */
		public bool Empty { get { return Head == null; } }
		
		/**
		 * Swaps the positions of two nodes in the list. Useful when sorting a list.
		 */
		public void Swap(Node node1, Node node2)
		{
			if (node1.Previous == node2)
			{
				node1.Previous = node2.Previous;
				node2.Previous = node1;
				node2.Next = node1.Next;
				node1.Next  = node2;
			}
			else if (node2.Previous == node1)
			{
				node2.Previous = node1.Previous;
				node1.Previous = node2;
				node1.Next = node2.Next;
				node2.Next  = node1;
			}
			else
			{
				var temp = node1.Previous;
				node1.Previous = node2.Previous;
				node2.Previous = temp;
				temp = node1.Next;
				node1.Next = node2.Next;
				node2.Next = temp;
			}

			if(Head == node1)
			{
				Head = node2;
			}
			else if(Head == node2)
			{
				Head = node1;
			}

			if (Tail == node1)
			{
				Tail = node2;
			}
			else if (Tail == node2)
			{
				Tail = node1;
			}

			if (node1.Previous != null)
			{							
				node1.Previous.Next = node1;
			}
			
            if(node2.Previous != null)
			{
				node2.Previous.Next = node2;
			}

			if (node1.Next != null)
			{
				node1.Next.Previous = node1;
			}

			if (node2.Next != null)
			{
				node2.Next.Previous = node2;
			}
		}
	}
}
