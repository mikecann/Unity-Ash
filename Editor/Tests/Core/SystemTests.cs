using System;
using System.Collections.Generic;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
    public class SystemTests
    {
        private IGame _game;

        private SystemBase _system1;
        private SystemBase _system2;

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
        public void AddSystemCallsAddToGame()
        {
            var result = new Tuple<string, IGame>(null, null);
            var system = new MockSystem((sys, action, game) => result = new Tuple<string, IGame>(action, (IGame)game));
            _game.AddSystem(system, 0);
            Assert.AreEqual(new Tuple<string, IGame>("added", _game), result);
        }

        [Test]
        public void RemoveSystemCallsRemovedFromGame()
        {
            var result = new Tuple<string, IGame>(null, null);
            var system = new MockSystem((sys, action, game) => result = new Tuple<string, IGame>(action, (IGame)game));
            _game.AddSystem(system, 0);
            _game.RemoveSystem(system);
            Assert.AreEqual(new Tuple<string, IGame>("removed", _game), result);
        }

        [Test]
        public void GameCallsUpdateOnSystems()
        {
            var result = new Tuple<string, object>(null, 0);
            var system = new MockSystem((sys, action, time) => result = new Tuple<string, object>(action, time));
            _game.AddSystem(system, 0);
            _game.Update(0.1f);
            Assert.AreEqual(new Tuple<string, object>("update", 0.1), result);
        }

        [Test]
        public void DefaultPriorityIsZero()
        {
            var system = new MockSystem((sys, action, game) => { });
            Assert.AreEqual(0, system.Priority);
        }

        [Test]
        public void CanSetPriorityWhenAddingSystem()
        {
            var system = new MockSystem((sys, action, game) => { });
            _game.AddSystem(system, 10);
            Assert.AreEqual(10, system.Priority);
        }

        [Test]
        public void SystemsUpdatedInPriorityOrderIfSameAsAddOrder()
        {
            var result = new List<SystemBase>();
            _system1 = new MockSystem((sys, action, time) => result.Add(sys));
            _system2 = new MockSystem((sys, action, time) => result.Add(sys));
            _game.AddSystem(_system1, 10);
            _game.AddSystem(_system2, 20);
            result = new List<SystemBase>();
            _game.Update(0.1f);
            var expected = new List<SystemBase> { _system1, _system2 };
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SystemsUpdatedInPriorityOrderIfReverseOfAddOrder()
        {
            var result = new List<SystemBase>();
            _system1 = new MockSystem((sys, action, time) => result.Add(sys));
            _system2 = new MockSystem((sys, action, time) => result.Add(sys));
            _game.AddSystem(_system2, 20);
            _game.AddSystem(_system1, 10);
            result = new List<SystemBase>();
            _game.Update(0.1f);
            var expected = new List<SystemBase> { _system1, _system2 };
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SystemsUpdatedInPriorityOrderIfPrioritiesAreNegative()
        {
            var result = new List<SystemBase>();
            _system1 = new MockSystem((sys, action, time) => result.Add(sys));
            _system2 = new MockSystem((sys, action, time) => result.Add(sys));
            _game.AddSystem(_system1, 10);
            _game.AddSystem(_system2, -20);
            result = new List<SystemBase>();
            _game.Update(0.1f);
            var expected = new List<SystemBase> { _system2, _system1 };
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UpdatingIsFalseBeforeUpdate()
        {
            Assert.IsFalse(_game.Updating);
        }

        [Test]
        public void UpdatingIsTrueDuringUpdate()
        {
            var updating = false;
            var system = new MockSystem((sys, action, time) => updating = _game.Updating);
            _game.AddSystem(system, 0);
            _game.Update(0.1f);
            Assert.IsTrue(updating);
        }

        [Test]
        public void UpdatingIsFalseAfterUpdate()
        {
            var system = new MockSystem((sys, action, time) => { });
            _game.AddSystem(system, 0);
            _game.Update(0.1f);
            Assert.IsFalse(_game.Updating);
        }

        [Test]
        public void CompleteEventIsDispatchedAfterUpdate()
        {
            var eventDispatched = false;
            _game.UpdateComplete += () => eventDispatched = true;
            var system = new MockSystem((sys, action, time) => { });
            _game.AddSystem(system, 0);
            _game.Update(0.1f);
            Assert.IsTrue(eventDispatched);
        }

        [Test]
        public void GetSystemReturnsTheSystem()
        {
            var system = new MockSystem((sys, action, time) => { });
            _game.AddSystem(system, 0);
            _game.AddSystem(new DummySystem(), 0);
            Assert.AreSame(system, _game.GetSystem(typeof(MockSystem)));
        }

        [Test]
        public void GetSystemReturnsNullIfNoSuchSystem()
        {
            _game.AddSystem(new DummySystem(), 0);
            Assert.IsNull(_game.GetSystem(typeof(MockSystem)));
        }

        [Test]
        public void RemoveAllSystemsDoesWhatItSays()
        {
            _game.AddSystem(new MockSystem((sys, action, time) => { }), 0);
            _game.AddSystem(new DummySystem(), 0);
            _game.RemoveAllSystems();
            var expected = new Tuple<SystemBase, SystemBase>(null, null);
            var results = new Tuple<SystemBase, SystemBase>(_game.GetSystem(typeof(MockSystem)),
                                                            _game.GetSystem(typeof(DummySystem)));
            Assert.AreEqual(expected, results);
        }

        [Test]
        public void RemoveSystemAndAddItAgainDontCauseInvalidLinkedList()
        {
            _system1 = new DummySystem();
            _system2 = new DummySystem();
            _game.AddSystem(_system1, 0);
            _game.AddSystem(_system2, 0);
            _game.RemoveSystem(_system1);
            _game.AddSystem(_system1, 0);
            _game.Update(0.1f);
            var expected = new Tuple<SystemBase, SystemBase>(null, null);
            var results = new Tuple<SystemBase, SystemBase>(_system2.Previous, _system1.Next);
            Assert.AreEqual(expected, results);
        }
    }
}
