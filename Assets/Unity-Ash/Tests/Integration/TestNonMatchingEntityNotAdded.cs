﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestNonMatchingEntityNotAdded
    {
        [Test]
        public void Test()
        {
            var engine = new Engine();

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            entity.Add<SpriteRenderer>();

            var nodes = engine.GetNodes<Node<Rigidbody>>();

            Assert.AreEqual(0, nodes.Count());
        }
    }
}
