﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    
    public class TestNewFamilyCanBeCreatedDuringNodeAdd
    {
        [Test]
        public void Test()
        {
            var engine = new Engine();

            var nodes1 = engine.GetNodes<Node<Rigidbody>>();
            nodes1.NodeAddedEvent.AddListener(n => engine.GetNodes<Node<Transform>>());

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            entity.Add<Rigidbody>();
        }
    }
}
