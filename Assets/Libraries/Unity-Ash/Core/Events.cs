using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;
using UnityEngine.Events;

namespace Ash.Core
{
    public class ComponentAdded : UnityEvent<IEntity, Type>
    {
    }

    public class ComponentRemoved : UnityEvent<IEntity, Type>
    {
    }
}
