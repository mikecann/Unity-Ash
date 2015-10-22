using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ash.Core
{
    public interface IFamily
    {
    }

    public interface IFamily<T> : IFamily
    {
        IEnumerable<T> Nodes { get; }
    }
}
