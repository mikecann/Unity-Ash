using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestAddingAndRemovingEntitiesDispatchesEvents
    {
        [Test]
        public void Test()
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

            Assert.AreEqual(0, nodes.Count());
            Assert.IsTrue(added);
            Assert.IsTrue(removed);
        }
    }
}
