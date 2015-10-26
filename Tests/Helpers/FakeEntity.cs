using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    public class FakeEntity : Entity
    {
        public IEngine engine;

        override protected IEngine FindEngine()
        {
            return engine;
        }

        override protected void DestroyComponent(Component component)
        {
            DestroyImmediate(component);
        }

        public void Awaken(IEngine engine)
        {
            this.engine = engine;
            Awake();
        }
    }
}
