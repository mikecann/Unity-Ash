using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IFamilyFactory
    {
        IFamily<T> Produce<T>();
    }
}
