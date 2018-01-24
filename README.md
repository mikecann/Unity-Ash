![logo](http://i.imgur.com/Wpsk1fy.png)

Unity-Ash
=============

An Entity / Component framework for Unity heavily inspired by [Richard Lord's Ash Game Framework](https://github.com/richardlord/Ash).

More information on Unity-Ash can be found in this blog post: https://mikecann.co.uk/programming/projects/unity/unity-ash/unityasteroids/unity-ash-a-different-way-of-thinking-about-making-games-in-unity/

Examples
-----

[Unity Ashteroids](https://github.com/mikecann/UnityAshteroids)

Download
----

Download the latest version: https://github.com/mikecann/Unity-Ash/releases/latest

Usage
----

There currently isnt a .unitypackage while the framework in is in flux, instead just clone this repo and copy the contents of "Assets/Libraries/Unity-Ash/Source" to your project.

Checkout [Ashteroids](https://github.com/mikecann/UnityAshteroids) for a full example but some quick tips:

+ Add the Entity Monobehaviour to any GameObject you wish to be detected by Ash.
+ Dont add or remove components from GameObject directly, instead use Entity.Add() and Remove().

Tests
-----

The project uses Unity Test tools for unit and intergration tests. Open the test tools from the menu in Unity and run.

Differences from Ash
----

There are a number of differences from Ash which is why this project is not a straight port and is instead inspired by the Ash Framework.

+ NodeList's are externally immutable
+ Explicit Node classes are optional, the generic Node class can be used instead.
+ Nodes can be of any type but the components must be exposed as Properties
+ For performance reasons Entities use a static lookup for the current Engine.
+ Systems dont contain the Priority property, instead it is supplied when added to the Engine.
