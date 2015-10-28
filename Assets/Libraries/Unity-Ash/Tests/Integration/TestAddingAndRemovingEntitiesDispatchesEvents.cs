using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    [IntegrationTest.DynamicTest("Main")]
    public class TestAddingAndRemovingEntitiesDispatchesEvents : MonoBehaviour
    {
        void Start()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();

            var nodes = engine.GetNodes<Node<SpriteRenderer>>();
            var added = false;
            var removed = false;
            nodes.NodeAddedEvent.AddListener(node => added = true);
            nodes.NodeAddedEvent.AddListener(node => removed = true);

            var renderer = entity.Add<SpriteRenderer>();
            entity.Remove(renderer);

            if (nodes.Count() == 0 && added && removed)
                 IntegrationTest.Pass();
            else
                IntegrationTest.Fail();
        }
    }
}
