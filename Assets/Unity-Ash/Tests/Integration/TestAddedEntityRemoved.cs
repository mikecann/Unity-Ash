using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Ash.Core
{
    
    public class TestAddedEntityRemoved
    {
        [Test]
        public void Test()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            entity.Add<SpriteRenderer>();

            var nodes = engine.GetNodes<Node<SpriteRenderer>>();

            GameObject.DestroyImmediate(obj);

            Assert.AreEqual(0, nodes.Count());
        }
    }
}
