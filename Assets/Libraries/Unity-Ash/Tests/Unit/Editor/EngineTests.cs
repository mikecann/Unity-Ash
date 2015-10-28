using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;

namespace Ash.Core
{
    [TestFixture]
    public class EngineTests
    {
        private Engine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new Engine();
        }

        [Test]
        public void AddingASystem_InformsTheSystem()
        {
            var system = Substitute.For<ISystem>();
            _engine.AddSystem(system, 12);
            system.Received().AddedToEngine(_engine);
        }

        [Test]
        public void RemovingASystem_InformsTheSystem()
        {
            var system = Substitute.For<ISystem>();
            _engine.RemoveSystem(system);
            system.Received().RemovedFromEngine(_engine);
        }

        [Test]
        public void SystemsAreUpdatedInPriorityOrder()
        {
            var system1 = Substitute.For<ISystem>();
            var system2 = Substitute.For<ISystem>();
            var system3 = Substitute.For<ISystem>();

            _engine.AddSystem(system1, 100);
            _engine.AddSystem(system2, -20);
            _engine.AddSystem(system3, 30);
            _engine.Update(23.45f);

            Received.InOrder(() =>
            {
                system2.Update(23.45f);
                system3.Update(23.45f);
                system1.Update(23.45f);
            });
        }

        [Test]
        public void RemovedSystemsAreNoLongerUpdated()
        {
            var system1 = Substitute.For<ISystem>();
            var system2 = Substitute.For<ISystem>();
            var system3 = Substitute.For<ISystem>();

            _engine.AddSystem(system1, 100);
            _engine.AddSystem(system2, -20);
            _engine.AddSystem(system3, 30);
            _engine.RemoveSystem(system2);
            _engine.Update(23.45f);

            system1.Received().Update(23.45f);
            system2.DidNotReceive().Update(23.45f);
            system3.Received().Update(23.45f);
        }

        [Test]
        public void GetsNodesForAGivenType()
        {
            var nodes1 = _engine.GetNodes<Node>();
            Assert.IsNotNull(nodes1);
        }

        [Test]
        public void IfTwoNodeTypesAreReturned_DifferentFamiliesAreReturned()
        {
            var nodes1 = _engine.GetNodes<Node<MockComponentA>>();
            var nodes2 = _engine.GetNodes<Node<MockComponentB>>();
            Assert.IsTrue(nodes1 != nodes2);
        }

        [Test]
        public void IfSameNodeIsRequestedTwice_SameNodesReturned()
        {
            var nodes1 = _engine.GetNodes<Node<MockComponentA>>();

            var entity = new MockEntity<MockComponentA>();
            _engine.AddEntity(entity);

            var nodes2 = _engine.GetNodes<Node<MockComponentA>>();

            Assert.IsTrue(nodes1.Count() == nodes2.Count());
            Assert.IsTrue(nodes1.ToList()[0] == nodes2.ToList()[0]);
            Assert.IsTrue(nodes1.ToList()[0].Component1 == nodes2.ToList()[0].Component1);
        }

