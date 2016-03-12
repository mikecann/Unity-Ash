using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    [IntegrationTest.DynamicTest("Main")]
    public class TestNewFamilyCanBeCreatedDuringNodeRemove : MonoBehaviour
    {
        void Start()
        {
            var engine = new Engine();
            
            var nodes1 = engine.GetNodes<Node<Rigidbody>>();
            nodes1.NodeRemovedEvent.AddListener(n => engine.GetNodes<Node<Transform>>());

            var obj = new GameObject();
            var entity = obj.AddComponent<Entity>();
            entity.Add<Rigidbody>();
            entity.Remove<Rigidbody>();

            IntegrationTest.Pass();
        }
    }
}
