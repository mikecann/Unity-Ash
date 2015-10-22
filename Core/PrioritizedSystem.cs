using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class PrioritizedSystem
    {
        public ISystem System { get; private set; }
        public int Priority { get; private set; }

        public PrioritizedSystem(ISystem system, int priority)
        {
            System = system;
            Priority = priority;
        }
    }
}
