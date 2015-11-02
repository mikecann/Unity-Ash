using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;

namespace Ash.Core
{
    public class MockSystem<T> : ISystem
    {
        public Action<IEngine> AddedToEngineCallback { get; set; }
        public Action<IEngine> RemovedFromEngineCallback { get; set; }
        public Action<float> UpdateCallback { get; set; }

        public void AddedToEngine(IEngine engine)
        {
            if (AddedToEngineCallback != null)
                AddedToEngineCallback(engine);
        }

        public void RemovedFromEngine(IEngine engine)
        {
            if (RemovedFromEngineCallback != null)
                RemovedFromEngineCallback(engine);
        }

        public void Update(float delta)
        {
            if (UpdateCallback != null)
                UpdateCallback(delta);
        }
    }
}
