using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Assets.Libraries.Unity_Ash.Tests.Helpers
{
    public class MockSystem<T> : ISystem
    {
        public void AddedToEngine(Engine engine)
        {
            throw new NotImplementedException();
        }

        public void RemovedFromEngine(Engine engine)
        {
            throw new NotImplementedException();
        }

        public void Update(float delta)
        {
            throw new NotImplementedException();
        }
    }
}
