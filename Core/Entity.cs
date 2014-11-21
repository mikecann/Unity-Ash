using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Net.RichardLord.Ash.Core
{
    sealed public class Entity : EntityBase
    {                      
        private IGame _engine;
        public IGame Engine 
        {
            get { return _engine; }
            set
            {
                if (_engine!=null) _engine.RemoveEntity(this);
                _engine = value;
                if (_engine != null) _engine.AddEntity(this);
            }
        }

        public UpdateFrequency updateFrequency = UpdateFrequency.EveryFrame;

        private IGame engine;
        private List<Component> componentCache;
        private int framesSinceLastUpdate;
        private int lastComponentCount;
        private List<Type> componentsToRemove;

        private void Awake()
        {
            componentsToRemove = new List<Type>();
            componentCache = new List<Component>();
            GetComponents<Component>(componentCache);
            AddNewComponents(componentCache);
        }

        void Start()
        {
            // Delaying looking for the engine until we start, this is so that creation code
            // can properly parent this entity
            Engine = FindEngine();
        }

        private IGame FindEngine()
        {
            // First try to find the game in the parent which should be relatively fast.
            var game = GetComponentInParent<AshGame>();
            if (game == null)
            {
                // If it cant find it we should warn the user
                Debug.LogWarning("For performance reasons your Entity should be a child of the AshGame");

                // This is an expensive call and a last ditch attempt to find the Ash Game
                game = GameObject.FindObjectOfType<AshGame>();
            }

            // If we still couldnt find the game then die.
            if (game == null) throw new Exception("Could not find find the AshGame in parent tree!");

            return game.Engine;
        }

        private void Update()
        {
            // We restrict how often we update for performance
            var shouldUpdate = false;
            if (updateFrequency == UpdateFrequency.EveryFrame) 
                shouldUpdate = true;
            else if (updateFrequency == UpdateFrequency.EveryOtherFrame && framesSinceLastUpdate % 2 == 0) 
                shouldUpdate = true;
            else if (updateFrequency == UpdateFrequency.Every10Frames && framesSinceLastUpdate % 10 == 0) 
                shouldUpdate = true;
             else if (updateFrequency == UpdateFrequency.IfComponentCountChanges)
                shouldUpdate = true;

            // For performance only update if its the right time to
            if (shouldUpdate)
            {
                // Only do the expensive stuff if we need to
                if (updateFrequency == UpdateFrequency.IfComponentCountChanges)
                {
                    var lastCount = componentCache.Count;
                    GetComponents<Component>(componentCache);
                    if(componentCache.Count != lastCount)
                    {
                        AddNewComponents(componentCache);
                        RemoveOldComponents(componentCache);
                    }
                }
                else
                {
                    GetComponents<Component>(componentCache);
                    AddNewComponents(componentCache);
                    RemoveOldComponents(componentCache);
                }   
            }            
            framesSinceLastUpdate++;
        }

        private void AddNewComponents(List<Component> components)
        {
            foreach (var component in components)
            {
                if (!Has(component.GetType()))
                    Add(component);
            }
        }

        private void RemoveOldComponents(List<Component> components)
        {
            componentsToRemove.Clear();
            foreach (var pair in Components)
            {
                var found = false;
                foreach(var component in components)
                {
                    if (component == pair.Value)
                    {
                        found = true;
                        break;
                    }
                }               
                if (!found)
                    componentsToRemove.Add(pair.Key);
            }
            foreach (var type in componentsToRemove)
                Remove(type);
        }

        private void OnDestroy()
        {
            if (Engine == null) return;
            Engine.RemoveEntity(this);
        }

    }
}
