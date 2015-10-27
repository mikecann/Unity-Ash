using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Core
{
    public interface ISystem
    {
        void AddedToEngine(Engine engine);
        void RemovedFromEngine(Engine engine);
        void Update(float delta);
    }
}
