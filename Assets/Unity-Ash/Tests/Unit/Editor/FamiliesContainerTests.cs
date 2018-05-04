using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    [TestFixture]
    public class FamiliesContainerTests
    {
        private FamiliesContainer _container;

        [SetUp]
        public void Setup()
        {
            _container = new FamiliesContainer();
        }

        [Test]
        public void AfterAdding_CanBeRetrieved()
        {
            var type = typeof (Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Add(type, family);

            Assert.IsTrue(_container.Get(type) == family);
        }

        [Test]
        public void AfterAdding_IsContained()
        {
            var type = typeof(Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Add(type, family);

            Assert.IsTrue(_container.Contains(type));
        }

        [Test]
        public void AfterAddingWhileLocked_IsContained()
        {
            var type = typeof(Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Lock();
            _container.Add(type, family);

            Assert.IsTrue(_container.Contains(type));
        }

        [Test]
        public void AfterAddingThenRemovingWhileLocked_IsNotContained()
        {
            var type = typeof(Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Lock();
            _container.Add(type, family);
            _container.Remove(type);

            Assert.IsFalse(_container.Contains(type));
        }

        [Test]
        public void AfterRemovingWhileLocked_IsNotContained()
        {
            var type = typeof(Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Add(type, family);
            _container.Lock();
            _container.Remove(type);

            Assert.IsFalse(_container.Contains(type));
        }

        [Test]
        public void AfterRemoving_RetrievalThrowsError()
        {
            var type = typeof (Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Add(type, family);
            _container.Remove(type);

            Assert.Throws<Exception>(() => _container.Get(type));
        }

        [Test]
        public void AfterRemoving_DoesNotContain()
        {
            var type = typeof(Node<SpriteRenderer>);
            var family = Substitute.For<IFamily>();

            _container.Add(type, family);
            _container.Remove(type);

            Assert.IsFalse(_container.Contains(type));
        }

        [Test]
        public void CanBeIterated()
        {
            var type1 = typeof (Node<SpriteRenderer>);
            var family1 = Substitute.For<IFamily>();
            var type2 = typeof (Node<Transform>);
            var family2 = Substitute.For<IFamily>();

            _container.Add(type1, family1);
            _container.Add(type2, family2);

            foreach (var family in _container)
                Assert.IsTrue(family == family1 || family == family2);
        }

        [Test]
        public void WhenLocked_FamiliesCanBeAddedWhileIterating()
        {
            _container.Add(typeof(Node<SpriteRenderer>), Substitute.For<IFamily>());
            _container.Add(typeof(Node<Transform>), Substitute.For<IFamily>());

            _container.Lock();

            foreach (var family in _container)
                _container.Add(typeof(Node<BoxCollider2D>), Substitute.For<IFamily>());
        }

        [Test]
        public void AfterLocking_FamilyCanBeRetrieved()
        {
            _container.Add(typeof(Node<SpriteRenderer>), Substitute.For<IFamily>());
            _container.Add(typeof(Node<Transform>), Substitute.For<IFamily>());

            _container.Lock();

            foreach (var family in _container)
                _container.Add(typeof(Node<BoxCollider2D>), Substitute.For<IFamily>());

            _container.UnLock();

            _container.Get(typeof (Node<BoxCollider2D>));
        }

        [Test]
        public void WhenLocked_AddedFamilyCanStillBeRetrieved()
        {
            _container.Add(typeof(Node<SpriteRenderer>), Substitute.For<IFamily>());
            _container.Add(typeof(Node<Transform>), Substitute.For<IFamily>());

            _container.Lock();

            foreach (var family in _container)
                _container.Add(typeof(Node<BoxCollider2D>), Substitute.For<IFamily>());

            _container.Get(typeof(Node<BoxCollider2D>));
        }

        [Test]
        public void PendingAreCleared()
        {
            _container.Lock();
            _container.Add(typeof(Node<SpriteRenderer>), Substitute.For<IFamily>());
            _container.UnLock();

            _container.Remove(typeof(Node<SpriteRenderer>));

            _container.Lock();
            _container.UnLock();

            Assert.Throws<Exception>(() => _container.Get(typeof(Node<SpriteRenderer>)));
        }
    }
}
