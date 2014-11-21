using System.Collections.Generic;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;
using UnityEngine;

namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
	public class ComponentMatchingFamilyTests
	{
		private IGame _game;
		private IFamily _family;
		
		[SetUp]
		public void CreateFamily()    
		{
			_game = new Game<ComponentMatchingFamily>();
		    _family = new ComponentMatchingFamily();
            _family.Setup(_game, typeof(MockNode));
		}

    	[TearDown]
		public void ClearFamily()    
		{
			_family = null;
			_game = null;
		}
		
		[Test]
		public void TestNodeListIsInitiallyEmpty()    
		{
			var nodes = _family.NodeList;
			Assert.IsNull(nodes.Head);
		}

        [Test]
        public void TestMatchingEntityIsAddedWhenAccessNodeListFirst()
        {
            var nodes = _family.NodeList;
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestMatchingEntityIsAddedWhenAccessNodeListSecond()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestNodeContainsEntityProperties()
        {
            var entity = new EntityBase();
            var point = new Vector2(1, 2);
            entity.Add(point);
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.AreEqual(point, ((MockNode)nodes.Head).Point);
        }

        [Test]
        public void TestMatchingEntityIsAddedWhenComponentAdded()
        {
            var nodes = _family.NodeList;
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.ComponentAddedToEntity(entity, typeof(Vector2));
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestNonMatchingEntityIsNotAdded()
        {
            var entity = new EntityBase();
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestNonMatchingEntityIsNotAddedWhenComponentAdded()
        {
            var entity = new EntityBase();
            entity.Add(new Matrix4x4());
            _family.ComponentAddedToEntity(entity, typeof(Matrix4x4));
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenAccessNodeListFirst()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            _family.RemoveEntity(entity);
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenAccessNodeListSecond()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            _family.RemoveEntity(entity);
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenComponentRemoved()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            entity.Remove(typeof(Vector2));
            _family.ComponentRemovedFromEntity(entity, typeof(Vector2));
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void NodeListContainsOnlyMatchingEntities()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entities.Add(entity);
                _family.NewEntity(entity);
                _family.NewEntity(new EntityBase());
            }

            var nodes = _family.NodeList;
            var results = new List<bool>();
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                results.Add(entities.Contains(node.Entity));
            }

            Assert.AreEqual(new List<bool> { true, true, true, true, true }, results);
        }

        [Test]
        public void NodeListContainsAllMatchingEntities()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entities.Add(entity);
                _family.NewEntity(entity);
                _family.NewEntity(new EntityBase());
            }

            var nodes = _family.NodeList;
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                entities.RemoveAt(entities.IndexOf(node.Entity));
            }
            Assert.AreEqual(0, entities.Count);
        }

        [Test]
        public void CleanUpEmptiesNodeList()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            _family.CleanUp();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void CleanUpSetsNextNodeToNull()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entities.Add(entity);
                _family.NewEntity(entity);
            }

            var nodes = _family.NodeList;
            var node = nodes.Head.Next;
            _family.CleanUp();
            Assert.IsNull(node.Next);
        }
    
        class MockNode : Node
        {
            public Vector2 Point { get; set; }
        }
    }
}

