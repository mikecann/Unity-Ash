using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    [IntegrationTest.DynamicTest("Main")]
    public class TestNonMatchingEntityNotAdded : MonoBehaviour
    {
        void Start()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            entity.Add<SpriteRenderer>();

            var nodes = engine.GetNodes<Node<Rigidbody>>();
            
            if (nodes.Count() == 0)
                IntegrationTest.Pass();
            else
                IntegrationTest.Fail();
        }
    }
}