        [Test]
        public void AddingAnEntity_InformsEachFamily()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);

            var familyA = Substitute.For<IFamily<Node<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<Node<MockComponentB>>>();

            factory.Produce<Node<MockComponentA>>().Returns(familyA);
            factory.Produce<Node<MockComponentB>>().Returns(familyB);

            engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();

            var entityA = new MockEntity();

            engine.AddEntity(entityA);

            familyA.Received().EntityAdded(entityA);
            familyB.Received().EntityAdded(entityA);
        }

        [Test]
        public void RemovingAnEntity_InformsEachFamily()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);

            var familyA = Substitute.For<IFamily<Node<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<Node<MockComponentB>>>();

            factory.Produce<Node<MockComponentA>>().Returns(familyA);
            factory.Produce<Node<MockComponentB>>().Returns(familyB);

            engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();

            var entityA = new MockEntity();

            engine.AddEntity(entityA);
            engine.RemoveEntity(entityA);

            familyA.Received().EntityRemoved(entityA);
            familyB.Received().EntityRemoved(entityA);
        }

        [Test]
        public void IfNodesAreReleased_FamilyNoLongerInformedWhenEntityAddedOrRemoved()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);

            var familyA = Substitute.For<IFamily<Node<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<Node<MockComponentB>>>();

            factory.Produce<Node<MockComponentA>>().Returns(familyA);
            factory.Produce<Node<MockComponentB>>().Returns(familyB);

            var nodes1 = engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();

            engine.ReleaseNodes(nodes1);

            var entityA = new MockEntity();

            engine.AddEntity(entityA);
            engine.RemoveEntity(entityA);

            familyA.DidNotReceive().EntityAdded(entityA);
            familyB.Received().EntityAdded(entityA);

            familyA.DidNotReceive().EntityRemoved(entityA);
            familyB.Received().EntityRemoved(entityA);
        }

        [Test]
        public void IfNodesAreReleasedThenRetrievedAgain_EntitiesReadded()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<Node<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<Node<MockComponentA>>>();
            factory.Produce<Node<MockComponentA>>().Returns(family1, family2);

            var nodes1 = engine.GetNodes<Node<MockComponentA>>();
            engine.AddEntity(entity);
            engine.ReleaseNodes(nodes1);
            engine.GetNodes<Node<MockComponentA>>();

            family1.Received().EntityAdded(entity);
            family2.Received().EntityAdded(entity);
        }

        [Test]
        public void IfNodesAreReleasedThenRetrievedAgain_RemovedEntitiesNotReadded()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<Node<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<Node<MockComponentA>>>();
            factory.Produce<Node<MockComponentA>>().Returns(family1, family2);

            var nodes1 = engine.GetNodes<Node<MockComponentA>>();
            engine.AddEntity(entity);
            engine.ReleaseNodes(nodes1);
            engine.RemoveEntity(entity);
            engine.GetNodes<Node<MockComponentA>>();

            family1.Received().EntityAdded(entity);
            family2.DidNotReceive().EntityAdded(entity);
        }

        [Test]
        public void IfComponentAddedAfterEntityAddedToEngine_FamiliesInformed()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<Node<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<Node<MockComponentB>>>();
            factory.Produce<Node<MockComponentA>>().Returns(family1);
            factory.Produce<Node<MockComponentB>>().Returns(family2);

            engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();
            engine.AddEntity(entity);

            entity.ComponentAdded.Invoke(entity, typeof(MockComponentC));

            family1.Received().ComponentAdded(entity, typeof (MockComponentC));
            family2.Received().ComponentAdded(entity, typeof(MockComponentC));
        }

        [Test]
        public void IfComponentRemovedAfterEntityAddedToEngine_FamiliesInformed()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<Node<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<Node<MockComponentB>>>();
            factory.Produce<Node<MockComponentA>>().Returns(family1);
            factory.Produce<Node<MockComponentB>>().Returns(family2);

            engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();
            engine.AddEntity(entity);

            entity.ComponentRemoved.Invoke(entity, typeof(MockComponentC));

            family1.Received().ComponentRemoved(entity, typeof(MockComponentC));
            family2.Received().ComponentRemoved(entity, typeof(MockComponentC));
        }

        [Test]
        public void IfComponentAddedOrRemovedAfterEntityRemovedFromEngine_FamiliesNotInformed()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<Node<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<Node<MockComponentB>>>();
            factory.Produce<Node<MockComponentA>>().Returns(family1);
            factory.Produce<Node<MockComponentB>>().Returns(family2);

            engine.GetNodes<Node<MockComponentA>>();
            engine.GetNodes<Node<MockComponentB>>();
            engine.AddEntity(entity);
            engine.RemoveEntity(entity);

            entity.ComponentAdded.Invoke(entity, typeof(MockComponentC));
            entity.ComponentRemoved.Invoke(entity, typeof(MockComponentC));

            family1.DidNotReceive().ComponentAdded(entity, typeof(MockComponentC));
            family2.DidNotReceive().ComponentAdded(entity, typeof(MockComponentC));
            family1.DidNotReceive().ComponentRemoved(entity, typeof(MockComponentC));
            family2.DidNotReceive().ComponentRemoved(entity, typeof(MockComponentC));
        }
    }
}
