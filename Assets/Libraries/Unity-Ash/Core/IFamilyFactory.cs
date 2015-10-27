using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Libraries.Unity_Ash.Core;

namespace Ash.Core
{
    public interface IFamilyFactory
    {
        IFamily<T> Produce<T>() where T : INode;
    }
}
