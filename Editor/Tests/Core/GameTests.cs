using System.Collections.Generic;
using Net.RichardLord.Ash.Core;
using NUnit.Framework;
using UnityEngine;
namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
    class GameTests
    {
        private IGame _game;

        [SetUp]
        public void CreateGame()
        {
            _game = new Game<MockFamily>();
            MockFamily.Reset();
        }

        [TearDown]
        public void ClearGame()
        {
            _game = null;
        }

        [Test]
        public void TestAddEntityChecksWithAllFamilies()
        {
            _game.GetNodeList<MockNode>();
            _game.GetNodeList<MockNode2>();
            var entity = new EntityBase();
            _game.AddEntity(entity);

            var results = new List<int>
            {
                MockFamily.Instances[0].NewEntityCalls,
                MockFamily.Instances[1].NewEntityCalls
            };
            Assert.AreEqual(new List<int> { 1, 1 }, results);
        }

        [Test]
        public void TestRemoveEntityChecksWithAllFamilies()
        {
            _game.GetNodeList<MockNode>();
            _game.GetNodeList<MockNode2>();
            var entity = new EntityBase();
            _game.AddEntity(entity);
            _game.RemoveEntity(entity);
            var results = new List<int>
            {
                MockFamily.Instances[0].RemoveEntityCalls,
                MockFamily.Instances[1].RemoveEntityCalls
            };
            Assert.AreEqual(new List<int> { 1, 1 }, results);
        }

        [Test]
        public void TestRemoveAllEntitiesChecksWithAllFamilies()
        {
            _game.GetNodeList<MockNode>();
            _game.GetNodeList<MockNode2>();
            var entity = new EntityBase();
            var entity2 = new EntityBase();
            _game.AddEntity(entity);
            _game.AddEntity(entity2);
            _game.RemoveAllEntities();
            var results = new List<int>
            {
                MockFamily.Instances[0].RemoveEntityCalls,
                MockFamily.Instances[1].RemoveEntityCalls
            };
            Assert.AreEqual(new List<int> { 2, 2 }, results);
        }

        [Test]
        public void TestComponentAddedChecksWithAllFamilies()
        {
            _game.GetNodeList<MockNode>();
            _game.GetNodeList<MockNode2>();
            var entity = new EntityBase();
            _game.AddEntity(entity);
            entity.Add(new Vector2());
            var results = new List<int>
            {
                MockFamily.Instances[0].ComponentAddedCalls,
                MockFamily.Instances[1].ComponentAddedCalls
            };
            Assert.AreEqual(new List<int> { 1, 1 }, results);
        }

        [Test]
        public void TestComponentRemovedChecksWithAllFamilies()
        {
            _game.GetNodeList<MockNode>();
            _game.GetNodeList<MockNode2>();
            var entity = new EntityBase();
            _game.AddEntity(entity);
            entity.Add(new Vector2());
            entity.Remove<Vector2>();
            var results = new List<int>
            {
                MockFamily.Instances[0].ComponentRemovedCalls,
                MockFamily.Instances[1].ComponentRemovedCalls
            };
            Assert.AreEqual(new List<int> { 1, 1 }, results);
        }

        [Test]
        public void TestGetNodeListCreatesFamily()
        {
            _game.GetNodeList<MockNode>();
            Assert.AreEqual(1, MockFamily.Instances.Count);
        }

        [Test]
        public void TestGetNodeListChecksAllEntities()
        {
            _game.AddEntity(new EntityBase());
            _game.AddEntity(new EntityBase());
            _game.GetNodeList<MockNode>();
            Assert.AreEqual(2, MockFamily.Instances[0].NewEntityCalls);
        }

        [Test]
        public void testReleaseNodeListCallsCleanUp()
        {
            _game.GetNodeList<MockNode>();
            _game.ReleaseNodeList<MockNode>();
            Assert.AreEqual(1, MockFamily.Instances[0].CleanUpCalls);
        }

        class MockNode : Node
        {
            public Vector2 Point { get; set; }
        }

        class MockNode2 : Node
        {
            public Matrix4x4 Matrix { get; set; }
        }
    }
}
