using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestMatchingEntityAdded
    {
        [Test]
        public void Test()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            var renderer = entity.Add<SpriteRenderer>();

            var nodes = engine.GetNodes<Node<SpriteRenderer>>();

            Assert.AreEqual(1, nodes.Count());
            Assert.AreEqual(renderer, nodes.First().Component1);
        }
    }
}
