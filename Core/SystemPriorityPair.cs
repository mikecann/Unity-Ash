using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    internal class SystemPriorityPair
    {
        public ISystem System { get; private set; }
        public int Priority { get; private set; }

        public SystemPriorityPair(ISystem system, int priority)
        {
            System = system;
            Priority = priority;
        }
    }
}
