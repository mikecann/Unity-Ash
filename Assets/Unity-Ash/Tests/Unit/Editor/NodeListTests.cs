using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Ash.Core
{
    [TestFixture]
    public class NodeListTests
    {
        [Test]
        public void AddingNode()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            Node eventNode = null;
            list.NodeAddedEvent.AddListener(n => eventNode = n);

            list.Add(node);

            Assert.AreEqual(node, eventNode);
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void RemovingNode()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            Node eventNode = null;
            list.NodeRemovedEvent.AddListener(n => eventNode = n);

            list.Add(node);
            list.Remove(node);

            Assert.AreEqual(node, eventNode);
            Assert.AreEqual(0, list.Count());
        }

        [Test]
        public void WhenLocked_AdditionAndRemovalDoesntOccur()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            Node addEventNode = null;
            list.NodeAddedEvent.AddListener(n => addEventNode = n);

            Node removeEventNode = null;
            list.NodeRemovedEvent.AddListener(n => removeEventNode = n);

            list.Lock();
            list.Add(node);
            list.Remove(node);

            Assert.AreEqual(null, addEventNode);
            Assert.AreEqual(null, removeEventNode);
            Assert.AreEqual(0, list.Count());
        }

        [Test]
        public void WhenUnlocked_AdditionApplied()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            Node addEventNode = null;
            list.NodeAddedEvent.AddListener(n => addEventNode = n);

            list.Lock();
            list.Add(node);
            list.Unlock();

            Assert.AreEqual(node, addEventNode);
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void WhenUnlocked_RemovalApplied()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            Node eventNode = null;
            list.NodeRemovedEvent.AddListener(n => eventNode = n);

            list.Add(node);
            list.Lock();
            list.Remove(node);
            list.Unlock();

            Assert.AreEqual(node, eventNode);
            Assert.AreEqual(0, list.Count());
        }

        [Test]
        public void WhenNextUnlocked_ChangesNotApplied()
        {
            var list = new NodeList<Node>();
            var node = new Node();

            var count = 0;
            list.NodeAddedEvent.AddListener(n => count++);

            list.Lock();
            list.Add(node);
            list.Unlock();
            list.Unlock();

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, list.Count());
        }
    }
}
