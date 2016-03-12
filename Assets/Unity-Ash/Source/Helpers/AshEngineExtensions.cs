using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;
using UnityEngine;

public static class AshEngineExtensions
{
    public static void ForEach<T>(this Engine engine, Action<T> callback) where T : Component
    {
        var nodes = engine.GetNodes<Node<T>>();
        foreach (var n in nodes)
            callback(n.Component1);
    }

}