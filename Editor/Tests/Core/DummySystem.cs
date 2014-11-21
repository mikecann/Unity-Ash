using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    class DummySystem : SystemBase
    {
        public override void AddToGame(IGame game) {}
        public override void RemoveFromGame(IGame game) {}
        public override void Update(float time) {}
    }
}
