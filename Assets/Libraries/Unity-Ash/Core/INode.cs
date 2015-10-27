using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Assets.Libraries.Unity_Ash.Core
{
    public interface INode
    {
        IEntity Entity { get; set; }
    }
}
