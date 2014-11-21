using System.Collections.Generic;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;
using UnityEngine;

namespace Net.RichardLord.AshTests.Core
{
	/**
	 * Tests the family class through the game class. Left over from a previous 
	 * architecture but retained because all tests shoudl still pass.
	 */
    [TestFixture]
	public class GameAndFamilyIntegrationTests
    {
        private Game<ComponentMatchingFamily> _game;

        [SetUp]
        public void CreateEntity()
        {
            _game = new Game<ComponentMatchingFamily>();
        }

        [TearDown]
        public void ClearEntity()
        {
            _game = null;
        }

        [Test]
        public void TestFamilyIsInitiallyEmpty()
        {
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestNodeContainsEntityProperties()
        {
            var entity = new EntityBase();
            var Vector2 = new Vector2();
            var matrix = new Matrix4x4();
            entity.Add(Vector2);
            entity.Add(matrix);

            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            var head = (MockNode)nodes.Head;
            Assert.AreEqual(new List<object> { Vector2, matrix }, new List<object> { head.Vector2, head.Matrix });
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenAccessFamilyFirst()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenAccessFamilySecond()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenComponentsAdded()
        {
            var entity = new EntityBase();
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestIncorrectEntityNotAddedToFamilyWhenAccessFamilyFirst()
        {
            var entity = new EntityBase();
            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestIncorrectEntityNotAddedToFamilyWhenAccessFamilySecond()
        {
            var entity = new EntityBase();
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenComponentRemovedAndFamilyAlreadyAccessed()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            entity.Remove<Vector2>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenComponentRemovedAndFamilyNotAlreadyAccessed()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            entity.Remove<Vector2>();
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenRemovedFromGameAndFamilyAlreadyAccessed()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.RemoveEntity(entity);
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenRemovedFromGameAndFamilyNotAlreadyAccessed()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            _game.RemoveEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void FamilyContainsOnlyMatchingEntities()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entity.Add(new Matrix4x4());
                entities.Add(entity);
                _game.AddEntity(entity);
            }

            var nodes = _game.GetNodeList<MockNode>();
            var results = new List<bool>();
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                results.Add(entities.Contains(node.Entity));
            }

            Assert.AreEqual(new List<bool> { true, true, true, true, true }, results);
        }

        [Test]
        public void FamilyContainsAllMatchingEntities()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entity.Add(new Matrix4x4());
                entities.Add(entity);
                _game.AddEntity(entity);
            }

            var nodes = _game.GetNodeList<MockNode>();
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                var index = entities.IndexOf(node.Entity);
                entities.RemoveAt(index);
            }
            Assert.AreEqual(0, entities.Count);
        }

        [Test]
        public void ReleaseFamilyEmptiesNodeList()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.ReleaseNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void ReleaseFamilySetsNextNodeToNull()
        {
            var entities = new List<EntityBase>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new EntityBase();
                entity.Add(new Vector2());
                entity.Add(new Matrix4x4());
                entities.Add(entity);
                _game.AddEntity(entity);
            }

            var nodes = _game.GetNodeList<MockNode>();
            var node = nodes.Head.Next;
            _game.ReleaseNodeList<MockNode>();
            Assert.IsNull(node.Next);
        }

        [Test]
        public void RemoveAllEntitiesDoesWhatItSays()
        {
            var entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            entity = new EntityBase();
            entity.Add(new Vector2());
            entity.Add(new Matrix4x4());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.RemoveAllEntities();
            Assert.IsNull(nodes.Head);
        }

        private class MockNode : Node
        {
            public Vector2 Vector2 { get; set; }
            public Matrix4x4 Matrix { get; set; }
        }
    }
}

