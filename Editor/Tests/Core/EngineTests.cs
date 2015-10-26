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
            var nodes1 = _engine.GetNodes<MockNode>();
            Assert.IsNotNull(nodes1);
        }

        [Test]
        public void IfTwoNodeTypesAreReturned_DifferentFamiliesAreReturned()
        {
            var nodes1 = _engine.GetNodes<MockNode>();
            var nodes2 = _engine.GetNodes<MockNode>();
            Assert.IsTrue(nodes1 != nodes2);
        }

        [Test]
        public void IfSameNodeIsRequestedTwice_SameNodesReturned()
        {
            var nodes1 = _engine.GetNodes<MockNode<MockComponentA>>();

            var entity = new MockEntity<MockComponentA>();
            _engine.AddEntity(entity);

            var nodes2 = _engine.GetNodes<MockNode<MockComponentA>>();

            Assert.IsTrue(nodes1.Count() == nodes2.Count());
            Assert.IsTrue(nodes1.ToList()[0] == nodes2.ToList()[0]);
            Assert.IsTrue(nodes1.ToList()[0].component1 == nodes2.ToList()[0].component1);
        }

        [Test]
        public void AddingAnEntity_InformsEachFamily()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);

            var familyA = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<MockNode<MockComponentB>>>();

            factory.Produce<MockNode<MockComponentA>>().Returns(familyA);
            factory.Produce<MockNode<MockComponentB>>().Returns(familyB);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();

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

            var familyA = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<MockNode<MockComponentB>>>();

            factory.Produce<MockNode<MockComponentA>>().Returns(familyA);
            factory.Produce<MockNode<MockComponentB>>().Returns(familyB);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();

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

            var familyA = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var familyB = Substitute.For<IFamily<MockNode<MockComponentB>>>();

            factory.Produce<MockNode<MockComponentA>>().Returns(familyA);
            factory.Produce<MockNode<MockComponentB>>().Returns(familyB);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();

            engine.ReleaseNodes<MockNode<MockComponentA>>();

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

            var family1 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            factory.Produce<MockNode<MockComponentA>>().Returns(family1, family2);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.AddEntity(entity);
            engine.ReleaseNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentA>>();

            family1.Received().EntityAdded(entity);
            family2.Received().EntityAdded(entity);
        }

        [Test]
        public void IfNodesAreReleasedThenRetrievedAgain_RemovedEntitiesNotReadded()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            factory.Produce<MockNode<MockComponentA>>().Returns(family1, family2);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.AddEntity(entity);
            engine.ReleaseNodes<MockNode<MockComponentA>>();
            engine.RemoveEntity(entity);
            engine.GetNodes<MockNode<MockComponentA>>();

            family1.Received().EntityAdded(entity);
            family2.DidNotReceive().EntityAdded(entity);
        }

        [Test]
        public void IfComponentAddedAfterEntityAddedToEngine_FamiliesInformed()
        {
            var factory = Substitute.For<IFamilyFactory>();
            var engine = new Engine(factory);
            var entity = new MockEntity<MockComponentA>();

            var family1 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<MockNode<MockComponentB>>>();
            factory.Produce<MockNode<MockComponentA>>().Returns(family1);
            factory.Produce<MockNode<MockComponentB>>().Returns(family2);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();
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

            var family1 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<MockNode<MockComponentB>>>();
            factory.Produce<MockNode<MockComponentA>>().Returns(family1);
            factory.Produce<MockNode<MockComponentB>>().Returns(family2);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();
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

            var family1 = Substitute.For<IFamily<MockNode<MockComponentA>>>();
            var family2 = Substitute.For<IFamily<MockNode<MockComponentB>>>();
            factory.Produce<MockNode<MockComponentA>>().Returns(family1);
            factory.Produce<MockNode<MockComponentB>>().Returns(family2);

            engine.GetNodes<MockNode<MockComponentA>>();
            engine.GetNodes<MockNode<MockComponentB>>();
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
