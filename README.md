![logo](http://i.imgur.com/Wpsk1fy.png)

Unity-Ash
=============

A working port of [Richard Lord's Ash Game Framework](https://github.com/richardlord/Ash) for Unity. It uses [David Arno's .Net port](https://github.com/DavidArno/Ash.NET) of Ash for most of the heavy-lifting.

More info on the blog post: http://www.mikecann.co.uk/myprojects/unityasteroids/unity-ashteroids-ash-framework-in-unity/

Usage
-----

1. Extend AshGame, add your systems in Awake (see [Unity Ashteroids](https://github.com/mikecann/UnityAshteroids) for an example of this)
2. Create a GameObject and add your new AshGame Component.
3. Create your GameObjects as usual but Add an Entity to each one. 

Thats it! The Entity will detect when components or added. The Systems will be updated with the latest NodeLists based on the component changes.

Notes on Performance
-----------

Entity works by calling GetComponents<> in its update loop then checking to see if any components have been added or removed since the last update. As such this is quite an expensive process so Entity contains an "updateFrequency" variable that you can use to toggle how often you want Entity to check for component changes. 

![update frequency](http://i.imgur.com/mK5oWBW.png)

If you know that a type of GameObject will never have components added or removed then set the updateFrequency to Never and enjoy native performance.

Known Issues
------------

+ Although Unity allows for multiple components of the same type Ash doesn't, don't try to use multiple components of the same type.

Potential Improvements
----------------------

### List Iterating System

AS3 had the ListIteratingSystem which simplified iterating over nodes in a NodeList. With proper generics in C# we should be able to improve on that.

For example the current BulletAgeSystem from the [Unity Ashteroids](https://github.com/mikecann/UnityAshteroids) example project currently looks like:

```
public class BulletAgeSystem : SystemBase
{
    private EntityCreator creator;

    private NodeList nodes;

    public BulletAgeSystem(EntityCreator creator)
    {
        this.creator = creator;
    }

    override public void AddToGame(IGame game)
    {
        nodes = game.GetNodeList<BulletAgeNode>();
    }

    override public void Update(float time)
    {
        for (var node = (BulletAgeNode)nodes.Head; node != null; node = (BulletAgeNode)node.Next)
        {
            var bullet = node.Bullet;
            bullet.lifeRemaining -= time;
            if (bullet.lifeRemaining <= 0)
            {
                creator.DestroyEntity(node.Entity);
            }
        }
    }

    override public void RemoveFromGame(IGame game)
    {
        nodes = null;
    }
}
```

With a ListIteratingSystem it could look like:

```
public class ListIteratingSystem<BulletAgeNode>
{
    private EntityCreator creator;

    public BulletAgeSystem(EntityCreator creator)
    {
        this.creator = creator;
    }

    override public void Update(BulletAgeNode node, float time)
    {
        var bullet = node.Bullet;
        bullet.lifeRemaining -= time;
        if (bullet.lifeRemaining <= 0)
        {
            creator.DestroyEntity(node.Entity);
        }
    }
}
```

The ListIteratingSystem could contain variants that support multiple node list e.g. ListIteratingSystm<T1,T2,T3,T4>

The ListIteratingSystem would improve total performance too as it would remove the uneccessary casting of nodes.

### Remove Linked Lists

Ash in AS3 used linked lists for much of its internals (NodeList, SystemList, EntityList) due to performance issues in AS3. 

In C# we have generic Lists and thus the performance issues are much less of a worry and so we can probably do away with the linked lists and use normal Lists instead which would greatly simplify things.

### Simplify Confusing Entity / EntityBase

Currently Entity extends EntityBase which contains Add / Remove component methods. This is because EntityBase was taken straight from the .Net port (with minor changes).

Ideally Entity and EntityBase should be merged. Adding a component to Entity should just add the component to the GameObject.

### Simplify Components

There shouldnt be a disconnect between an Ash Component and a Unity Component.

Ash Components dont have to extend UnityEngine.Component whereas Unity components do. 

Because this is Ash for Unity we should follow Unity and force the fact that all components should extend UnityEngine.Component.

### Remove Time From Systems

Because we are dealing with Unity we have access to the UnityEngine.Time class which gives us much more info about the current frame and thus systems dont need to have time passed to them in their Update methods.

### Generic Nodes

Nodes could be made generic so that we dont have to cast when accessing Next and Previous, for example currently its

```
var movement = (MovementNode)node.Next
```

If nodes were generic then it would become simply:

```
var movement = node.Next;
```

