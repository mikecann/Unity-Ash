using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;
using NSubstitute;
using NUnit.Framework;

namespace Assets.Libraries.Unity_Ash.Editor.Tests.Core
{
    [TestFixture]
    public class ComponentMatchingFamilyTests
    {

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void AddingAnEntityWithMissingComponents_IsNotAddedToNodeList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentB>();
            family.EntityAdded(entityA);

            Assert.IsEmpty(family.Nodes);
        }

        [Test]
        public void AddingAnEntityWithMatchingComponent_IsAddedToNodeList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();
            family.EntityAdded(entityA);

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.First().component1, entityA.Components[0]);
        }

        [Test]
        public void AddingAnEntityWithMultipleMatchingComponents_IsAddedToNodeList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA, MockComponentB>>();

            var entityA = new MockEntity<MockComponentA, MockComponentB>();
            family.EntityAdded(entityA);

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.First().component1, entityA.Components[0]);
        }

        [Test]
        public void AddingAnEntityWithMultipleComponentsButNotMatching_IsNotAddedToNodeList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA, MockComponentB>>();

            var entityA = new MockEntity<MockComponentA, MockComponentC>();
            family.EntityAdded(entityA);

            Assert.IsEmpty(family.Nodes);
        }

        [Test]
        public void AddingDifferentEntitiesWithMatchingComponent_AllAreAddedToNodeList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entity1 = new MockEntity<MockComponentA>();
            var entity2 = new MockEntity<MockComponentA>();

            family.EntityAdded(entity1);

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.ToList()[0].component1, entity1.Components[0]);

            family.EntityAdded(entity2);

            Assert.AreEqual(family.Nodes.Count(), 2);
            Assert.AreEqual(family.Nodes.ToList()[1].component1, entity2.Components[0]);
        }

        [Test]
        [ExpectedException(typeof(ComponentMatchingFamilyException))]
        public void IfAnEntityIsAddedTwice_ExceptionThrown()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();

            family.EntityAdded(entityA);
            family.EntityAdded(entityA);
        }

        [Test]
        public void IfFamilyDoesntContainEntityWhenOneIsRemoved_NothingHappens()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entity1 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity1);

            var entity2 = new MockEntity<MockComponentB>();
            family.EntityAdded(entity2);

            Assert.IsTrue(family.Nodes.Count() == 1);
            Assert.IsTrue(family.Nodes.First().component1 == entity1.Components[0]);
        }

        [Test]
        public void IfEntityMatches_RemovesNodeFromList()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entity1 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity1);
            family.EntityRemoved(entity1);

            Assert.IsEmpty(family.Nodes);
        }
        
        [Test]
        public void IfEntityOfSameTypeButDifferentInstanceIsRemoved_CorrectInstanceIsRemoved()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entity1 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity1);

            var entity2 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity2);

            Assert.IsTrue(family.Nodes.Count() == 2);

            family.EntityRemoved(entity1);

            Assert.IsTrue(family.Nodes.Count() == 1);
            Assert.IsTrue(family.Nodes.First().component1 == entity2.Components[0]);
        }

        [Test]
        public void WhenEntityAdded_NodePoolUsed()
        {
            var pool = Substitute.For<INodePool<MockNode<MockComponentA>>>();
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>(pool);

            var entity1 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity1);

            pool.Received().UnPool();
        }

        [Test]
        public void WhenEntityRemoved_NodeReturnedToPool()
        {
            var pool = Substitute.For<INodePool<MockNode<MockComponentA>>>();
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>(pool);

            var node = new MockNode<MockComponentA>();
            pool.UnPool().Returns(node);

            var entity1 = new MockEntity<MockComponentA>();
            family.EntityAdded(entity1);
            family.EntityRemoved(entity1);
            
            pool.Received().UnPool();
            pool.Received().Pool(node);
        }

        [Test]
        public void IfComponentAddedThatMakesThisEntityMatch_AddedToNodes()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();
            family.ComponentAdded(entityA, typeof(MockComponentA));

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.First().component1, entityA.Components[0]);
        }

        [Test]
        public void IfComponentAddedToEntityThatAlreadyInList_NothingHappens()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();
            family.EntityAdded(entityA);
            family.ComponentAdded(entityA, typeof(MockComponentB));

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.First().component1, entityA.Components[0]);
        }

        [Test]
        public void IfComponentRemovedThatMakesThisEntityNoLongerMatch_RemovedFromNodes()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();
            family.EntityAdded(entityA);
            family.ComponentRemoved(entityA, typeof (MockComponentA));

            Assert.IsEmpty(family.Nodes);
        }

        [Test]
        public void IfComponentRemovedFromAnEntityNotInTheList_NothingHappens()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entity1 = new MockEntity<MockComponentA>();
            var entity2 = new MockEntity<MockComponentB>();

            family.EntityAdded(entity1);
            family.EntityAdded(entity2);

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.ToList()[0].component1, entity1.Components[0]);

            family.ComponentRemoved(entity2, typeof(MockComponentA));

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.ToList()[0].component1, entity1.Components[0]);
        }

        [Test]
        public void IfComponentRemovedButTheEntityStillMatches_NothingHappens()
        {
            var family = new ComponentMatchingFamily<MockNode<MockComponentA>>();

            var entityA = new MockEntity<MockComponentA>();
            family.EntityAdded(entityA);
            family.ComponentRemoved(entityA, typeof(MockComponentB));

            Assert.AreEqual(family.Nodes.Count(), 1);
            Assert.AreEqual(family.Nodes.ToList()[0].component1, entityA.Components[0]);
        }

        //[Test]
        //public void NodeContainingPropertiesIsStillValid()
        //{
        //    var family = new ComponentMatchingFamily<MockNodeWithProperties<MockComponentA>>();

        //    var entityA = new MockEntity<MockComponentA>();
        //    family.EntityAdded(entityA);

        //    Assert.AreEqual(family.Nodes.Count(), 1);
        //    Assert.AreEqual(family.Nodes.First().Component1, entityA.Components[0]);
        //}
    }
}
