using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public class ComponentMatchingFamilyFactory : IFamilyFactory
    {
        public IFamily<T> Produce<T>()
        {
            return new ComponentMatchingFamily<T>();
        }
    }
}
