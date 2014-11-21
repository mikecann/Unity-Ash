using System;

namespace Net.RichardLord.Ash.Core
{
    /**
     * Used internally, this is an ordered list of Systems for use by the game loop.
     */

    internal class SystemList
    {
        internal SystemBase Head { get; set; }
        internal SystemBase Tail { get; set; }

        internal void Add(SystemBase system)
        {
            if (Head == null)
            {
                Head = Tail = system;
                system.Next = system.Previous = null;
            }
            else
            {
                var node = Tail;
                for (; node != null; node = node.Previous)
                {
                    if (node.Priority <= system.Priority)
                    {
                        break;
                    }
                }

                if (node == Tail)
                {
                    Tail.Next = system;
                    system.Previous = Tail;
                    system.Next = null;
                    Tail = system;
                }
                else if (node == null)
                {
                    system.Next = Head;
                    system.Previous = null;
                    Head.Previous = system;
                    Head = system;
                }
                else
                {
                    system.Next = node.Next;
                    system.Previous = node;
                    node.Next.Previous = system;
                }
            }
        }

        internal void Remove(SystemBase system)
        {
            if (Head == system)
            {
                Head = Head.Next;
            }
            if (Tail == system)
            {
                Tail = Tail.Previous;
            }

            if (system.Previous != null)
            {
                system.Previous.Next = system.Next;
            }

            if (system.Next != null)
            {
                system.Next.Previous = system.Previous;
            }
            // N.B. Don't set system.next and system.previous to null because that will break the list iteration if node is the current node in the iteration.
        }

        internal void RemoveAll()
        {
            while (Head != null)
            {
                var system = Head;
                Head = Head.Next;
                system.Previous = null;
                system.Next = null;
            }
            Tail = null;
        }

        internal SystemBase Get(Type type)
        {
            for (var system = Head; system != null; system = system.Next)
            {
                if (type.IsAssignableFrom(system.GetType()))
                {
                    return system;
                }
            }
            return null;
        }
    }
}
