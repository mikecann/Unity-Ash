using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Core
{
    public interface ISystem
    {
        void AddedToEngine(IEngine engine);
        void RemovedFromEngine(IEngine engine);
        void Update(float delta);
    }
}
