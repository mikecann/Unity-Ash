using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestEntity
    {
        private FakeEntity _entity;
        private IEngine _engine;

        [SetUp]
        public void Setup()
        {
            FakeEntity.engine = _engine = Substitute.For<IEngine>();
            _entity = new GameObject().AddComponent<FakeEntity>();
        }

        [Test]
        public void DuringAwake_EntityAddedToEngine()
        {
            _engine.Received(1).AddEntity(_entity);
        }

        [Test]
        public void AfterAwkening_EntityDetectsExistingComponents()
        {
            Assert.IsFalse(_entity.Has<SpriteRenderer>());
        }

        [Test]
        public void AfterAddingComponentInternally_EntityHasComponent()
        {
            _entity.Add<SpriteRenderer>();
            Assert.IsTrue(_entity.Has<SpriteRenderer>());
        }

        [Test]
        public void AfterRemovingComponentInternally_EntityNoLongerHasComponent()
        {
            _entity.Add<SpriteRenderer>();
            _entity.Remove<SpriteRenderer>();
            Assert.IsFalse(_entity.Has<SpriteRenderer>());
        }

        [Test]
        public void AddingComponentInternally_DispatchesAddedEvent()
        {
            var added = 0;
            _entity.ComponentAdded.AddListener((e, t) => added++);
            _entity.Add<SpriteRenderer>();
            Assert.AreEqual(1, added);
        }

        [Test]
        public void RemovingComponentInternally_DispatchesRemovedEvent()
        {
            var removed = 0;
            _entity.ComponentAdded.AddListener((e, t) => removed++);
            _entity.Add<SpriteRenderer>();
            _entity.Remove<SpriteRenderer>();
            Assert.AreEqual(1, removed);
        }

        [Test]
        public void WhenEntityGameObjectDestroyed_EngineInformed()
        {
            GameObject.DestroyImmediate(_entity.gameObject);
            _engine.Received(1).RemoveEntity(Arg.Any<IEntity>());
        }

        [Test]
        public void WhenEntityComponentRemoved_EngineInformed()
        {
            GameObject.DestroyImmediate(_entity);
            _engine.Received(1).RemoveEntity(_entity);
        }

        //[Test]
        //public void IfAComponentIsAddedExternally_EntityHasComponent()
        //{
        //    _entity.gameObject.AddComponent<SpriteRenderer>();
        //    Assert.IsTrue(_entity.Has<SpriteRenderer>());
        //}

        //[Test]
        //public void IfAComponentIsRemovedExternally_EntityDoesNotHaveComponent()
        //{
        //    _entity.Add<SpriteRenderer>();
        //    GameObject.DestroyImmediate(_entity.gameObject.GetComponent<SpriteRenderer>());
        //    Assert.IsFalse(_entity.Has<SpriteRenderer>());
        //}

        //[Test]
        //public void IfAComponentIsAddedInternally_ThenRemovedExternally_ComponentRemovedEventDispatchedWhenMissingComponentNoticed()
        //{
        //    var componentsRemoved = 0;
        //    _entity.Add<SpriteRenderer>();
        //    _entity.ComponentRemoved.AddListener((e,t) => componentsRemoved++);
        //    Assert.AreEqual(0, componentsRemoved);
        //    GameObject.DestroyImmediate(_entity.gameObject.GetComponent<SpriteRenderer>());
        //    Assert.AreEqual(0, componentsRemoved);
        //    Assert.IsFalse(_entity.Has<SpriteRenderer>());
        //    Assert.AreEqual(1, componentsRemoved);
        //}
    }
}