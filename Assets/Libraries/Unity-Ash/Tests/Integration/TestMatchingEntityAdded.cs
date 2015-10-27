using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    [IntegrationTest.DynamicTest("Main")]
    public class TestMatchingEntityAdded : MonoBehaviour
    {
        void Start()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            var renderer = entity.Add<SpriteRenderer>();

            var nodes = engine.GetNodes<Node<SpriteRenderer>>();

            if (nodes.Count() == 1)
            {
                var node = nodes.First();
                if (node.Entity == entity && node.Component1 == renderer)
                    IntegrationTest.Pass();
                else
                    IntegrationTest.Fail();
            }
            else
                IntegrationTest.Fail();
        }
    }
}
