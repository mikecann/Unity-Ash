using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    [TestFixture]
    public class EntityTests : UnityUnitTest
    {

        [Test]
        public void AddingAComponent_AddsItToGameObjectAndInvokesEvent()
        {
            var obj = CreateGameObject();
            var entity = obj.AddComponent<Entity>();
            entity.AddComponent<MockUnityComponent>();


        }
    }
}
