using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Net.RichardLord.AshTests.Core
{
    public class TestingHelpers
    {
        public static void CallMethod(object obj, string name, object[] methodParams = null)
        {
            var dynMethod = obj.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(obj, methodParams);
        }
    }

    public class BaseTest
    {
        private List<GameObject> createdObjects = new List<GameObject>();

        [TearDown]
        public void ClearCreatedObjects()
        {
            foreach (var item in createdObjects)
                GameObject.DestroyImmediate(item);
        }

        [SetUp]
        public void CreateEntity()
        {
            createdObjects.Clear();
        }

        public GameObject CreateGameObject (string name = null)
	    {
		    var go = string.IsNullOrEmpty(name) ? new GameObject() : new GameObject(name);
            createdObjects.Add(go);
		    return go;
	    }

    }
}
