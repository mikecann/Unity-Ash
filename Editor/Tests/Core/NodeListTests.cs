using System.Collections.Generic;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
	public class NodeListTests
	{
        private NodeList _nodes;
        private Node _tempNode;

        [SetUp]
        public void CreateEntity()
        {
            _nodes = new NodeList();
            _tempNode = new Node();
        }

        [TearDown]
        public void ClearEntity()
        {
            _nodes = null;
            _tempNode = null;
        }

        [Test]
        public void AddingNodeTriggersAddedSignal()
        {
            var eventFired = false;
            var mockNode = new MockNode();
            _nodes.NodeAdded += delegate { eventFired = true; };
            _nodes.Add(mockNode);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void RemovingNodeTriggersRemovedSignal()
        {
            var eventFired = false;
            var mockNode = new MockNode();
            _nodes.Add(mockNode);
            _nodes.NodeRemoved += delegate { eventFired = true; };
            _nodes.Remove(mockNode);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void AllNodesAreCoveredDuringIteration()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }

            for (var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
            }
            Assert.AreEqual(0, nodeArray.Count);
        }

        [Test]
        public void RemovingCurrentNodeDuringIterationIsValid()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }

            var count = 0;
            for (var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
                if (++count == 2)
                {
                    _nodes.Remove(node);
                }
            }
            Assert.AreEqual(0, nodeArray.Count);
        }

        [Test]
        public void RemovingNextNodeDuringIterationIsValid()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }

            var count = 0;
            for (var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
                if (++count == 2)
                {
                    _nodes.Remove(node.Next);
                }
            }
            Assert.AreEqual(1, nodeArray.Count);
        }

        [Test]
        public void ComponentAddedSignalContainsCorrectParameters()
        {
            Node signalNode = null;
            _nodes.NodeAdded += node => signalNode = node;
            _nodes.Add(_tempNode);
            Assert.AreSame(_tempNode, signalNode);
        }

        [Test]
        public void ComponentRemovedSignalContainsCorrectParameters()
        {
            Node signalNode = null;
            _nodes.Add(_tempNode);
            _nodes.NodeRemoved += node => signalNode = node;
            _nodes.Remove(_tempNode);
            Assert.AreSame(_tempNode, signalNode);
        }

        [Test]
        public void NodesInitiallySortedInOrderOfAddition()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            var expected = new List<Node> { node1, node2, node3 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SwappingOnlyTwoNodesChangesTheirOrder()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Swap(node1, node2);
            var expected = new List<Node> { node2, node1 };
            var actual = new List<Node> { _nodes.Head, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SwappingAdjacentNodesChangesTheirPositions()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            var node4 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            _nodes.Add(node4);
            _nodes.Swap(node2, node3);
            var expected = new List<Node> { node1, node3, node2, node4 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Tail.Previous, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SwappingNonAdjacentNodesChangesTheirPositions()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            var node4 = new MockNode();
            var node5 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            _nodes.Add(node4);
            _nodes.Add(node5);
            _nodes.Swap(node2, node4);
            var expected = new List<Node> { node1, node4, node3, node2, node5 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Head.Next.Next, _nodes.Tail.Previous, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SwappingEndNodesChangesTheirPositions()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            _nodes.Swap(node1, node3);
            var expected = new List<Node> { node3, node2, node1 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }

        class MockNode : Node {}
    }
}

