using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestCanRemoveComponentDuringNodeAdded
    {
        [Test]
        public void Test()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            var nodes = engine.GetNodes<Node<SpriteRenderer>>();
            var nodeAddedCalled = false;

            // We should be able to remove a component during a node added event
            nodes.NodeAddedEvent.AddListener(node =>
            {
                nodeAddedCalled = true;
                Assert.AreEqual(1, nodes.Count());
                node.Component1.Remove<SpriteRenderer>();
                Assert.AreEqual(0, nodes.Count());
            });

            // This should trigger the above node added event
            entity.Add<SpriteRenderer>();

            Assert.AreEqual(0, nodes.Count());
            Assert.IsTrue(nodeAddedCalled);
        }
    }
}