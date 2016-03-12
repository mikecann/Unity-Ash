using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ash.Core
{
    enum PendingChange
    {
        Add,
        Remove
    }

    class PendingChangeFamilyPair
    {
        public IFamily Family { get; set; }
        public PendingChange Change { get; set; }
    }

    public class FamiliesContainer : IEnumerable<IFamily>
    {
        private Dictionary<Type, IFamily> _families;
        private Dictionary<Type, PendingChangeFamilyPair> _pending;
        private bool _isLocked;

        public FamiliesContainer()
        {
            _families = new Dictionary<Type, IFamily>();
            _pending = new Dictionary<Type, PendingChangeFamilyPair>();
        }
 
        public void Add(Type nodeType, IFamily family)
        {
            if (_isLocked)
            {
                _pending[nodeType] = new PendingChangeFamilyPair
                {
                    Change = PendingChange.Add,
                    Family = family
                };
            }
            else
            {
                _families[nodeType] = family;
            }
        }

        public IFamily Get(Type nodeType)
        {
            if (_isLocked)
            {
                if (_pending.ContainsKey(nodeType) && _pending[nodeType].Change == PendingChange.Add)
                    return _pending[nodeType].Family;
            }
            
            if (!_families.ContainsKey(nodeType))
                throw new Exception("Cannot get Family, is not within container");

            return _families[nodeType];
        }

        public bool Contains(Type nodeType)
        {
            if (_pending.ContainsKey(nodeType))
                return _pending[nodeType].Change == PendingChange.Add;

            return _families.ContainsKey(nodeType);
        }

        public void Remove(Type type)
        {
            if (_isLocked)
            {
                _pending[type] = new PendingChangeFamilyPair
                {
                    Change = PendingChange.Remove
                };
            }
            else
                _families.Remove(type);
        }

        public IEnumerator<IFamily> GetEnumerator()
        {
            return _families.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Lock()
        {
            _isLocked = true;
        }

        public void UnLock()
        {
            _isLocked = false;

            foreach (var pair in _pending)
            {
                if (pair.Value.Change == PendingChange.Add)
                    Add(pair.Key, pair.Value.Family);
                if (pair.Value.Change == PendingChange.Remove)
                    Remove(pair.Key);
            }

            _pending.Clear();
        }
    }
}
