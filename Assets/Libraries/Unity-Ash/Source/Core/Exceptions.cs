using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class ComponentMatchingFamilyException : Exception
    {
        public ComponentMatchingFamilyException(string message) : base(message)
        {
        }
    }

    public class EntityException : Exception
    {
        public EntityException(string message) : base(message)
        {
        }
    }
}
