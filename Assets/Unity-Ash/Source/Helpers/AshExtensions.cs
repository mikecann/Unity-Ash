using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ash.Core;
using Ash.Helpers;
using UnityEngine;

public static class AshEngineExtensions
{
    public static void First<T>(this IEngine engine, Action<T> callback) where T : Component
    {
        var nodes = engine.GetNodes<Node<T>>();
        if (nodes.Any())
            callback(nodes.First().Component1);
    }

    public static Maybe<T> First<T>(this IEngine engine) where T : Component
    {
        var nodes = engine.GetNodes<Node<T>>();
        if (nodes.Any())
            return new Maybe<T>(nodes.First().Component1);
        else
            return new Maybe<T>(null);
    }

    public static void ForEach<T1>(this IEngine engine, Action<T1> callback) where T1 : Component
    {
        var nodes = engine.GetNodes<Node<T1>>();
        foreach (var n in nodes)
            callback(n.Component1);
    }

    public static void ForEach<T1, T2>(this IEngine engine, Action<T1, T2> callback) 
        where T1 : Component where T2 : Component
    {
        var nodes = engine.GetNodes<Node<T1, T2>>();
        foreach (var n in nodes)
            callback(n.Component1, n.Component2);
    }

    public static void ForEach<T1, T2, T3>(this IEngine engine, Action<T1, T2, T3> callback) 
        where T1 : Component where T2 : Component where T3 : Component
    {
        var nodes = engine.GetNodes<Node<T1, T2, T3>>();
        foreach (var n in nodes)
            callback(n.Component1, n.Component2, n.Component3);
    }

    public static void ForEach<T1, T2, T3, T4>(this IEngine engine, Action<T1, T2, T3, T4> callback)
        where T1 : Component where T2 : Component where T3 : Component where T4 : Component
    {
        var nodes = engine.GetNodes<Node<T1, T2, T3, T4>>();
        foreach (var n in nodes)
            callback(n.Component1, n.Component2, n.Component3, n.Component4);
    }

    public static void RemoveAll<T1>(this IEngine engine) where T1 : Component
    {
        var nodes = engine.GetNodes<Node<T1>>();
        while(nodes.Any())
            nodes.Last().Component1.Remove();
    }

    public static void Remove<T>(this Entity entity) where T : Component
    {
        var component = entity.GetComponent<T>();
        if (component != null)
            entity.Remove(component);
    }

    public static T Add<T>(this Component component) where T : Component
    {
        var entity = component.GetComponent<Entity>();
        if (entity == null)
            throw new Exception("Cant add component, this component doesnt have an Entity component attached to the GameObject");

        return entity.Add<T>();
    }

    public static void Remove<T>(this Component component) where T : Component
    {
        var entity = component.GetComponent<Entity>();
        if (entity == null)
            throw new Exception("Cant remove component, this component doesnt have an Entity component attached to the GameObject");

        entity.Remove<T>();
    }

    public static void Remove(this Component component)
    {
        var entity = component.GetComponent<Entity>();
        if (entity == null)
            throw new Exception("Cant remove component, this component doesnt have an Entity component attached to the GameObject");

        entity.Remove(component);
    }

    public static bool Has<T>(this IEntity entity)
    {
        return entity.Has(typeof(T));
    }

    public static T Get<T>(this IEntity entity) where T : Component
    {
        return (T)entity.Get(typeof(T));
    }
}