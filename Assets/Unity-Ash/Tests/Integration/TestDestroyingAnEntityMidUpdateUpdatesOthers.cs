using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestDestroyingAnEntityMidUpdateUpdatesOthers
    {
        private Engine _engine;
        private Entity _entity1;
        private Entity _entity2;
        private Entity _entity3;
        private ISystem _system;
        private INodeList<Node<Transform>> _nodes;
        private int _calls;

        [SetUp]
        public void Setup()
        {
            _engine = new Engine();

            _entity1 = new GameObject().AddComponent<Entity>();
            _entity2 = new GameObject().AddComponent<Entity>();
            _entity3 = new GameObject().AddComponent<Entity>();

            _system = Substitute.For<ISystem>();
            _nodes = _engine.GetNodes<Node<Transform>>();

            _engine.AddSystem(_system, 0);

            _calls = 0;
        }

        [Test]
        public void TestAssumption()
        {
            _system.When(s => s.Update(0)).Do(c =>
            {
                foreach (var node in _nodes)
                    _calls++;
            });

            _engine.Update(0);

            Assert.AreEqual(3, _calls);
        }

        [Test]
        public void TestDestroyingThisOne()
        {
            _system.When(s => s.Update(0)).Do(c =>
            {
                foreach (var node in _nodes)
                {
                    _calls++;

                    if (_calls==2)
                        GameObject.DestroyImmediate(_entity2.gameObject);
                }
            });

            _engine.Update(0);

            Assert.AreEqual(3, _calls);
        }

        [Test]
        public void TestDestroyingNextOne()
        {
            _system.When(s => s.Update(0)).Do(c =>
            {
                foreach (var node in _nodes)
                {
                    _calls++;

                    if (_calls == 1)
                        GameObject.DestroyImmediate(_entity2.gameObject);

                    if (_calls == 2)
                        Assert.IsTrue(node.Component1==null);
                }
            });

            _engine.Update(0);

            Assert.AreEqual(3, _calls);
        }

        [Test]
        public void TestRemovingNextOne()
        {
            _system.When(s => s.Update(0)).Do(c =>
            {
                foreach (var node in _nodes)
                {
                    _calls++;

                    if (_calls == 1)
                        _engine.RemoveEntity(_entity2);
                }
            });

            _engine.Update(0);

            Assert.AreEqual(3, _calls);
        }

    }
}
