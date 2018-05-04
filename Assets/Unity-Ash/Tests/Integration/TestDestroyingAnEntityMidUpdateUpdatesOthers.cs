using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace Ash.Core
{
    public class TestDestroyingAnEntityMidUpdateUpdatesOthers
    {
        [Test]
        public void Test()
        {
            //var engine = new Engine();

            //var firstPass = new Dictionary<Entity, bool>();

            //firstPass[AddEntity()] = false;
            //firstPass[AddEntity()] = false;
            //firstPass[AddEntity()] = false;
            //firstPass[AddEntity()] = false;

            //var secondPass = new Dictionary<Entity, bool>(firstPass);

            //var nodes = engine.GetNodes<Node<Entity, SpriteRenderer>>();

            //var i = 0;
            //foreach (var node in nodes)
            //{
            //    firstPass[node.component1] = true;
            //    if (i++ == 2)
            //        node.component1.Destroy();
            //}

            //foreach (var node in nodes)
            //{
            //    secondPass[node.component1] = true;
            //}

            //IntegrationTest.Pass();
        }

        //private Entity AddEntity()
        //{
        //    var obj = new GameObject();
        //    var entity = obj.AddComponent<Entity>();
        //    var renderer = entity.Add<SpriteRenderer>();
        //    return entity;
        //}
    }
}
