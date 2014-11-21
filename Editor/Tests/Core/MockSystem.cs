using System;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    class MockSystem : SystemBase
    {
        private readonly Action<SystemBase, string, object> _callback;

        public MockSystem(Action<SystemBase, string, object> callback)
        {
            _callback = callback;
        }

        override public void AddToGame(IGame game)
        {
            _callback(this, "added", game);
        }

        override public void RemoveFromGame(IGame game)
        {
            _callback(this, "removed", game);
        }


        public override void Update(float time)
        {
            _callback(this, "update", time);
        }
    }
}
